namespace TwinEditor.FileTypes
{
    using System;
    using System.IO;
    using System.Drawing;
    using TwinEditor.Execution;

    public class TextFileType : IFileType
    {
        public string Name
        {
            get { return "Text"; }
        }

        public string FileNamePattern
        {
            get { return "*.txt"; }
        }

        public string FileExtension
        {
            get { return ".txt"; }
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
                return (System.Drawing.Image)rm.GetObject("TextImage");
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
            get { return false; }
        }

        public void Execute(string content, ExecutionResult stdout)
        {

        }
    }
}