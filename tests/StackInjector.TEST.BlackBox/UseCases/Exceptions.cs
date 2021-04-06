using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Internal;
using StackInjector.Attributes;
using StackInjector.Exceptions;
using StackInjector.Settings;
using StackInjector.TEST.ExternalAssembly;

using SWS = StackInjector.Settings.StackWrapperSettings;

namespace StackInjector.TEST.BlackBox.UseCases
{

#pragma warning disable IDE0051, IDE0044, CS0169, CS0649

	internal class Exceptions : CommonTestingMethods
	{
		//  ----------

		[Service]
		private class BaseNotAServiceThrower {[Served] private List<int> integers; }

		[Test]
		public void ThrowsNotAService ()
			=> Assert.Throws<NotAServiceException>(() => Injector.From<BaseNotAServiceThrower>());

		//  ----------

		private class BaseNotAService { }

		[Test]
		public void ThrowsBaseNotAService ()
			=> Assert.Throws<NotAServiceException>(() => Injector.From<BaseNotAService>());

		//  ----------

		[Service]
		private abstract class AbstractThrower { }

		[Test]
		public void ThrowsOnAbstractClass ()
			=> Assert.Throws<MissingParameterlessConstructorException>( () => Injector.From<AbstractThrower>() );

		// ----------

		// references class in unregistred external assembly
		[Service]
		internal class ClassInExternalAssemblyBase {[Served] public Externalclass externalClass; }

		[Test]
		public void ThrowsServiceNotFound ()
		{
			var settings = SWS.With(
				mask: MaskOptions.BlackList
					.Register(typeof(Externalclass))
				);

			//todo change to specific mask exception
			Assert.Throws<InvalidOperationException>(() => Injector.From<ClassInExternalAssemblyBase>(settings));
		}


		[Test]
		public void ExternalAssemblyReference ()
		{
			var externalClass = Injector.From<ClassInExternalAssemblyBase>().Entry.externalClass;

			Assert.That(externalClass, Is.TypeOf<Externalclass>());
		}


		[Test]
		[Ignore("feature disabled.")]
		public void ExternalAllAssemblyReference ()
		{
			//var settings = SWS.With();
			//settings.MaskOptions.RegisterDomain();


			//var externalClass = Injector.From<BaseServiceNotFoundThrower>(settings).Entry.externalClass;

			//Assert.That(externalClass, Is.TypeOf<Externalclass>());
		}


		//  ----------

		private interface INoImplementationThrower { void SomeMethod (); }
		[Service] private class BaseNoImplementationThrower {[Served] private INoImplementationThrower no; }

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
			=> Assert.Throws<MissingParameterlessConstructorException>(() => Injector.From<BaseNoParameterlessConstructorThrower>());

		//  ----------

		[Service]
		private class BaseNoSetterThrower {[Served] public Base Base { get; } }

		[Test]
		public void ThrowsNoSetter ()
			=> Assert.Throws<NoSetterException>(() => Injector.From<BaseNoSetterThrower>());

		//  ----------

		[Service(Pattern = InstantiationPattern.AlwaysCreate)]
		private class AlwaysCreateLoopInjectionThrower {[Served] private readonly AlwaysCreateLoopInjectionThrower loop; }

		[Service]
		private class InstantiationPatternBase {[Served] private readonly AlwaysCreateLoopInjectionThrower loop; }

		[Test]
		[Timeout(200)]
		public void ThrowsInstLimitReach_AlwaysCreate ()
		{
			Assert.Throws<InstancesLimitReachedException>(() => Injector.From<InstantiationPatternBase>());
		}


		// ----------


		[Service]
		private class SingletonLoopInjectionNotThrowerBase { [Served] readonly SingletonLoopInjectionNotThrower pippo; }

		[Service(Pattern = InstantiationPattern.Singleton)]
		private class SingletonLoopInjectionNotThrower { [Served] readonly SingletonLoopInjectionNotThrower self; }


		[Test][Retry(3)]
		//[Timeout(200)]
		public void NotThrowsInstLimitReach_Singleton_wBase ()
		{
			ExecuteTest(100, () =>
			{
				var settings = SWS.Default;
				settings.Injection
					.LimitInstancesCount(2);
				Assert.DoesNotThrow(() => Injector.From<SingletonLoopInjectionNotThrowerBase>(settings));
			});
		}


		[Test][Retry(3)]
		//[Timeout(200)]
		public void NotThrowsInstLimitReach_Singleton ()
		{
			ExecuteTest(100, () =>
			{
				var settings = SWS.Default;
				settings.Injection.LimitInstancesCount(2);
				Assert.DoesNotThrow(() => Injector.From<SingletonLoopInjectionNotThrower>(settings));
			});
		}

		// ----------

		[Test]
		[Timeout(500)]
		public void ThrowsInstLimitReach_Cloned ()
		{
			var settings = SWS.Default;
			settings.Injection
				.LimitInstancesCount(1);
			Assert.Throws<InstancesLimitReachedException>( () => Injector.From<IBase>().CloneCore(settings).ToWrapper<IBase>() );
		}

		// ----------


		[Test]
		public void ThrowsOnMaskDisabled () 
			=> Assert.Throws<InvalidOperationException>( () => MaskOptions.Disabled.Register() );
		

	}
}
