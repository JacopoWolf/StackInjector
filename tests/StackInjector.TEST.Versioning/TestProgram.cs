using NUnit.Framework;
using StackInjector.Settings;

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
                    .Create()
                    .Register( typeof(TestProgram).Assembly )
                    .VersioningMethod( ServedVersionTagettingMethod.Exact );
        }

        [Test]
        public void VersioningV1()
        {

            

            Assert.Pass();
        }
    }
}