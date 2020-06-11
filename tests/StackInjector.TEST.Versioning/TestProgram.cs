using NUnit.Framework;
using StackInjector.Settings;
using StackInjector.TEST.Versioning.Services;

namespace StackInjector.TEST.Versioning
{
    internal class TestProgram
    {

        [Test]
        public void MinorVersioning()
        {
            var settings =
                StackWrapperSettings
                .Default
                .VersioningMethod(ServedVersionTargetingMethod.LatestMinor);

            Injector.From<EntryPointTestMinor>( settings ).Start( e => e.EntryPoint() );
        }

        [Test]
        public void MajorVersioning()
        {
            var settings =
                StackWrapperSettings
                .Default
                .VersioningMethod(ServedVersionTargetingMethod.LatestMajor);

            Injector.From<EntryPointTestMajor>( settings ).Start( e => e.EntryPoint() );
        }



    }
}