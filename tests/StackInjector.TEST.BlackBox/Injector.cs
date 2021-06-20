using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using StackInjector.Attributes;
using StackInjector.Core;
using StackInjector.Settings;
using StackInjector.TEST.Structures.Simple;

namespace StackInjector.TEST.BlackBox
{
	/*
	 * all of those tests use the default class configuration
	 */

	[TestFixture]
	public class Injection
	{

#pragma warning disable 0649

		[Test]
		public void From_Class ()
		{
			var wrapper = Injector.From<Base>();
			var entry = wrapper.Entry;

			Assert.Multiple(() => {

				Assert.That(entry, Is.InstanceOf<Base>());
				Assert.That(entry.level1A, Is.Not.Null.And.InstanceOf<Level1_11>());
				Assert.That(entry.level1B, Is.Not.Null.And.InstanceOf<Level1_12>());
				Assert.AreSame(entry.level1A.level2, entry.level1B.level2);
				Assert.AreEqual(5, wrapper.CountServices()); // 4 classes + 1 wrapper

			});
		}

		[Test]
		public void From_Interface ()
		{
			var wrapper = Injector.From<IBase>();
			var entry = wrapper.Entry;

			Assert.Multiple(() => {

				Assert.That(entry, Is.InstanceOf<Base>());
				var _entry = (Base)entry;
				Assert.That(_entry.level1A, Is.Not.Null.And.InstanceOf<Level1_11>());
				Assert.That(_entry.level1B, Is.Not.Null.And.InstanceOf<Level1_12>());
				Assert.AreSame(_entry.level1A.level2, _entry.level1B.level2);
				Assert.AreEqual(5, wrapper.CountServices()); // 4 classes + 1 wrapper

			});
		}



		[Service] private class _AccessWrapper {[Served] public IStackWrapperCore wrapper; }

		[Test]
		public void IsInjected_Wrapper ()
		{
			var wrapper = Injector.From<_AccessWrapper>();

			Assert.AreSame(wrapper, wrapper.Entry.wrapper);
		}



		[Service] private class _AllSimpleLevel1s {[Served] public IEnumerable<ILevel1> level1s; }

		[Test(Description = "the ienumerable should be a list of every class implementing ILevel1")]
		public void ServeEnum_Interface ()
		{
			var wrapper = Injector.From<_AllSimpleLevel1s>();

			var level1s = wrapper.Entry.level1s;

			CollectionAssert.AreEquivalent(
				expected: new[] { typeof(Level1_11), typeof(Level1_12), typeof(Level1_50), typeof(Level1b_20) },
				actual:   level1s.Select(i => i.GetType())
			);
		}


		[Service] private class _AllSimpleLevel1s_class {[Served] public IEnumerable<Level1_11> level1s; }

		[Test(Description = "the ienumerable should be a list of every class inherithing from Level1_12")]
		public void ServeEnum_Class ()
		{
			var wrapper = Injector.From<_AllSimpleLevel1s_class>();

			var level1s = wrapper.Entry.level1s;

			CollectionAssert.AreEquivalent(
				expected: new Type[] { typeof(Level1_11), typeof(Level1b_20) },
				actual:   level1s.Select(i => i.GetType())
			);
		}


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


		[Service]
		private class _ExternalAssemblyReference_Class {[Served] public readonly ExternalAssembly.Externalclass externalclass; }

		[Test]
		public void ExternalAssembly_FromClass ()
		{
			Assert.DoesNotThrow(() => Injector.From<_ExternalAssemblyReference_Class>());
		}


		[Service]
		internal class _ExternalAssemblyReference_Interface {[Served] public readonly ExternalAssembly.IExternalClass externalclass; }

		[Test]
		public void ExternalAssembly_FromInterface ()
		{
			Assert.DoesNotThrow(() => Injector.From<_ExternalAssemblyReference_Interface>());
		}

	}
}