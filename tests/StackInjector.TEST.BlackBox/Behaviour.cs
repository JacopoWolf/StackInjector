using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using StackInjector.Attributes;
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


		//? should this maybe go to Inejctor
		[Service]
		private class _AlwaysCreateBase
		{
			[Served] public _AlwaysCreateInstance instance1;
			[Served] public _AlwaysCreateInstance instance2;
		}

		[Service(Pattern = InstantiationPattern.AlwaysCreate)]
		private class _AlwaysCreateInstance { }

		[Test]
		public void AlwaysCreate ()
		{
			var wrapper = Injector.From<_AlwaysCreateBase>();

			Assert.Multiple(() =>
			{
				Assert.AreEqual(2, wrapper.GetServices<_AlwaysCreateInstance>().Count());
				CollectionAssert.AllItemsAreUnique(wrapper.GetServices<_AlwaysCreateInstance>());
			});

		}

	}
}
