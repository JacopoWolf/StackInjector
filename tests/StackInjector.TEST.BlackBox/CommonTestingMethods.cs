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
#if RELEASE
		[TearDown]
		public void ConsoleLogOnError ()
		{
			var result = TestContext.CurrentContext.Result.Outcome.Status;
			if ( result == TestStatus.Failed || result == TestStatus.Inconclusive )
			{
				Console.WriteLine(
					$"\t\tFailed:{TestContext.CurrentContext.Test.FullName}\n\t\tMessage:{TestContext.CurrentContext.Result.Message}"
					);
			}
		}
#endif
	}
}
