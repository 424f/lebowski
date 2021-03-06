namespace Lebowski.Synchronization.DifferentialSynchronization
{
    using System;
    using System.IO;
    using System.Timers;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Diagnostics;
    using Lebowski.TextModel;
    using Lebowski.Net;
    using DiffMatchPatch;

    /// <summary>
    /// This class provides an implementation of the Differential Synchronization
    /// synchronization algorithm by Neil Fraser. It is based around sending patches
    /// of local changes to other sites and is described in detail in [1].
    ///
    /// Our current implementation has the following limitations:
    ///     - Does only allow for two sites
    ///     - Uses a simplistic cursor preservation technique that does not
    ///       work in all cases
    ///
    /// [1] http://neil.fraser.name/writing/sync/
    /// </summary>
    public sealed class DifferentialSynchronizationStrategy : ISynchronizationStrategy
    {
        // Constant used to give a unique identifier to the server site
        private const int ServerId = 0;

        /// <summary>
        /// A unique integer identifying this client in the current session.
        /// Use ServerId for the server and ServerId + i for the i-th client.
        /// </summary>
        public int SiteId { get; private set; }

        /// <summary>
        /// The connection that is used to communicate with the other site
        /// </summary>
        private IConnection Connection;

        /// <summary>
        /// The text context that remote changes are applied to and which is monitored
        /// for local changes.
        /// </summary>
        private ITextContext Context;

        /// <summary>
        /// A shadow text context that represents the last known state of the
        /// remote site
        /// </summary>
        private StringTextContext Shadow;

        /// <summary>
        /// Indicates whether there are local changes that have not yet been
        /// propagated to other clients.
        /// </summary>
        public bool HasChanged { get; private set; }

        /// <summary>
        /// Indicates whether this client currently holds the token.
        /// </summary>
        public TokenState TokenState { get; private set; }

        /// <summary>
        /// Indicates whether this client has already sent a TokenRequestMessage
        /// to the other participant.
        /// </summary>
        public bool TokenRequestSent { get; private set; }

        // We use diff-match-patch for (you guessed it) diffing, matching and patching
        private diff_match_patch DiffMatchPatch = new diff_match_patch();

        /// <summary>
        /// Creates a new differential synchronization session on an already established
        /// networking connection.
        /// </summary>
        /// <param name="siteId">The <see cref="SiteId">SiteId</see> for this site.</param>
        /// <param name="context">The <see cref="Context">text context</see> to be operate on.</param>
        /// <param name="connection">The connection that is used to communicate with the other site.</param>
        public DifferentialSynchronizationStrategy(int siteId, ITextContext context, IConnection connection)
        {
            SiteId = siteId;
            DiffMatchPatch =  new diff_match_patch();
            Context = context;
            Connection = connection;
            Shadow = new StringTextContext(context.Data);
            TokenRequestSent = false;

            HasChanged = false;

            TokenState = SiteId == 0 ? TokenState.HavingToken : TokenState.WaitingForToken;

            Connection.Received += ConnectionReceived;
            Context.Changed += ContextChanged;
        }

        private void SendPatches()
        {
            Debug.Assert(TokenState == TokenState.HavingToken);

            // Create diff locally
            var localDiffs = DiffMatchPatch.diff_main(Shadow.Data, Context.Data);

            // Apply patch to shadow
            var patch = DiffMatchPatch.patch_make(Shadow.Data, localDiffs);
            Shadow.Data = (string)DiffMatchPatch.patch_apply(patch, Shadow.Data)[0];

            var delta = DiffMatchPatch.diff_toDelta(localDiffs);

            Connection.Send(new DiffMessage(delta, Context.SelectionStart, Context.SelectionEnd));
        }

        private void ApplyPatches(DiffMessage diffMessage)
        {
            Debug.Assert(TokenState == TokenState.WaitingForToken);
            lock(this)
            {
                Context.Invoke((Action)delegate {
                    InvokedApplyPatches(diffMessage);
                });
            }
        }
        
        private void InvokedApplyPatches(DiffMessage diffMessage)
        {
            string old = Context.Data;

            // Apply diff to shadow
            var diffs = DiffMatchPatch.diff_fromDelta(Shadow.Data, diffMessage.Delta);
            var patches = DiffMatchPatch.patch_make(Shadow.Data, diffs);
            Shadow.Data = (string)DiffMatchPatch.patch_apply(patches, Shadow.Data)[0];

            /* For the real context, we have to perform each operation by hand,
            so the context can do things like moving around the selection area */
            foreach (Patch p in patches)
            {
                System.Console.WriteLine(p);
            }

            //Console.WriteLine("--");
            //Console.WriteLine("Selection was: {0} {1} = '{2}'", Context.SelectionStart, Context.SelectionEnd, Context.SelectedText);
            //Console.WriteLine("Caret was: {0}", Context.CaretPosition);

            bool hadSelection = Context.HasSelection;
            int start = Context.SelectionStart;
            int end = Context.SelectionEnd;
            int caret = Context.CaretPosition;

            // TODO: check whether patches we're applied at all

            // We restore the offset locations using absolute referencing
            // See: http://neil.fraser.name/writing/cursor/
            foreach (Patch patch in patches)
            {
                //Console.WriteLine("Patch = {0} ({1}, {2}, {3}, {4})", patch, patch.start1, patch.start2, patch.length1, patch.length2);
                int index = patch.start1;
                //Console.WriteLine("index {0}, start {1}, end {2}, caret {3}", index, start, end, caret);
                foreach (Diff diff in patch.diffs)
                {
                    //Console.WriteLine("Diff {0}", diff.ToString());
                    switch(diff.operation)
                    {
                        case Operation.DELETE:
                            if (start > index)
                            {
                                start -= diff.text.Length;
                            }
                            if (end > index)
                            {
                                end -= diff.text.Length;
                            }
                            if (caret >= index)
                            {
                                caret -= diff.text.Length;
                            }
                            break;

                        case Operation.EQUAL:
                            index += diff.text.Length;
                            break;

                        case Operation.INSERT:
                            if (start >= index)
                            {
                                start += diff.text.Length;
                            }
                            if (end > index)
                            {
                                end += diff.text.Length;
                            }
                            if (caret >= index)
                            {
                                caret += diff.text.Length;
                            }
                            index += diff.text.Length;
                            break;
                    }
                    //Console.WriteLine("index {0}, start {1}, end {2}, caret {3}", index, start, end, caret);
                }
            }
            start = Math.Max(0, start);
            caret = Math.Max(0, caret);
            Context.Data = (string)DiffMatchPatch.patch_apply(patches, Context.Data)[0];

            end = Math.Min(Context.Data.Length, end);
            start = Math.Min(Context.Data.Length, start);
            caret = Math.Min(Context.Data.Length, caret);

            if (end < start)
            {
                end = start;
            }
            if (hadSelection)
            {
                Context.SetSelection(start, end);
            }
            Context.CaretPosition = caret;
            //Console.WriteLine("Selection is: {0} {1}", start, end);
            //Console.WriteLine("Caret is: {0}", caret);
            //Console.WriteLine("Was {0} now {1}", old, Context.Data);

            // Display remote selection
            // TODO: when adding support for multiple users, we need to actually provide a real identifier
            Context.SetRemoteSelection(0, diffMessage.SelectionStart, diffMessage.SelectionEnd);

            Context.Refresh();            
        }

        /// <summary>
        /// This will return the tokens that this site currently has to the
        /// other participants, thus ensuring that they can propagate their
        /// changes back to us.
        /// </summary>
        private void FlushToken()
        {
            if (TokenState == TokenState.HavingToken)
            {
                SendPatches();
                TokenState = TokenState.WaitingForToken;
            }
        }

        private void ConnectionReceived(object sender, ReceivedEventArgs e)
        {
            Context.Invoke(delegate {
                InvokedConnectionReceived(sender, e);
            });
        }
        
        private void InvokedConnectionReceived(object sender, ReceivedEventArgs e)
        {
            lock(this)
            {
                if (e.Message is DiffMessage)
                {
                    Debug.Assert(TokenState == TokenState.WaitingForToken);
                    TokenRequestSent = false;
                    DiffMessage diffMessage = (DiffMessage)e.Message;

                    ApplyPatches(diffMessage);
                    TokenState = TokenState.HavingToken;

                    if (HasChanged)
                    {
                        HasChanged = false;
                        SendPatches();
                        TokenState = TokenState.WaitingForToken;
                    }
                }
                else if (e.Message is TokenRequestMessage)
                {
                    if (TokenState == TokenState.HavingToken)
                    {
                        HasChanged = false;
                        SendPatches();
                        TokenState = TokenState.WaitingForToken;
                    }
                }
                else
                {
                    throw new Exception(String.Format("Encountered unknown message type '{0}'", e.Message.GetType().Name));
                }
            }           
        }

        private void ContextChanged(object sender, ChangeEventArgs e)
        {
            if (e.Issuer == this)
                return;
            // If we currently have the token, we can just send this single change...
            if (TokenState == TokenState.HavingToken)
            {
                HasChanged = false;
                SendPatches();
                TokenState = TokenState.WaitingForToken;
            }
            // .. otherwise we first have to request it.
            else
            {
                if (!TokenRequestSent)
                {
                    Connection.Send(new TokenRequestMessage());
                }
                TokenRequestSent = true;
                HasChanged = true;
            }
        }

        /// <inheritdoc />
        public void Close()
        {
            Context.Changed -= ContextChanged;
            Connection.Received -= ConnectionReceived;
        }
    }
}