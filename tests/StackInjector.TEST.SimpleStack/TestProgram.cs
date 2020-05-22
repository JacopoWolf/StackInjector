using System;
using NUnit.Framework;
using StackInjector.Exceptions;
using StackInjector.TEST.SimpleStack1.Services;

namespace StackInjector.TEST.SimpleStack1
{

    // This whole project works as core for the testing-oriented development of the core of this library

    internal class TestProgram
    {
        [Test]
        public void WithInterfaces ()
        {
            var result = Injector.From<IThingsGenerator>().Start();

            Assert.AreEqual("test", result);
        }

        [Test]
        public void NotAService ()
        {
            Assert.Throws
                (
                    typeof(NotAServiceException),
                    () =>
                    {
                        Injector.From<NotAServiceGenerator>().Start();
                    }
                );
        }

        [Test]
        public void NullReference ()
        {
            Assert.Throws
                (
                    typeof(NullReferenceException),
                    () =>
                    {
                        Injector.From<NullReferenceGenerator>().Start();
                    }
                );
        }

        [Test]
        public void AccessWrapper()
        {
            Injector.From<AccessWrapperEntryPoint>().Start();
        }

        [Test]
        public void Cloning()
        {
            Injector.From<ServiceCloningEntryPoint>().Start();
        }

    }
}
