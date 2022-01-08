using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using StackInjector.Attributes;
using StackInjector.Exceptions;
using StackInjector.Settings;
using StackInjector.TEST.Structures.Simple;

namespace StackInjector.TEST.BlackBox
{

#pragma warning disable 0649, 0169, IDE0051

	[TestFixture]
	class Exceptions
	{

		[Service]
		private class BaseNotAServiceThrower {[Served] public int Cutelittleint { get; set; } }

		[Test]
		public void ThrowsNotAService_Property ()
		{
			Assert.Throws<NotAServiceException>(() => Injector.From<BaseNotAServiceThrower>());
		}



		private class BaseNotAService { }

		[Test]
		public void ThrowsNotAService_Base ()
		{
			Assert.Throws<NotAServiceException>(() => Injector.From<BaseNotAService>());
		}



		[Service]
		private abstract class AbstractThrower { }

		[Test]
		public void ThrowsOnAbstractClass ()
		{
			Assert.Throws<InvalidConstructorException>(() => Injector.From<AbstractThrower>());
		}



		private interface INoImplementationThrower { void SomeMethod (); }
		[Service] private class BaseNoImplementationThrower {[Served] private readonly INoImplementationThrower no; }

		[Test]
		public void ThrowsImplementationNotFound ()
			=> Assert.Throws<ImplementationNotFoundException>(() => Injector.From<BaseNoImplementationThrower>());

		//  ----------
		[Service(Pattern = InstantiationPattern.AlwaysCreate)] private class InvalidEntryTypeThrower { }

		[Test]
		public void ThrowsInvalidEntryType ()
			=> Assert.Throws<InvalidEntryTypeException>(() => Injector.From<InvalidEntryTypeThrower>());

		//  ----------

		[Service]
		private class BaseNoParameterlessConstructorThrower
		{ public int test; public BaseNoParameterlessConstructorThrower ( int test ) => this.test = test; }

		[Test]
		public void ThrowsMissingParameterlessConstructor ()
			=> Assert.Throws<InvalidConstructorException>(() => Injector.From<BaseNoParameterlessConstructorThrower>());

		//  ----------

		[Service]
		private class BaseNoSetterThrower {[Served] public Base Base { get; } }

		[Test]
		public void ThrowsNoSetter ()
			=> Assert.Throws<NoSetterException>(() => Injector.From<BaseNoSetterThrower>());


	}
}
