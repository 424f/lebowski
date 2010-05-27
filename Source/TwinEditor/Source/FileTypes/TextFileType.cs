namespace TwinEditor.FileTypes
{
    using System;
    using System.IO;
    using System.Drawing;
    using TwinEditor.Execution;

    /// <summary>
    /// Provides support for a plain text file.
    /// </summary>
    public class TextFileType : IFileType
    {
        /// <inheritdoc/>
        public string Name
        {
            get { return "Text"; }
        }

        /// <inheritdoc/>
        public string FileNamePattern
        {
            get { return "*.txt"; }
        }

        /// <inheritdoc/>
        public string FileExtension
        {
            get { return ".txt"; }
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
                return (System.Drawing.Image)rm.GetObject("TextImage");
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
            get { return false; }
        }

        /// <inheritdoc/>
        public void Execute(string content, ExecutionResult stdout)
        {

        }
    }
}