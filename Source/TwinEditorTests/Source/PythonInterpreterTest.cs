namespace TwinEditor.Test
{
    using System;
    using System.IO;
    using TwinEditor.Execution;
    using NUnit.Framework;
    [TestFixture]
    public class PythonInterpreterTest
    {
        [Test]
        public void testExecution()
        {
            var interpreter = new PythonInterpreter();
            var writer = new PythonStringWriter();
            object val = interpreter.ExecuteCode("print 'test'", writer);
            Assert.AreEqual("test\n", writer.GetContent());
        }

        [Test]
        public void testExecution2()
        {
            var interpreter = new PythonInterpreter();
            var writer = new PythonStringWriter();
            string code = @"
def fact(n):
  return (1 if n <= 1 else n*fact(n-1))
for i in xrange(5):
  print fact(i)";
            object val = interpreter.ExecuteCode(code, writer);
            Assert.AreEqual("1\n1\n2\n6\n24\n", writer.GetContent());
        }

        [Test]
        public void testErrorHandling1()
        {
            var interpreter = new PythonInterpreter();
            var writer = new PythonStringWriter();
            Assert.DoesNotThrow(delegate { interpreter.ExecuteCode("x", writer); });
        }
    }
}