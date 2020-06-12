using System;
using NUnit.Framework;
using StackInjector.Exceptions;
using StackInjector.TEST.SimpleStack.Services;

namespace StackInjector.TEST.SimpleStack
{

    // This whole project works as core for the testing-oriented development of the core of this library

    internal class TestProgram
    {
        [Test]
        public void WithInterfaces ()
        {
            var result = Injector.From<IThingsGenerator>().Start( e => e.StartGenerating() );

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
                        Injector.From<NotAServiceGenerator>().Start( e => e.EntryPoint()  );
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
                        Injector.From<NullReferenceGenerator>().Start( e => e.EntryPoint() );
                    }
                );
        }

        [Test]
        public void BadEntryPoint()
        {
            Assert.Throws
                (
                    typeof(InvalidEntryTypeException),
                    () =>
                    {
                        Injector.From<BadEntryPointGenerator>();
                    }
                );
        }


        [Test]
        public void AccessWrapper()
        {
            Injector.From<AccessWrapperEntryPoint>().Start( e => e.EntryPoint() );
        }

        [Test]
        public void Cloning()
        {
            Injector.From<ServiceCloningEntryPoint>().Start( e => e.EntryPoint() );
        }

    }
}
