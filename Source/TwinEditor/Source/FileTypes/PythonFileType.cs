namespace TwinEditor.FileTypes
{
    using System;
    using System.IO;
    using System.Drawing;
    using System.Threading;
    using TwinEditor.Execution;

    /// <summary>
    /// The IFileType implementation of a Python File
    /// </summary>
    public class PythonFileType : IFileType
    {
        /// <value>
        /// The Python interpreter instance
        /// </value>
        private PythonInterpreter interpreter;

        /// <summary>
        /// Initializes a new instance of PythonFileType
        /// </summary>
        public PythonFileType()
        {
            interpreter = new PythonInterpreter();
        }
        
        /// <inheritdoc/>
        public string Name
        {
            get { return "Python"; }
        }

        /// <inheritdoc/>
        public string FileNamePattern
        {
            get { return "*.py"; }
        }

        /// <inheritdoc/>
        public string FileExtension
        {
            get { return ".py"; }
        }

        /// <inheritdoc/>
        public bool FileNameMatches(string fileName)
        {
            return fileName.EndsWith(FileExtension);
        }

        /// <inheritdoc/>
        public Image Icon
        {
            get
            {
                var rm = new System.Resources.ResourceManager("TwinEditor.Resources", System.Reflection.Assembly.GetExecutingAssembly());
                return (System.Drawing.Image)rm.GetObject("PythonImage");
            }
        }

        /// <inheritdoc/>
        public bool CanCompile
        {
            get { return false; }
        }

        /// <inheritdoc/>
        public void Compile(string content, TextWriter stdout)
        {

        }

        /// <inheritdoc/>
        public bool CanExecute
        {
            get { return true; }
        }

        /// <summary>
        /// Spawns a new thread that performs the execution of the content.
        /// </summary>
        /// <param name="content">Source code to be executed.</param>
        /// <param name="result">ExecutionResult instance that fires events according to the execution state.</param>
        public void Execute(string content, ExecutionResult result)
        {
            ThreadStart ts = new ThreadStart((Action) delegate { DoExecute(content, result); });
            Thread t = new Thread(ts);
            t.Name = "Python execution thread";
            t.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content">Source code to be executed.</param>
        /// <param name="result">ExecutionResult instance that fires events according to the execution state.</param>
        private void DoExecute(string content, ExecutionResult result)
        {
            var writer = new PythonStringWriter();
            string standardOut = "";
            writer.Write += delegate(object sender, WriteEventArgs e)
            {
                standardOut += e.Text.Replace("\n", Environment.NewLine);
                result.OnExecutionChanged(new ExecutionChangedEventArgs(e.Text.Replace("\n", Environment.NewLine)));
            };
            interpreter.ExecuteCode(content, writer);
            result.OnFinishedExecution(new FinishedExecutionEventArgs(0, standardOut));
        }
    }
}