namespace TwinEditorTests.FileTypes
{
    using System;
    using NUnit.Framework;
    using LebowskiTests;
    using TwinEditor.FileTypes;
    using TwinEditor.Execution;
    
    [TestFixture]
    public class PythonExecutionTest
    {
        IFileType fileType = new PythonFileType();
        ExecutionResult result;
        bool finishedExecution;
        string stdout = "";
        int maxWaitTime = 1000;
        
        [SetUp]
        public void SetUp()
        {
            result = new ExecutionResult();
            result.ExecutionChanged += ResultExecutionChanged;
            result.FinishedExecution += ResultFinishedExecution;            
            finishedExecution = false;
            stdout = "";
        }
        
        [TearDown]
        public void TearDown()
        {
            result.ExecutionChanged -= ResultExecutionChanged;
            result.FinishedExecution -= ResultFinishedExecution;
            result = null;   
        }
        
        private void ResultExecutionChanged(object sender, ExecutionChangedEventArgs e)
        {
            stdout += e.StandardOut;
        }
        
        private void ResultFinishedExecution(object sender, FinishedExecutionEventArgs e)
        {
            finishedExecution = true;
        }
        
        [Test]
        public void CanExecutePythonCode()
        {
            Assert.AreEqual(true, fileType.CanExecute);
        }

        [Test]
        public void CannotCompilePythonCode()
        {
            Assert.AreEqual(false, fileType.CanCompile);
        }        
        
        [Test]
        public void ExecuteHelloWorld()
        {
            fileType.Execute("print 'hello world'\n", result);
            TestUtil.WaitUntil(() => Assert.AreEqual(true, finishedExecution), maxWaitTime);
            Assert.AreEqual("hello world\r\n", stdout);
        }
        
        [Test]
        public void ExecuteLoop()
        {
            fileType.Execute("for i in range(10):\n  print i,\n", result);
            TestUtil.WaitUntil(() => Assert.AreEqual(true, finishedExecution), maxWaitTime);
            Assert.AreEqual("0 1 2 3 4 5 6 7 8 9", stdout);
        }
        
    }
}
