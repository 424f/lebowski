namespace TwinEditor.FileTypes
{
    using System;
    using System.IO;
    using System.Drawing;
    using TwinEditor.Execution;

    /// <summary>
    /// Provides the behavior and properties of a file type supported
    /// by the editor.
    /// </summary>
    public interface IFileType
    {
        /// <summary>
        /// A descriptive name that will be used to display this file type
        /// to the user in the user interface.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The pattern for files that are of this type.
        /// See http://msdn.microsoft.com/en-us/library/system.windows.controls.openfiledialog.filter(VS.95).aspx
        /// for the needed form.
        /// </summary>
        string FileNamePattern { get; }

        /// <summary>
        /// The extenstion of the file type.
        /// This is basically the same as the FileNamePattern but without the asterisk.
        /// </summary>
        string FileExtension { get; }

        /// <summary>
        /// Checks whether `fileName` appears to be the name of a file
        /// of this type.
        /// </summary>
        bool FileNameMatches(string fileName);

        /// <summary>
        /// The icon that is associated with this type and is displayed
        /// in the GUI (e.g. create file dialog, tab page, ...).
        /// </summary>
        Image Icon { get; }

        /// <summary>
        /// Indicates whether the file can be compiled.
        /// </summary>
        bool CanCompile { get; }
        
        /// <summary>
        /// Compiles the file.
        /// </summary>
        /// <param name="source">Source code to compile.</param>
        /// <param name="stdout">TextWriter where the results should be written to.</param>
        void Compile(string source, TextWriter stdout);

        /// <summary>
        /// Indicates whether the file can be executed.
        /// </summary>
        bool CanExecute { get; }
        
        /// <summary>
        /// Execute the source of the file.
        /// </summary>
        /// <param name="source">Source code to execute.</param>
        /// <param name="result">ExecutionResult instance that fires events according to the execution state.</param>
        void Execute(string source, ExecutionResult result);
    }
}