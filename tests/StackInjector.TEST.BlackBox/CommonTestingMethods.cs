using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace StackInjector.TEST.BlackBox
{
	/// <summary>
	/// common methods for every test class to inherith from.
	/// </summary>
	class CommonTestingMethods
	{
		// this is done because apaprently [Timeout] and [Retry] together just don't work
		protected static void ExecuteTest ( int timeout, Action test )
		{
			try
			{
				var task = Task.Run(test);
				if ( !task.Wait(timeout))
					throw new TimeoutException("Timed out");
			}
			catch ( Exception ex )
			{
				//If the caught exception is not an assert exception but an unhandled exception.
				if ( !(ex is AssertionException) )
					Assert.Fail(ex.Message);
			}
		}
	}
}
