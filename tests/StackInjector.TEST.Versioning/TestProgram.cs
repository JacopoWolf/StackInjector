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
                .VersioningMethod( ServedVersionTagettingMethod.LatestMinor )
                .From<EntryPointTestMinor>()
                .Start();
        }

        [Test]
        public void MajorVersioning()
        {
            StackWrapperSettings
                .Default()
                .VersioningMethod( ServedVersionTagettingMethod.LatestMajor )
                .From<EntryPointTestMajor>()
                .Start();
        }



    }
}