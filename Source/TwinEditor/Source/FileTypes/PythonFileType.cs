namespace TwinEditor.FileTypes
{
    using System;
    using System.IO;
    using System.Drawing;
    using System.Threading;
    using TwinEditor.Execution;
    
    public class PythonFileType : IFileType
    {
        private PythonInterpreter interpreter;
        
        public PythonFileType()
        {
            interpreter = new PythonInterpreter();
        }
        
        public string Name
        {
            get { return "Python"; }
        }
        
        public string FileNamePattern
        {
            get { return "*.py"; }
        }
        
        public string FileExtension
        {
            get { return ".py"; }
        }
    
        public bool FileNameMatches(string fileName)
        {
            return fileName.EndsWith(FileExtension);
        }
        
        public Image Icon
        {
            get
            {
                var rm = new System.Resources.ResourceManager("TwinEditor.Resources", System.Reflection.Assembly.GetExecutingAssembly());
                return (System.Drawing.Image)rm.GetObject("PythonImage");
            }
        }
        
        public bool CanCompile
        {
            get { return false; }
        }
        
        public void Compile(string content, TextWriter stdout)
        {
            
        }
        
        public bool CanExecute
        {
            get { return true; }
        }        
        
        public void Execute(string content, ExecutionResult result)
        {
            ThreadStart ts = new ThreadStart((Action) delegate { DoExecute(content, result); });
            Thread t = new Thread(ts);
            t.Name = "Python execution thread";            
            t.Start();
        }        
        
        private void DoExecute(string content, ExecutionResult result)
        {
            var writer = new PythonStringWriter();
            string standardOut = "";
            writer.Write += delegate(object sender, WriteEventArgs e)
            {
                standardOut += e.Text;
                result.OnExecutionChanged(new ExecutionChangedEventArgs(e.Text.Replace("\n", Environment.NewLine)));
            };                        
            interpreter.ExecuteCode(content, writer);            
            result.OnFinishedExecution(new FinishedExecutionEventArgs(0, standardOut));
        }
    }
}
