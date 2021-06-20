using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using StackInjector.Attributes;
using StackInjector.Settings;
using StackInjector.TEST.ExternalAssembly;
using StackInjector.TEST.Structures.Simple;
using static StackInjector.TEST.BlackBox.Injection;

namespace StackInjector.TEST.BlackBox
{

	#pragma warning disable 0649

	[TestFixture]
	class Versioning
	{
		#region maj min exact

		[Service] private class VersionClass {[Served] internal Level1_11 Level; }

		[Test]
		public void ServedVersioningClass ()
		{
			var settings = StackWrapperSettings.Default;
			settings.Versioning
				.VersioningMethod(ServedVersionTargetingMethod.LatestMajor, true);

			/* CLASSES ARE NOT VERSIONED, ONLY INTERFACES */
			Assert.IsInstanceOf<Level1_11>(
				Injector.From<VersionClass>(settings).Entry.Level
			);

		}

		[Service] private class VersionInterface {[Served(TargetVersion = 1.0)] public ILevel1 level1; }

		[Test]
		public void ServedVersioningInterface ()
		{
			var versionedService = Injector.From<VersionInterface>().Entry.level1;

			Assert.IsInstanceOf<Level1_11>(versionedService);
		}


		[Test]
		public void SettingVersioningLatestMaj ()
		{
			var settings = StackWrapperSettings.Default;
			settings.Versioning
				.VersioningMethod(ServedVersionTargetingMethod.LatestMajor, true);

			var versionedService = Injector.From<VersionInterface>( settings ).Entry.level1;

			Assert.IsInstanceOf<Level1_50>(versionedService);
		}


		[Test]
		public void SettingVersioningLatestMin ()
		{
			var settings = StackWrapperSettings.Default;
			settings.Versioning
				.VersioningMethod(ServedVersionTargetingMethod.LatestMinor, true);

			var versionedService = Injector.From<VersionInterface>( settings ).Entry.level1;

			Assert.IsInstanceOf<Level1_12>(versionedService);
		}

		#endregion


		[Service(Version = 2.0)]
		private class _ExternalAssemblyReference_Local : ExternalAssembly.IExternalClass
		{
			public void SomeMethod () => throw new NotImplementedException();
		}

		[Test]
		public void ExternalAssembly_LocalImplementation_Throws ()
		{
			var settings = StackWrapperSettings.With(
					mask: MaskOptions.BlackList
				);
			settings.Mask.Add(typeof(ExternalAssembly.Externalclass));
			Assert.Throws<InvalidOperationException>(() => Injector.From<_ExternalAssemblyReference_Interface>(settings));
		}

		[Test]
		public void ExternalAssembly_LocalImplementation_Vers ()
		{
			var settings = StackWrapperSettings.With(
					mask: MaskOptions.BlackList
				);
			settings.Mask.Add(typeof(Externalclass));
			settings.Versioning.AddAssembliesToLookup(Assembly.GetExecutingAssembly());

			var wrapper = Injector.From<_ExternalAssemblyReference_Interface>(settings);
			Assert.IsInstanceOf<_ExternalAssemblyReference_Local>(wrapper.Entry.externalclass);
		}

		[Test]
		public void ExternalAssembly_LocalImplementation_Bind ()
		{
			var settings = StackWrapperSettings.With(
					mask: MaskOptions.BlackList
				);
			settings.Mask
				.Add(typeof(Externalclass));
			settings.Versioning
				.AddAssembliesToLookup(typeof(IExternalClass).Assembly)
				.AddInterfaceBinding<IExternalClass,_ExternalAssemblyReference_Local>();

			var wrapper = Injector.From<_ExternalAssemblyReference_Interface>(settings);
			Assert.IsInstanceOf<_ExternalAssemblyReference_Local>(wrapper.Entry.externalclass);
		}

	}
}
