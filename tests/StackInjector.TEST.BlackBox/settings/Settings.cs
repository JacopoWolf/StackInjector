using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using StackInjector.Attributes;
using StackInjector.Settings;
using StackInjector.TEST.Structures.Simple;

namespace StackInjector.TEST.BlackBox
{
	[TestFixture]
	public class Settings
	{

		[Test]
		public void ThrowsOnMaskDisabled ()
			=> Assert.Throws<InvalidOperationException>(() => MaskOptions.Disabled.Register());


	}
}
