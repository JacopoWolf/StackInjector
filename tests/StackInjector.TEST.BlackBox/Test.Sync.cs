using NUnit.Framework;
using StackInjector.Settings;
using StackInjector.TEST.BlackBox.SimpleStructure;

namespace StackInjector.TEST.BlackBox
{
    internal class TestSync
    {

        [Test]
        public void SimpleWithClassses ()
        {
            var baseClass = Injector.From<Base>().Entry;

            // asserts the whole structure works and references are correct.
            Assert.AreEqual(42, baseClass.Logic());
        }

        [Test]
        public void SimpleFromInterfaces ()
        {
            var baseClass = Injector.From<IBase>().Entry;

            // asserts the whole structure works and references are correct.
            Assert.AreEqual(42, baseClass.Logic());
        }

        [Test]
        public void ServedVersioning ()
        {
            var versionedService = Injector.From<VersionedBase>().Entry.level1;

            Assert.That(versionedService, Is.TypeOf<Level1B>());
        }

        [Test]
        public void SettingVersioningLatestMaj ()
        {
            var settings =
                StackWrapperSettings.Default
                .InjectionVersioningMethod(ServedVersionTargetingMethod.LatestMajor,true);

            var versionedService = Injector.From<VersionedBase>( settings ).Entry.level1;

            Assert.That(versionedService, Is.TypeOf<Level1LatestVersion>());
        }

        [Test]
        public void SettingVersioningLatestMin ()
        {
            var settings =
                StackWrapperSettings.Default
                .InjectionVersioningMethod(ServedVersionTargetingMethod.LatestMinor,true);

            var versionedService = Injector.From<VersionedBase>( settings ).Entry.level1;

            Assert.That(versionedService, Is.TypeOf<Level1B>());
        }
    }
}