using System;
using System.Linq;
using NUnit.Framework;
using StackInjector.Attributes;
using StackInjector.Core;
using StackInjector.Exceptions;
using StackInjector.Settings;

#pragma warning disable CS0649
namespace StackInjector.TEST.BlackBox.UseCases
{
	internal class Sync : CommonTestingMethods
	{

		[Test]
		public void FromClass ()
		{
			var baseClass = Injector.From<Base>().Entry;

			// asserts the whole structure works and references are correct.
			Assert.AreEqual(42, baseClass.Logic());
		}


		[Test]
		public void FromInterface ()
		{
			var baseClass = Injector.From<IBase>().Entry;

			// asserts the whole structure works and references are correct.
			Assert.AreEqual(42, baseClass.Logic());
		}


		[Service] private class ReferenceLoopA {[Served] public ReferenceLoopB loopB; }

		[Service] private class ReferenceLoopB {[Served] public ReferenceLoopA loopA; }

		[Test]
		public void CircularReference () => Assert.That(() => Injector.From<ReferenceLoopA>(), Throws.Nothing);


		[Test]
		public void ServeStrict ()
		{
			var settings = StackWrapperSettings.Default;
		settings.Injection
				.ServingMethod( StackWrapperSettings.ServeAllStrict, true );

			var entry = Injector.From<ForgotTheServedAttributeBase>(settings).Entry;

			Assert.That(new object[] { entry.level1A, entry.Level1B }, Is.All.Null);
		}


		[Test]//[Ignore("feature has to be reviewd")]
		public void RemoveUnusedTypes ()
		{
			var settings = StackWrapperSettings.Default;
			settings.Injection
				.RemoveUnusedTypesAfterInjection();

			Assert.Multiple(() =>
			{
				var wrap1 = Injector.From<Base>( );
				Assert.AreEqual(4, wrap1.CountServices());

				// base is removed after injecting from a class that doesn't need it
				var clone1 = wrap1.DeepCloneCore( settings ).ToWrapper<Level1_2>( );
				Assert.AreEqual(2, clone1.CountServices());
			});

		}

		[Test]
		public void AccessWrapper ()
		{
			var wrapper = Injector.From<AccessWrapperBase>();

			Assert.AreSame(wrapper, wrapper.Entry.wrapper);
		}


		[Test]
		public void ServeEnum ()
		{
			var injected = Injector.From<AllLevel1Base>().Entry.level1s;

			CollectionAssert.AreEquivalent
				(
					new Type[] { typeof(Level1A), typeof(Level1B), typeof(Level1LatestVersion), typeof(Level1_2) },
					injected.Select(i => i.GetType())
				);
		}

		// -----------

		[Service]
		private class AlwaysCreateBase
		{
			[Served] public AlwaysCreateInstance instance1;
			[Served] public AlwaysCreateInstance instance2;
		}

		[Service(Pattern = InstantiationPattern.AlwaysCreate)]
		private class AlwaysCreateInstance { }

		[Test]
		public void AlwaysCreate ()
		{
			var wrapper = Injector.From<AlwaysCreateBase>();

			Assert.Multiple(() =>
			{
				Assert.AreEqual(2, wrapper.GetServices<AlwaysCreateInstance>().Count());
				CollectionAssert.AllItemsAreUnique(wrapper.GetServices<AlwaysCreateInstance>());
			});

		}


		internal class Cloning
		{
			[Test]
			public void ExternalAfterCloning ()
			{
				Assert.DoesNotThrow(() =>
				{
					Injector
						.From<Level1_2>()		// service with no dependency
						.CloneCore()
						.ToWrapper<Level1A>();
				});
			}

			[Test]
			public void CloneSame ()
			{
				var wrapper = Injector.From<AccessWrapperBase>();

				var cloneWrapper = wrapper.Entry.Clone();

				CollectionAssert.AreEquivalent(wrapper.GetServices<object>(), cloneWrapper.GetServices<object>());
			}

			[Test]
			public void CloneNoRepetitionsSingleton ()
			{
				var wrapper = Injector.From<IBase>();

				var clone = wrapper.CloneCore().ToWrapper<IBase>();

				Assert.AreSame(clone, clone.GetServices<IStackWrapperCore>().Single());

			}


		}


	}
}