using System;
using NUnit.Framework;
using TwinEditor.UI;

namespace TwinEditor.Test
{
    /// <summary>
    /// Description of UITester.
    /// </summary>
    
    [TestFixture]
    public class UITester
    {
        
        [STAThread]
        [Test]
        public void test1()
        {
            ApplicationViewForm mainForm = new ApplicationViewForm();
            mainForm.Show();
            Assert.IsNotNull(mainForm);
        }

    }
}
