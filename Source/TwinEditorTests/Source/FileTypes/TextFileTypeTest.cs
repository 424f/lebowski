namespace TwinEditorTests.FileTypes
{
    using NUnit.Framework;
    using TwinEditor.FileTypes;
    
    [TestFixture]
    public class TextFileTypeTest
    {
        IFileType fileType = new TextFileType();
        
        [Test]
        public void CannotExecute()
        {
            Assert.AreEqual(false, fileType.CanExecute);
        }

        [Test]
        public void CannotCompile()
        {
            Assert.AreEqual(false, fileType.CanCompile);
        }                
    }
}
