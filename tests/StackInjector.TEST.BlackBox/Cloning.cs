using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using StackInjector.Attributes;
using StackInjector.Core;
using StackInjector.Exceptions;
using StackInjector.Settings;
using StackInjector.TEST.Structures.Simple;
using StackInjector.Wrappers;

namespace StackInjector.TEST.BlackBox
{
	[TestFixture]
	public class Cloning
	{

		//? might be used for exception asserting
		//var iete = Assert.Throws<InvalidEntryTypeException>(() => wrapperA.Entry.Logic() );
		//Assert.IsInstanceOf<ServiceNotFoundException>(iete.InnerException);
		//StringAssert.Contains("No instance found", iete.Message);

#pragma warning disable 0649

		#region Simple cloning

		[Test]
		public void Shallow_IsSameCore ()
		{
			var wrapper = Injector.From<IBase>();

			var clone = wrapper.CloneCore().ToWrapper<IBase>();

			Assert.Multiple(() => {
				// settings is a copy
				Assert.AreSame(wrapper.Settings, clone.Settings);
				// all instances are same
				CollectionAssert.AreEquivalent( wrapper.GetServices<object>(), clone.GetServices<object>() );
			});
		}


		[Test]
		public void Shallow_DisposeSourceBoth ()
		{
			var settings = StackWrapperSettings.Default;
			settings.Injection
				.TrackInstantiationDiff();

			IStackWrapper<Base> wrapperA, wrapperB;

			using ( wrapperA = Injector.From<Base>(settings) )
			{
				wrapperB = wrapperA.CloneCore().ToWrapper<Base>();
			}

			// wrapper B is a shallow clone of A. Sharing the core, disposing of one disposes the other.
			Assert.Multiple(() => {
				CollectionAssert.IsEmpty(wrapperA.GetServices<Base>());
				CollectionAssert.IsEmpty(wrapperB.GetServices<Base>());
			});
		}

		#endregion

		#region Deep Cloning

		[Test]
		public void Deep_IsDifferentCore ()
		{
			var wrapper = Injector.From<IBase>();

			var clone = wrapper.DeepCloneCore().ToWrapper<IBase>();

			var allsrvcs = wrapper.GetServices<object>().Concat(clone.GetServices<object>());

			Assert.Multiple(() => {
				// settings is a copy
				Assert.AreNotSame(wrapper.Settings, clone.Settings);
				// no duplicate instance reference
				Assert.AreEqual(allsrvcs.Count(), allsrvcs.Distinct().Count());
				// all types are duplicated
				Assert.That(allsrvcs.GroupBy(i => i.GetType()).Select(g => g.Count()), Is.All.EqualTo(2));
			});
		}

		[Test] //? perhaps move to Settings
		public void Deep_RemoveUnused ()
		{
			var settings = StackWrapperSettings.Default;
			settings.Injection
				.RemoveUnusedTypesAfterInjection();

			Assert.Multiple(() =>
			{
				var wrap = Injector.From<Base>( );
				Assert.AreEqual(4, wrap.CountServices());

				// base is removed after injecting from a class that doesn't need it
				var clone = wrap.DeepCloneCore( settings ).ToWrapper<ILevel2>( );
				Assert.AreEqual(2, clone.CountServices());
			});
		}


		[Test]
		public void Deep_DisposeSourceNotClone ()
		{
			var settings = StackWrapperSettings.Default;
			settings.Injection
				.TrackInstantiationDiff();

			IStackWrapper<Base> wrapperA, wrapperB;

			using ( wrapperA = Injector.From<Base>(settings) )
			{
				wrapperB = wrapperA.DeepCloneCore().ToWrapper<Base>();
			}

			// wrapper B is a deep clone of A. Being different objects, disposing one won't interact with the other
			Assert.Multiple(() => {
				CollectionAssert.IsEmpty(wrapperA.GetServices<Base>());
				CollectionAssert.IsNotEmpty(wrapperB.GetServices<Base>());
			});
		}

		#endregion

	}
}
