using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using StackInjector.Attributes;
using StackInjector.Exceptions;
using StackInjector.Settings;

namespace StackInjector.TEST.BlackBox
{
	[TestFixture]
	public class Behaviour
	{
#pragma warning disable 0649

		[Service] private class _ReferenceLoopA {[Served] public _ReferenceLoopB loopB; }
		[Service] private class _ReferenceLoopB {[Served] public _ReferenceLoopA loopA; }

		[Timeout(1000)]
		[Test(Description = "verifies that A<->B does not throw an error when using Singleton references.")]
		public void Avoid_CircularReference ()
		{
			var wrapper = Injector.From<_ReferenceLoopA>();

			Assert.Multiple(() => {
				Assert.AreSame(wrapper.Entry, wrapper.Entry.loopB.loopA);
				Assert.AreEqual(2, wrapper.CountServices());
			});
		}

		// the base class is necessary because an entry point cannot be of type alwayscreate
		[Service] private class _ReferenceLoop_AC_base {[Served]public _ReferenceLoop_AC loopinit; }
		[Service(Pattern = InstantiationPattern.AlwaysCreate)] private class _ReferenceLoop_AC {[Served] public _ReferenceLoop_AC loopB; }

		[Timeout(1000)]
		[Test(Description = "verifies that A<->B throws an exception when using AlwaysCreate references.")]
		public void Limit_CircularReference ()
		{
			var settings = StackWrapperSettings.Default;
			settings.Injection.LimitInstancesCount(2);

			Assert.Throws<InstancesLimitReachedException>( ()=> Injector.From<_ReferenceLoop_AC_base>( settings ) );
			
		}


	}
}
