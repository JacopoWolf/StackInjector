using NUnit.Framework;
using StackInjector.Settings;
using StackInjector.TEST.Versioning.Services;

namespace StackInjector.TEST.Versioning
{
    internal class TestProgram
    {
        private StackWrapperSettings UsedSettings { get; set; }

        [SetUp]
        public void SetUp()
        {
            this.UsedSettings =
                StackWrapperSettings
                    .Default()
                    .Register( typeof(TestProgram).Assembly )
                    .VersioningMethod( ServedVersionTagettingMethod.Exact );
        }

        [Test]
        public void Versioning()
        {
            Injector.From<EntryPointTest>(this.UsedSettings).Start();
        }
    }
}