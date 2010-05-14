﻿using System;
using System.Threading;

namespace LebowskiTests
{
	public class AreEqualTimeoutException : Exception
	{
		public AreEqualTimeoutException(string message) : base(message)
		{
			
		}
	}
	
	public static class TestUtil
	{

		/// <summary>
		/// As in the networking code, we have a lot of asynchronous code, we sometimes have to wait
		/// for a few milliseconds until cause yields an effect. WaitAreEqual waits for at most
		/// maxMilliseconds ms for expected to became equal to actual and otherwise throws a
		/// AreEqualTimeoutException.
		/// </summary>					
		public static void WaitAreEqual<T>(T expected, ref T actual, int maxMilliseconds)
		{
			int waited = 0;
			const int waitInterval = 10;
			while(waited <= maxMilliseconds)
			{
				if(expected.Equals(actual))
				{
					return;
				}
				Thread.Sleep(waitInterval);
				waited += waitInterval;
			}
			throw new AreEqualTimeoutException("Should have been '" + expected.ToString() + "' but was '" + actual.ToString() + "', even after " + maxMilliseconds + "ms.");
		}
		
	}
}