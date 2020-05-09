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
            StackWrapperSettings
                .Default()
                .VersioningMethod( ServedVersionTagetingMethod.LatestMinor )
                .From<EntryPointTestMinor>()
                .Start();
        }

        [Test]
        public void MajorVersioning()
        {
            StackWrapperSettings
                .Default()
                .VersioningMethod( ServedVersionTagetingMethod.LatestMajor )
                .From<EntryPointTestMajor>()
                .Start();
        }



    }
}