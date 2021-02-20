using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

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

////#if RELEASE
////		[TearDown]
////		public void ConsoleLogOnError ()
////		{
////			var result = TestContext.CurrentContext.Result.Outcome.Status;
////			if ( result == TestStatus.Failed || result == TestStatus.Inconclusive )
////			{
////				Console.WriteLine(
////					$"\t\tFailed:{TestContext.CurrentContext.Test.FullName}\n\t\tMessage:{TestContext.CurrentContext.Result.Message}"
////					);
////			}
////		}
////#endif
	}
}
