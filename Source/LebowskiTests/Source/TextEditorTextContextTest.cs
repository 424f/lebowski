using System;
using Lebowski.TextModel;
using Lebowski.TextModel.Operations;
using ICSharpCode.TextEditor;
using System.Windows.Forms;
using NUnit.Framework;

namespace LebowskiTests
{
    
    [TestFixture]
    public class TextEditorTextContextTest
    {
        private TextEditorTextContext textEditorTextContext;
        
        public TextEditorTextContextTest()
        {
            // default constructor
        }
        
        
        [SetUp]
        [STAThread]
        public void Initialize()
        {
            ICSharpCode.TextEditor.TextEditorControl textEditorControl = new ICSharpCode.TextEditor.TextEditorControl();
 
            textEditorTextContext = new TextEditorTextContext(textEditorControl);
        }

        [Test]
        [STAThread]
        public void Test_InitEmpty()
        {
            Assert.AreEqual("", textEditorTextContext.Data, "Initial Data is not empty!");
            Assert.AreEqual(0, textEditorTextContext.SelectionStart, "Initial SelectionStart not 0!");
            Assert.AreEqual(0, textEditorTextContext.SelectionEnd, "Initial SelectionEnd not 0!");
            Assert.AreEqual("", textEditorTextContext.SelectedText, "Initial SelectedText not empty!");
        }
        
        [Test]
        [STAThread]
        public void Test_InitNotEmpty()
        {
            ICSharpCode.TextEditor.TextEditorControl textEditorControl = new ICSharpCode.TextEditor.TextEditorControl();
            textEditorControl.Text = "test content";
            textEditorTextContext = new TextEditorTextContext(textEditorControl);
            Assert.AreEqual("test content", textEditorTextContext.Data, "Initial Data is not equal to argument string!");
        }
        
        [Test]
        [STAThread]
        public void Test_InsertEmptyAt0()
        {
            InsertOperation insert = new InsertOperation("text", 0);
            textEditorTextContext.Insert(null, insert);
            Assert.AreEqual("text", textEditorTextContext.Data, "Inconsistent state after insertion!");
        }
        
        [Test]
        [STAThread]
        public void Test_InsertEmptyAt5()
        {
            InsertOperation insert = new InsertOperation("text", 5);
            
            Assert.Throws(typeof(ArgumentOutOfRangeException),
                          delegate
                          {
                              textEditorTextContext.Insert(null, insert);
                          },
                         "Insertion out of range was accepted!");
        }
        
        [Test]
        [STAThread]
        public void Test_InsertTwiceAt0()
        {
            InsertOperation insert1 = new InsertOperation("world", 0);
            InsertOperation insert2 = new InsertOperation("hello", 0);
            textEditorTextContext.Insert(null, insert1);
            Assert.AreEqual("world", textEditorTextContext.Data, "Inconsistent state after insertion!");
            textEditorTextContext.Insert(null, insert2);
            Assert.AreEqual("helloworld", textEditorTextContext.Data, "Inconsistent state after insertion!");
        }
        
        [Test]
        [STAThread]
        public void Test_InsertTwiceAt0ThenAtEnd()
        {
            InsertOperation insert1 = new InsertOperation("world", 0);
            InsertOperation insert2 = new InsertOperation("hello", 5);
            textEditorTextContext.Insert(null, insert1);
            Assert.AreEqual("world", textEditorTextContext.Data, "Inconsistent state after insertion!");
            textEditorTextContext.Insert(null, insert2);
            Assert.AreEqual("worldhello", textEditorTextContext.Data, "Inconsistent state after insertion!");
        }
        
        [Test]
        [STAThread]
        public void Test_InsertTwiceAt0ThenInMiddle()
        {
            InsertOperation insert1 = new InsertOperation("world", 0);
            InsertOperation insert2 = new InsertOperation("hello", 2);
            textEditorTextContext.Insert(null, insert1);
            Assert.AreEqual("world", textEditorTextContext.Data, "Inconsistent state after insertion!");
            textEditorTextContext.Insert(null, insert2);
            Assert.AreEqual("wohellorld", textEditorTextContext.Data, "Inconsistent state after insertion!");
        }
        
        [Test]
        [STAThread]
        public void Test_InsertAndDelete()
        {
            InsertOperation insert = new InsertOperation("world", 0);
            DeleteOperation delete = new DeleteOperation(3);
            textEditorTextContext.Insert(null, insert);
            Assert.AreEqual("world", textEditorTextContext.Data, "Inconsistent state after insertion!");
            textEditorTextContext.Delete(null, delete);
            Assert.AreEqual("word", textEditorTextContext.Data, "Inconsistent state after deletion!");
        }
        
        [Test]
        [STAThread]
        public void Test_DeleteEmptyAt0()
        {
            DeleteOperation delete = new DeleteOperation(0);
            Assert.Throws(typeof(ArgumentOutOfRangeException),
                          delegate
                          {
                              textEditorTextContext.Delete(null, delete);
                          },
                         "Deletion out of range was accepted!");
        }
        
        [Test]
        [STAThread]
        public void Test_DeleteNotEmptyAt0()
        {
            ICSharpCode.TextEditor.TextEditorControl textEditorControl = new ICSharpCode.TextEditor.TextEditorControl();
            textEditorControl.Text = "test content";
            textEditorTextContext = new TextEditorTextContext(textEditorControl);
            DeleteOperation delete = new DeleteOperation(0);
            textEditorTextContext.Delete(null, delete);
            Assert.AreEqual("est content", textEditorTextContext.Data, "Inconsistent state after deletion!");
        }
        
        [Test]
        [STAThread]
        public void Test_DeleteNotEmptyTwiceAt0()
        {
            ICSharpCode.TextEditor.TextEditorControl textEditorControl = new ICSharpCode.TextEditor.TextEditorControl();
            textEditorControl.Text = "test content";
            textEditorTextContext = new TextEditorTextContext(textEditorControl);
            DeleteOperation delete1 = new DeleteOperation(0);
            DeleteOperation delete2 = new DeleteOperation(0);
            textEditorTextContext.Delete(null, delete1);
            Assert.AreEqual("est content", textEditorTextContext.Data, "Inconsistent state after deletion!");;
            textEditorTextContext.Delete(null, delete2);
            Assert.AreEqual("st content", textEditorTextContext.Data, "Inconsistent state after deletion!");
        }
        
        [Test]
        [STAThread]
        public void Test_DeleteNotEmptyTwiceAt0ThenEnd()
        {
            ICSharpCode.TextEditor.TextEditorControl textEditorControl = new ICSharpCode.TextEditor.TextEditorControl();
            textEditorControl.Text = "test content";
            textEditorTextContext = new TextEditorTextContext(textEditorControl);
            DeleteOperation delete1 = new DeleteOperation(0);
            DeleteOperation delete2 = new DeleteOperation(10);
            textEditorTextContext.Delete(null, delete1);
            Assert.AreEqual("est content", textEditorTextContext.Data, "Inconsistent state after deletion!");;
            textEditorTextContext.Delete(null, delete2);
            Assert.AreEqual("est conten", textEditorTextContext.Data, "Inconsistent state after deletion!");
        }
        
        [Test]
        [STAThread]
        public void Test_DeleteNotEmptyTwiceAt0ThenInMiddle()
        {
            ICSharpCode.TextEditor.TextEditorControl textEditorControl = new ICSharpCode.TextEditor.TextEditorControl();
            textEditorControl.Text = "test content";
            textEditorTextContext = new TextEditorTextContext(textEditorControl);
            DeleteOperation delete1 = new DeleteOperation(0);
            DeleteOperation delete2 = new DeleteOperation(5);
            textEditorTextContext.Delete(null, delete1);
            Assert.AreEqual("est content", textEditorTextContext.Data, "Inconsistent state after deletion!");;
            textEditorTextContext.Delete(null, delete2);
            Assert.AreEqual("est cntent", textEditorTextContext.Data, "Inconsistent state after deletion!");
        }
        
        [Test]
        [STAThread]
        public void Test_SetSelectionNotEmptyAt1And5()
        {
           ICSharpCode.TextEditor.TextEditorControl textEditorControl = new ICSharpCode.TextEditor.TextEditorControl();
            textEditorControl.Text = "selection";
            textEditorTextContext = new TextEditorTextContext(textEditorControl);
            Assert.AreEqual(0, textEditorTextContext.SelectionStart, "Initial SelectionStart not 0!");
            Assert.AreEqual(0, textEditorTextContext.SelectionEnd, "Initial SelectionEnd not 0!");
            textEditorTextContext.SetSelection(1, 5);
            Assert.AreEqual(1, textEditorTextContext.SelectionStart, "SelectionStart not 1!");
            Assert.AreEqual(5, textEditorTextContext.SelectionEnd, "SelectionEnd not 5!");
            Assert.AreEqual("elec", textEditorTextContext.SelectedText, "SelectedText not elec");
        }
        
        [Test]
        [STAThread]
        public void Test_SetSelectionNotEmptyAtSame()
        {
            ICSharpCode.TextEditor.TextEditorControl textEditorControl = new ICSharpCode.TextEditor.TextEditorControl();
            textEditorControl.Text = "selection";
            textEditorTextContext = new TextEditorTextContext(textEditorControl);
            Assert.AreEqual(0, textEditorTextContext.SelectionStart, "Initial SelectionStart not 0!");
            Assert.AreEqual(0, textEditorTextContext.SelectionEnd, "Initial SelectionEnd not 0!");
            textEditorTextContext.SetSelection(2, 2);
            Assert.AreEqual(2, textEditorTextContext.SelectionStart, "SelectionStart not 2!");
            Assert.AreEqual(2, textEditorTextContext.SelectionEnd, "SelectionEnd not 2!");
            Assert.AreEqual("", textEditorTextContext.SelectedText, "SelectedText not ''");
        }
        
        [Test]
        [STAThread]
        public void Test_SetSelectionEmptyAt1And5()
        {
            Assert.AreEqual(0, textEditorTextContext.SelectionStart, "Initial SelectionStart not 0!");
            Assert.AreEqual(0, textEditorTextContext.SelectionEnd, "Initial SelectionEnd not 0!");
            Assert.Throws(typeof(ArgumentOutOfRangeException),
                          delegate
                          {
                              textEditorTextContext.SetSelection(1, 5);
                          },
                         "Selection out of range accepted!");
        }
        
        [Test]
        [STAThread]
        public void Test_SetSelectionInvalid()
        {
            Assert.AreEqual(0, textEditorTextContext.SelectionStart, "Initial SelectionStart not 0!");
            Assert.AreEqual(0, textEditorTextContext.SelectionEnd, "Initial SelectionEnd not 0!");
            Assert.Throws(typeof(ArgumentException),
                          delegate
                          {
                              textEditorTextContext.SetSelection(5, 1);
                          },
                         "Invalid selection accepted!");
        }
        
        [Test]
        [STAThread]
        public void Test_RefreshEmpty()
        {
            Assert.AreEqual("", textEditorTextContext.Data, "Initial Data is not empty!");
            Assert.AreEqual(0, textEditorTextContext.SelectionStart, "Initial SelectionStart not 0!");
            Assert.AreEqual(0, textEditorTextContext.SelectionEnd, "Initial SelectionEnd not 0!");
            Assert.AreEqual("", textEditorTextContext.SelectedText, "Initial SelectedText not empty!");
            textEditorTextContext.Refresh();
            Assert.AreEqual("", textEditorTextContext.Data, "Initial Data is not empty after Refresh!");
            Assert.AreEqual(0, textEditorTextContext.SelectionStart, "Initial SelectionStart not 0 after Refresh!");
            Assert.AreEqual(0, textEditorTextContext.SelectionEnd, "Initial SelectionEnd not 0 after Refresh!");
            Assert.AreEqual("", textEditorTextContext.SelectedText, "Initial SelectedText not empty after Refresh!");
        }
        
        [Test]
        [STAThread]
        public void Test_RefreshNotEmpty()
        {
            ICSharpCode.TextEditor.TextEditorControl textEditorControl = new ICSharpCode.TextEditor.TextEditorControl();
            textEditorControl.Text = "refresh";
            textEditorTextContext = new TextEditorTextContext(textEditorControl);
            Assert.AreEqual("refresh", textEditorTextContext.Data, "Initial Data is not equal to argument string!");
            Assert.AreEqual(0, textEditorTextContext.SelectionStart, "Initial SelectionStart not 0!");
            Assert.AreEqual(0, textEditorTextContext.SelectionEnd, "Initial SelectionEnd not 0!");
            Assert.AreEqual("", textEditorTextContext.SelectedText, "Initial SelectedText not empty!");
            textEditorTextContext.Refresh();
            Assert.AreEqual("refresh", textEditorTextContext.Data, "Initial Data is not equal to argumetn string after Refresh!");
            Assert.AreEqual(0, textEditorTextContext.SelectionStart, "Initial SelectionStart not 0 after Refresh!");
            Assert.AreEqual(0, textEditorTextContext.SelectionEnd, "Initial SelectionEnd not 0 after Refresh!");
            Assert.AreEqual("", textEditorTextContext.SelectedText, "Initial SelectedText not empty after Refresh!");
        }
        
        [Test]
        [STAThread]
        public void Test_RefreshNotEmptyAndSelected()
        {
            ICSharpCode.TextEditor.TextEditorControl textEditorControl = new ICSharpCode.TextEditor.TextEditorControl();
            textEditorControl.Text = "refresh";
            textEditorTextContext = new TextEditorTextContext(textEditorControl);
            textEditorTextContext.SetSelection(1,3);
            Assert.AreEqual("refresh", textEditorTextContext.Data, "Initial Data is not equal to argument string!");
            Assert.AreEqual(1, textEditorTextContext.SelectionStart, "SelectionStart not 1!");
            Assert.AreEqual(3, textEditorTextContext.SelectionEnd, "SelectionEnd not 3!");
            Assert.AreEqual("ef", textEditorTextContext.SelectedText, "Initial SelectedText not empty!");
            textEditorTextContext.Refresh();
            Assert.AreEqual("refresh", textEditorTextContext.Data, "Initial Data is not empty after Refresh!");
            Assert.AreEqual(1, textEditorTextContext.SelectionStart, "SelectionStart not 1 after Refresh!");
            Assert.AreEqual(3, textEditorTextContext.SelectionEnd, "SelectionEnd not 3 after Refresh!");
            Assert.AreEqual("ef", textEditorTextContext.SelectedText, "Initial SelectedText not empty after Refresh!");
        }
        
        [Test]
        [STAThread]
        public void Test_SetRemoreSelectionEmpty()
        {
            Assert.AreEqual("", textEditorTextContext.Data, "Initial Data is not empty!");
            Assert.AreEqual(0, textEditorTextContext.SelectionStart, "Initial SelectionStart not 0!");
            Assert.AreEqual(0, textEditorTextContext.SelectionEnd, "Initial SelectionEnd not 0!");
            Assert.AreEqual("", textEditorTextContext.SelectedText, "Initial SelectedText not empty!");
            textEditorTextContext.SetRemoteSelection(null, 3, 4);
            Assert.AreEqual("", textEditorTextContext.Data, "Initial Data is not empty after Refresh!");
            Assert.AreEqual(0, textEditorTextContext.SelectionStart, "Initial SelectionStart not 0 after Refresh!");
            Assert.AreEqual(0, textEditorTextContext.SelectionEnd, "Initial SelectionEnd not 0 after Refresh!");
            Assert.AreEqual("", textEditorTextContext.SelectedText, "Initial SelectedText not empty after Refresh!");
        }
        
        [Test]
        [STAThread]
        public void Test_SetRemoteSelectionNotEmpty()
        {
            ICSharpCode.TextEditor.TextEditorControl textEditorControl = new ICSharpCode.TextEditor.TextEditorControl();
            textEditorControl.Text = "remote";
            textEditorTextContext = new TextEditorTextContext(textEditorControl);
            Assert.AreEqual("remote", textEditorTextContext.Data, "Initial Data is not equal to argument string!");
            Assert.AreEqual(0, textEditorTextContext.SelectionStart, "Initial SelectionStart not 0!");
            Assert.AreEqual(0, textEditorTextContext.SelectionEnd, "Initial SelectionEnd not 0!");
            Assert.AreEqual("", textEditorTextContext.SelectedText, "Initial SelectedText not empty!");
            textEditorTextContext.SetRemoteSelection(null, 2, 4);
            Assert.AreEqual("remote", textEditorTextContext.Data, "Initial Data is not empty after Refresh!");
            Assert.AreEqual(0, textEditorTextContext.SelectionStart, "SelectionStart not 0 after Refresh!");
            Assert.AreEqual(0, textEditorTextContext.SelectionEnd, "SelectionEnd not 0 after Refresh!");
            Assert.AreEqual("", textEditorTextContext.SelectedText, "Initial SelectedText not empty after Refresh!");
        }
    }
}