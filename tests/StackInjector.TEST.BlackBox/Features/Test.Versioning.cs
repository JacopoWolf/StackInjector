using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using StackInjector.Attributes;
using StackInjector.Settings;

namespace StackInjector.TEST.BlackBox.Features
{
    internal class Versioning
    {
        #pragma warning disable CS0649

        [Service] private class VersionClass {[Served] internal Level1A Level1_2; }

        [Test]
        public void ServerdVersioningClass ()
        {
            var settings =
                StackWrapperSettings.Default
                .InjectionVersioningMethod(ServedVersionTargetingMethod.LatestMajor,true);

            var versionedService = Injector.From<VersionClass>(settings).Entry.Level1_2;

            /* CLASSES ARE NOT VERSIONED, ONLY INTERFACES
             * this is why this tests checks if the field is not of Level1_2
            */
            Assert.That(versionedService, Is.Not.InstanceOf<Level1_2>());

        }


        [Test]
        public void SettingVersioningLatestMaj ()
        {
            var settings =
                StackWrapperSettings.Default
                .InjectionVersioningMethod(ServedVersionTargetingMethod.LatestMajor,true);

            var versionedService = Injector.From<InterfaceVersionedBase>( settings ).Entry.level1;

            Assert.That(versionedService, Is.TypeOf<Level1LatestVersion>());
        }


        [Test]
        public void SettingVersioningLatestMin ()
        {
            var settings =
                StackWrapperSettings.Default
                .InjectionVersioningMethod(ServedVersionTargetingMethod.LatestMinor,true);

            var versionedService = Injector.From<InterfaceVersionedBase>( settings ).Entry.level1;

            Assert.That(versionedService, Is.TypeOf<Level1B>());
        }
    }
}
