


namespace TwinEditor
{
    using System;
    using System.IO;
    using System.Windows.Forms;
    using System.Configuration;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using Lebowski;
    using Lebowski.Net;
    using Lebowski.Net.Tcp;
    using Lebowski.Net.Lidgren;
    using Lebowski.TextModel;
    using Lebowski.Synchronization.DifferentialSynchronization;
    using TwinEditor.UI;
    internal sealed class Program
    {
        /// <summary>
        /// Program entry point.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            /*// Debug
            if (!Directory.Exists("Debug"))
            {
                Directory.CreateDirectory("Debug");
            }

            DiffMatchPatch.diff_match_patch DiffMatchPatch = new DiffMatchPatch.diff_match_patch();
            string text1, shadow1, shadow2;
            text1 = "foo\r\nfoo";
            shadow1 = "foo\r\nfoo\r\n";
            shadow2 = "";
            var diff = DiffMatchPatch.diff_main(text1, shadow1);
            var delta_ = DiffMatchPatch.diff_toDelta(diff);

            var patch1 = DiffMatchPatch.patch_make(shadow1, diff);
            shadow1 = (string)DiffMatchPatch.patch_apply(patch1, shadow1)[0];

            shadow2 = (string)DiffMatchPatch.patch_apply(patch1, shadow2)[0];


            var di = new DirectoryInfo(".");
            foreach (FileInfo fi in di.GetFiles())
            {
                if (!fi.Name.EndsWith(".dat"))
                    continue;
                Stream stream = File.OpenRead(fi.Name);
                BinaryFormatter bf = new BinaryFormatter();
                SynchronizationStep step = (SynchronizationStep)bf.Deserialize(stream);
                stream.Close();

                if (step.Sent)
                    Console.ForegroundColor = ConsoleColor.Green;
                else
                    Console.ForegroundColor = ConsoleColor.Yellow;

                Console.WriteLine("-- Site {0}: {1} -- ", step.Site, fi.Name);
                Console.WriteLine(step.StackTrace.Split('\n')[4]);

                string shadow = step.Shadow;
                string context = step.Data;
                string delta = step.Diff;

                Console.WriteLine("Data [{1}] = {0}", context, context.Length);
                Console.WriteLine("Shadow [{1}] = {0}", shadow, shadow.Length);
                Console.WriteLine("Delta = {0}", delta);

                // Try to perform step
                // Apply diff to shadow
                var diffs = DiffMatchPatch.diff_fromDelta(shadow, delta);
                var shadowPatch = DiffMatchPatch.patch_make(shadow, diffs);
                shadow = (string)DiffMatchPatch.patch_apply(shadowPatch, shadow)[0];

                // For the real context, we have to perform each operation by hand,
                //so the context can do things like moving around the selection area
                var textPatch = DiffMatchPatch.patch_make(context, diffs);
                foreach (var patch in textPatch)
                {
                    System.Console.WriteLine(patch);
                }

                if (step.Sent == false)
                {
                    context = (string)DiffMatchPatch.patch_apply(textPatch, context)[0];
                }
                Console.WriteLine("Data' = {0}", context);
                Console.WriteLine("Shadow' = {0}", shadow);
            }*/

            #if (!DEBUG)
            try
            #endif
            {
                log4net.Config.BasicConfigurator.Configure();
                Program prog = new Program();
                prog.Run();
            }
            #if (!DEBUG)
            /* In the release build, we hope that no unhandled exceptions occur, but if they do,
            we would like the user the option to report a bug */
            catch(Exception e)
            {
                UI.ErrorMessage errorMessage = new UI.ErrorMessage("Application failure", "We're so sorry, but it appears that an unrecoverable error occured :(", e);
                errorMessage.ShowReportButton = true;
                errorMessage.Report += delegate
                {
                    // TODO: send per e-mail or post on getsatisfaction
                };
                errorMessage.ShowDialog();
            }
            #endif
        }

        private void SetupConfiguration()
        {
            var appSettings = Configuration.ApplicationSettings.Default;

            // Retrieve username from system if none has been set explicitly
            string userName = appSettings.UserName;
            if (userName == null || userName == "")
            {
                userName = Environment.UserName;
            }

            appSettings.UserName = userName;
            appSettings.Save();
            System.Console.WriteLine("Welcome, {0}", userName);
        }

        private void Run()
        {
            // initialize application utilities (e.g. language resources)
            ApplicationUtil.Initialize();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            SetupConfiguration();

            // Display main form
            IApplicationView applicationView = new ApplicationViewForm();
            ApplicationContext presenter = new ApplicationContext(applicationView);
            applicationView.ApplicationContext = presenter;

            // Restore recent files if any
            var appSettings = Configuration.ApplicationSettings.Default;
            List<string> recentFiles = appSettings.RecentFileList;
            if (recentFiles != null) {
            	((ApplicationViewForm) applicationView).UpdateRecentFiles(recentFiles);
            }
            applicationView.Show();

            Application.Run();
        }
    }
}