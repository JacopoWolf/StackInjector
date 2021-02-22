﻿using System.Linq;
using NuGet.Frameworks;
using NUnit.Framework;
using StackInjector.Attributes;
using StackInjector.Exceptions;
using StackInjector.Settings;
using StackInjector.Wrappers;

namespace StackInjector.TEST.BlackBox.Features
{
#pragma warning disable CS0649

	internal class InstantiationDiff : CommonTestingMethods
	{
		[Service]
		private class WrapperBase
		{
			[Served]
			public readonly ServiceA serviceA;

			public void Work () => this.serviceA.sharedCondition = true;

		}

		[Service]
		private class ServiceA
		{
			public bool sharedCondition;
		}


		[Test]
		public void SimpleClone ()
		{
			var settings = StackWrapperSettings.Default;
			settings.InjectionOptions
							.TrackInstantiationDiff();

			IStackWrapper<WrapperBase> wrapperA, wrapperB;

			wrapperA = Injector.From<WrapperBase>(settings);
			wrapperB = wrapperA.CloneCore().ToWrapper<WrapperBase>();


			Assert.Multiple(() =>
			{
				Assert.DoesNotThrow(() => wrapperB.Entry.Work());
				Assert.AreSame(
					wrapperA.GetServices<ServiceA>().First(),
					wrapperB.GetServices<ServiceA>().First()
				);
				Assert.AreSame(
					wrapperA.Settings,
					wrapperB.Settings
					);
			});
		}

		[Test]
		public void SimpleCloneWithDispose ()
		{
			var settings = StackWrapperSettings.Default;
			settings.InjectionOptions
							.TrackInstantiationDiff();

			IStackWrapper<WrapperBase> wrapperB;

			using( var wrapperA = Injector.From<WrapperBase>(settings) )
			{
				wrapperA.Start(e => e.Work());
				wrapperB = wrapperA.CloneCore().ToWrapper<WrapperBase>();
			}

			// after a deep clone there are no instances in wrapperB
			Assert.Throws<InvalidEntryTypeException>(() => wrapperB.Entry.Work());

		}


		[Test]
		public void DeepClone ()
		{
			var settings = StackWrapperSettings.Default;
			settings.InjectionOptions
							.TrackInstantiationDiff();

			IStackWrapper<WrapperBase> wrapperB;

			var wrapperA = Injector.From<WrapperBase>(settings);

			wrapperA.Start(e => e.Work());
			wrapperB = wrapperA.DeepCloneCore().ToWrapper<WrapperBase>();

			Assert.Multiple(() =>
			{
				Assert.DoesNotThrow(() => wrapperB.Entry.Work());
				Assert.AreNotSame(
					wrapperA.GetServices<ServiceA>().First(),
					wrapperB.GetServices<ServiceA>().First()
				);
				Assert.AreNotSame(
					wrapperA.Settings,
					wrapperB.Settings
					);
			});
		}

		[Test]
		public void DeepCloneWithDispose ()
		{
			var settings = StackWrapperSettings.Default;
			settings.InjectionOptions
							.TrackInstantiationDiff();

			IStackWrapper<WrapperBase> wrapperB;

			using( var wrapperA = Injector.From<WrapperBase>(settings) )
			{
				wrapperA.Start(e => e.Work());
				wrapperB = wrapperA.DeepCloneCore().ToWrapper<WrapperBase>();
			}

			// wrapper B is a deep clone. Being different objects, disposing one won't interact with the other
			Assert.DoesNotThrow(() => wrapperB.Entry.Work());

		}
	}
}