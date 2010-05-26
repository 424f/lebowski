namespace Lebowski.TextModel
{
    using System;
        
    /// <summary>
    /// Provides data for the <see cref="ITextContext.Changed">Change</see> event.
    /// </summary>    
    public class ChangeEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the ChangeEventArgs class.
        /// </summary>
        /// <param name="issuer">See <see cref="Issuer">Issuer</see>.</param>
        public ChangeEventArgs(object issuer)
        {
            Issuer = issuer;
        }

        /// <summary>
        /// The object responsible for the change
        /// </summary>
        public object Issuer { get; protected set; }        
    }
}
