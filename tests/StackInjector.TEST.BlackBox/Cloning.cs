using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using StackInjector.Core;
using StackInjector.Settings;
using StackInjector.TEST.Structures.Simple;

namespace StackInjector.TEST.BlackBox
{
	[TestFixture]
	public class Cloning
	{

		[Test]
		public void ExternalAfterCloning ()
		{
			Assert.DoesNotThrow(() =>
			{
				Injector
					.From<Level1_11>()       // service with no dependency
					.CloneCore()
					.ToWrapper<Level1b_20>();
			});
		}

		[Test][Order(0)]
		public void CloneSame ()
		{
			var wrapper = Injector.From<IBase>();

			var cloneWrapper = wrapper.CloneCore().ToWrapper<IBase>();

			CollectionAssert.AreEquivalent(wrapper.GetServices<object>(), cloneWrapper.GetServices<object>());
		}

		[Test]
		public void CloneNoRepetitionsSingleton ()
		{
			var wrapper = Injector.From<IBase>();

			var clone = wrapper.CloneCore().ToWrapper<IBase>();

			Assert.AreSame(clone, clone.GetServices<IStackWrapperCore>().Single());

		}


		[Test]
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
				var clone1 = wrap1.DeepCloneCore( settings ).ToWrapper<ILevel2>( );
				Assert.AreEqual(2, clone1.CountServices());
			});
		}

	}
}
