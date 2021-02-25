using NUnit.Framework;
using StackInjector.Attributes;
using StackInjector.Settings;

namespace StackInjector.TEST.BlackBox.Features
{
	internal class Versioning : CommonTestingMethods
	{
#pragma warning disable CS0649

		[Service] private class VersionClass {[Served] internal Level1A Level1_2; }

		[Test]
		public void ServedVersioningClass ()
		{
			var settings = StackWrapperSettings.Default;
			settings.InjectionOptions
				.VersioningMethod(ServedVersionTargetingMethod.LatestMajor,true);

			var versionedService = Injector.From<VersionClass>(settings).Entry.Level1_2;

			/* CLASSES ARE NOT VERSIONED, ONLY INTERFACES
             * this is why this tests checks if the field is not of Level1_2
            */
			Assert.That(versionedService, Is.Not.InstanceOf<Level1_2>());

		}

		[Test]
		public void ServedVersioningInterface ()
		{
			var versionedService = Injector.From<InterfaceVersionedBase>().Entry.level1;

			Assert.That(versionedService, Is.TypeOf<Level1B>());
		}


		[Test]
		public void SettingVersioningLatestMaj ()
		{
			var settings = StackWrapperSettings.Default;
			settings.InjectionOptions
				.VersioningMethod(ServedVersionTargetingMethod.LatestMajor,true);

			var versionedService = Injector.From<InterfaceVersionedBase>( settings ).Entry.level1;

			Assert.That(versionedService, Is.TypeOf<Level1LatestVersion>());
		}


		[Test]
		public void SettingVersioningLatestMin ()
		{
			var settings = StackWrapperSettings.Default;
			settings.InjectionOptions
				.VersioningMethod(ServedVersionTargetingMethod.LatestMinor,true);

			var versionedService = Injector.From<InterfaceVersionedBase>( settings ).Entry.level1;

			Assert.That(versionedService, Is.TypeOf<Level1B>());
		}
	}
}
