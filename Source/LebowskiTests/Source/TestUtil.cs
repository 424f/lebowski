
namespace LebowskiTests
{
    using System;
    using System.Threading;
    public class AreEqualTimeoutException : Exception
    {
        public AreEqualTimeoutException(string message) : base(message)
        {

        }
    }

    public static class TestUtil
    {
        private const int waitInterval = 10;
        
        /// <summary>
        /// As in the networking code we have a lot of asynchronous code, we sometimes have to wait
        /// for a few milliseconds until cause yields an effect. WaitAreEqual waits for at most
        /// maxMilliseconds ms for expected to became equal to actual and otherwise throws a
        /// AreEqualTimeoutException.
        /// </summary>
        public static void WaitAreEqual<T>(T expected, Func<T> actualCallback, int maxMilliseconds)
        {
            int waited = 0;
            while (waited <= maxMilliseconds)
            {
                if (expected.Equals(actualCallback.Invoke()))
                {
                    return;
                }
                Thread.Sleep(waitInterval);
                waited += waitInterval;
            }
            throw new AreEqualTimeoutException("Should have been '" + expected.ToString() + "' but was '" + actualCallback.Invoke().ToString() + "', even after " + maxMilliseconds + "ms.");
        }

        /// <summary>
        /// Waits at most maxMilliseconds for the assertion to pass
        /// </summary>
        /// <param name="assertCallback">The assertion expression to check-</param>
        /// <param name="maxMilliseconds">The maximum number of milliseconds to wait.</param>
        public static void WaitUntil(Action assertCallback, int maxMilliseconds)
        {
            int waited = 0;
            const int waitInterval = 10;
            Exception lastException = null;
            while (waited <= maxMilliseconds)
            {
                try
                {
                    assertCallback.Invoke();
                    return;
                }
                catch (Exception e)
                {
                    lastException = e;
                    Thread.Sleep(waitInterval);
                    waited += waitInterval;
                }
            }
            throw lastException;
        }        
    }
}