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

            applicationView.Show();

            Application.Run();
        }
    }
}