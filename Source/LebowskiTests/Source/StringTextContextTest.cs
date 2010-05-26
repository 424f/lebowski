using System;
using Lebowski.TextModel;
using Lebowski.TextModel.Operations;
using NUnit.Framework;

namespace LebowskiTests
{
    
    [TestFixture]
    public class StringTextContextTest
    {
        private StringTextContext stringTextContext;
        
        public StringTextContextTest()
        {
            // default constructor
        }
        
        [SetUp]
        public void Initialize()
        {
            stringTextContext = new StringTextContext();
        }

        [Test]
        public void Test_InitEmpty()
        {
            Assert.AreEqual("", stringTextContext.Data, "Initial Data is not empty!");
            Assert.AreEqual(0, stringTextContext.SelectionStart, "Initial SelectionStart not 0!");
            Assert.AreEqual(0, stringTextContext.SelectionEnd, "Initial SelectionEnd not 0!");
            Assert.AreEqual("", stringTextContext.SelectedText, "Initial SelectedText not empty!");
        }
        
        [Test]
        public void Test_InitNotEmpty()
        {
            stringTextContext = new StringTextContext("test content");
            Assert.AreEqual("test content", stringTextContext.Data, "Initial Data is not equal to argument string!");
        }
        
        [Test]
        public void Test_InsertEmptyAt0()
        {
            InsertOperation insert = new InsertOperation("text", 0);
            stringTextContext.Insert(null, insert);
            Assert.AreEqual("text", stringTextContext.Data, "Inconsistent state after insertion!");
        }
        
        [Test]
        public void Test_InsertEmptyAt5()
        {
            InsertOperation insert = new InsertOperation("text", 5);
            
            Assert.Throws(typeof(ArgumentOutOfRangeException),
                          delegate
                          {
                              stringTextContext.Insert(null, insert);
                          },
                         "Insertion out of range was accepted!");
        }
        
        [Test]
        public void Test_InsertTwiceAt0()
        {
            InsertOperation insert1 = new InsertOperation("world", 0);
            InsertOperation insert2 = new InsertOperation("hello", 0);
            stringTextContext.Insert(null, insert1);
            Assert.AreEqual("world", stringTextContext.Data, "Inconsistent state after insertion!");
            stringTextContext.Insert(null, insert2);
            Assert.AreEqual("helloworld", stringTextContext.Data, "Inconsistent state after insertion!");
        }
        
        [Test]
        public void Test_InsertTwiceAt0ThenAtEnd()
        {
            InsertOperation insert1 = new InsertOperation("world", 0);
            InsertOperation insert2 = new InsertOperation("hello", 5);
            stringTextContext.Insert(null, insert1);
            Assert.AreEqual("world", stringTextContext.Data, "Inconsistent state after insertion!");
            stringTextContext.Insert(null, insert2);
            Assert.AreEqual("worldhello", stringTextContext.Data, "Inconsistent state after insertion!");
        }
        
        [Test]
        public void Test_InsertTwiceAt0ThenInMiddle()
        {
            InsertOperation insert1 = new InsertOperation("world", 0);
            InsertOperation insert2 = new InsertOperation("hello", 2);
            stringTextContext.Insert(null, insert1);
            Assert.AreEqual("world", stringTextContext.Data, "Inconsistent state after insertion!");
            stringTextContext.Insert(null, insert2);
            Assert.AreEqual("wohellorld", stringTextContext.Data, "Inconsistent state after insertion!");
        }
        
        [Test]
        public void Test_InsertAndDelete()
        {
            InsertOperation insert = new InsertOperation("world", 0);
            DeleteOperation delete = new DeleteOperation(3);
            stringTextContext.Insert(null, insert);
            Assert.AreEqual("world", stringTextContext.Data, "Inconsistent state after insertion!");
            stringTextContext.Delete(null, delete);
            Assert.AreEqual("word", stringTextContext.Data, "Inconsistent state after deletion!");
        }
        
        [Test]
        public void Test_DeleteEmptyAt0()
        {
            DeleteOperation delete = new DeleteOperation(0);
            Assert.Throws(typeof(ArgumentOutOfRangeException),
                          delegate
                          {
                              stringTextContext.Delete(null, delete);
                          },
                         "Deletion out of range was accepted!");
        }
        
        [Test]
        public void Test_DeleteNotEmptyAt0()
        {
            stringTextContext = new StringTextContext("test content");
            DeleteOperation delete = new DeleteOperation(0);
            stringTextContext.Delete(null, delete);
            Assert.AreEqual("est content", stringTextContext.Data, "Inconsistent state after deletion!");
        }
        
        [Test]
        public void Test_DeleteNotEmptyTwiceAt0()
        {
            stringTextContext = new StringTextContext("test content");
            DeleteOperation delete1 = new DeleteOperation(0);
            DeleteOperation delete2 = new DeleteOperation(0);
            stringTextContext.Delete(null, delete1);
            Assert.AreEqual("est content", stringTextContext.Data, "Inconsistent state after deletion!");;
            stringTextContext.Delete(null, delete2);
            Assert.AreEqual("st content", stringTextContext.Data, "Inconsistent state after deletion!");
        }
        
        [Test]
        public void Test_DeleteNotEmptyTwiceAt0ThenEnd()
        {
            stringTextContext = new StringTextContext("test content");
            DeleteOperation delete1 = new DeleteOperation(0);
            DeleteOperation delete2 = new DeleteOperation(10);
            stringTextContext.Delete(null, delete1);
            Assert.AreEqual("est content", stringTextContext.Data, "Inconsistent state after deletion!");;
            stringTextContext.Delete(null, delete2);
            Assert.AreEqual("est conten", stringTextContext.Data, "Inconsistent state after deletion!");
        }
        
        [Test]
        public void Test_DeleteNotEmptyTwiceAt0ThenInMiddle()
        {
            stringTextContext = new StringTextContext("test content");
            DeleteOperation delete1 = new DeleteOperation(0);
            DeleteOperation delete2 = new DeleteOperation(5);
            stringTextContext.Delete(null, delete1);
            Assert.AreEqual("est content", stringTextContext.Data, "Inconsistent state after deletion!");;
            stringTextContext.Delete(null, delete2);
            Assert.AreEqual("est cntent", stringTextContext.Data, "Inconsistent state after deletion!");
        }
        
        [Test]
        public void Test_SetSelectionNotEmptyAt1And5()
        {
            stringTextContext = new StringTextContext("selection");
            Assert.AreEqual(0, stringTextContext.SelectionStart, "Initial SelectionStart not 0!");
            Assert.AreEqual(0, stringTextContext.SelectionEnd, "Initial SelectionEnd not 0!");
            stringTextContext.SetSelection(1, 5);
            Assert.AreEqual(1, stringTextContext.SelectionStart, "SelectionStart not 1!");
            Assert.AreEqual(5, stringTextContext.SelectionEnd, "SelectionEnd not 5!");
            Assert.AreEqual("elec", stringTextContext.SelectedText, "SelectedText not elec");
        }
        
        [Test]
        public void Test_SetSelectionNotEmptyAtSame()
        {
            stringTextContext = new StringTextContext("selection");
            Assert.AreEqual(0, stringTextContext.SelectionStart, "Initial SelectionStart not 0!");
            Assert.AreEqual(0, stringTextContext.SelectionEnd, "Initial SelectionEnd not 0!");
            stringTextContext.SetSelection(2, 2);
            Assert.AreEqual(2, stringTextContext.SelectionStart, "SelectionStart not 2!");
            Assert.AreEqual(2, stringTextContext.SelectionEnd, "SelectionEnd not 2!");
            Assert.AreEqual("", stringTextContext.SelectedText, "SelectedText not ''");
        }
        
        [Test]
        public void Test_SetSelectionEmptyAt1And5()
        {
            Assert.AreEqual(0, stringTextContext.SelectionStart, "Initial SelectionStart not 0!");
            Assert.AreEqual(0, stringTextContext.SelectionEnd, "Initial SelectionEnd not 0!");
            Assert.Throws(typeof(ArgumentOutOfRangeException),
                          delegate
                          {
                              stringTextContext.SetSelection(1, 5);
                          },
                         "Selection out of range accepted!");
        }
        
        [Test]
        public void Test_SetSelectionInvalid()
        {
            Assert.AreEqual(0, stringTextContext.SelectionStart, "Initial SelectionStart not 0!");
            Assert.AreEqual(0, stringTextContext.SelectionEnd, "Initial SelectionEnd not 0!");
            Assert.Throws(typeof(ArgumentException),
                          delegate
                          {
                              stringTextContext.SetSelection(5, 1);
                          },
                         "Invalid selection accepted!");
        }
        
        [Test]
        public void Test_RefreshEmpty()
        {
            Assert.AreEqual("", stringTextContext.Data, "Initial Data is not empty!");
            Assert.AreEqual(0, stringTextContext.SelectionStart, "Initial SelectionStart not 0!");
            Assert.AreEqual(0, stringTextContext.SelectionEnd, "Initial SelectionEnd not 0!");
            Assert.AreEqual("", stringTextContext.SelectedText, "Initial SelectedText not empty!");
            stringTextContext.Refresh();
            Assert.AreEqual("", stringTextContext.Data, "Initial Data is not empty after Refresh!");
            Assert.AreEqual(0, stringTextContext.SelectionStart, "Initial SelectionStart not 0 after Refresh!");
            Assert.AreEqual(0, stringTextContext.SelectionEnd, "Initial SelectionEnd not 0 after Refresh!");
            Assert.AreEqual("", stringTextContext.SelectedText, "Initial SelectedText not empty after Refresh!");
        }
        
        [Test]
        public void Test_RefreshNotEmpty()
        {
            stringTextContext = new StringTextContext("refresh");
            Assert.AreEqual("refresh", stringTextContext.Data, "Initial Data is not equal to argument string!");
            Assert.AreEqual(0, stringTextContext.SelectionStart, "Initial SelectionStart not 0!");
            Assert.AreEqual(0, stringTextContext.SelectionEnd, "Initial SelectionEnd not 0!");
            Assert.AreEqual("", stringTextContext.SelectedText, "Initial SelectedText not empty!");
            stringTextContext.Refresh();
            Assert.AreEqual("refresh", stringTextContext.Data, "Initial Data is not equal to argumetn string after Refresh!");
            Assert.AreEqual(0, stringTextContext.SelectionStart, "Initial SelectionStart not 0 after Refresh!");
            Assert.AreEqual(0, stringTextContext.SelectionEnd, "Initial SelectionEnd not 0 after Refresh!");
            Assert.AreEqual("", stringTextContext.SelectedText, "Initial SelectedText not empty after Refresh!");
        }
        
        [Test]
        public void Test_RefreshNotEmptyAndSelected()
        {
            stringTextContext = new StringTextContext("refresh");
            stringTextContext.SetSelection(1,3);
            Assert.AreEqual("refresh", stringTextContext.Data, "Initial Data is not equal to argument string!");
            Assert.AreEqual(1, stringTextContext.SelectionStart, "SelectionStart not 1!");
            Assert.AreEqual(3, stringTextContext.SelectionEnd, "SelectionEnd not 3!");
            Assert.AreEqual("ef", stringTextContext.SelectedText, "Initial SelectedText not empty!");
            stringTextContext.Refresh();
            Assert.AreEqual("refresh", stringTextContext.Data, "Initial Data is not empty after Refresh!");
            Assert.AreEqual(1, stringTextContext.SelectionStart, "SelectionStart not 1 after Refresh!");
            Assert.AreEqual(3, stringTextContext.SelectionEnd, "SelectionEnd not 3 after Refresh!");
            Assert.AreEqual("ef", stringTextContext.SelectedText, "Initial SelectedText not empty after Refresh!");
        }
        
        [Test]
        public void Test_SetRemoreSelectionEmpty()
        {
            Assert.AreEqual("", stringTextContext.Data, "Initial Data is not empty!");
            Assert.AreEqual(0, stringTextContext.SelectionStart, "Initial SelectionStart not 0!");
            Assert.AreEqual(0, stringTextContext.SelectionEnd, "Initial SelectionEnd not 0!");
            Assert.AreEqual("", stringTextContext.SelectedText, "Initial SelectedText not empty!");
            stringTextContext.SetRemoteSelection(null, 3, 4);
            Assert.AreEqual("", stringTextContext.Data, "Initial Data is not empty after Refresh!");
            Assert.AreEqual(0, stringTextContext.SelectionStart, "Initial SelectionStart not 0 after Refresh!");
            Assert.AreEqual(0, stringTextContext.SelectionEnd, "Initial SelectionEnd not 0 after Refresh!");
            Assert.AreEqual("", stringTextContext.SelectedText, "Initial SelectedText not empty after Refresh!");
        }
        
        [Test]
        public void Test_SetRemoteSelectionNotEmpty()
        {
            stringTextContext = new StringTextContext("remote");
            Assert.AreEqual("remote", stringTextContext.Data, "Initial Data is not equal to argument string!");
            Assert.AreEqual(0, stringTextContext.SelectionStart, "Initial SelectionStart not 0!");
            Assert.AreEqual(0, stringTextContext.SelectionEnd, "Initial SelectionEnd not 0!");
            Assert.AreEqual("", stringTextContext.SelectedText, "Initial SelectedText not empty!");
            stringTextContext.SetRemoteSelection(null, 2, 4);
            Assert.AreEqual("remote", stringTextContext.Data, "Initial Data is not empty after Refresh!");
            Assert.AreEqual(0, stringTextContext.SelectionStart, "SelectionStart not 0 after Refresh!");
            Assert.AreEqual(0, stringTextContext.SelectionEnd, "SelectionEnd not 0 after Refresh!");
            Assert.AreEqual("", stringTextContext.SelectedText, "Initial SelectedText not empty after Refresh!");
        }
    }
}