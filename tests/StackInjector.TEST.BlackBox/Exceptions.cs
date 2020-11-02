using System.Collections.Generic;
using NUnit.Framework;
using StackInjector.Attributes;
using StackInjector.Exceptions;
using StackInjector.Settings;
using StackInjector.TEST.ExternalAssembly;

namespace StackInjector.TEST.BlackBox
{

#pragma warning disable IDE0051, IDE0044, CS0169, CS0649

    internal class Exceptions
    {
        //  ----------

        [Service]
        private class BaseNotAServiceThrower {[Served] private List<int> integers; }

        [Test]
        public void ThrowsNotAService ()
            => Assert.Throws<NotAServiceException>(() => Injector.From<BaseNotAServiceThrower>());

        //  ----------

        private class BaseNotAService { }

        [Test]
        public void ThrowsBaseNotAService ()
            => Assert.Throws<NotAServiceException>(() => Injector.From<BaseNotAService>());

        //  ----------

        // references class in unregistred external assembly
        [Service]
        private class BaseServiceNotFoundThrower {[Served] public Externalclass externalClass; }

        [Test]
        public void ThrowsServiceNotFound ()
            => Assert.Throws<ServiceNotFoundException>(() => Injector.From<BaseServiceNotFoundThrower>());


        [Test]
        public void ExternalAssemblyReference ()
        {
            var settings =
                StackWrapperSettings.Default
                .RegisterAssemblyOf<Externalclass>();

            var externalClass = Injector.From<BaseServiceNotFoundThrower>(settings).Entry.externalClass;

            Assert.That(externalClass, Is.TypeOf<Externalclass>());
        }


        [Test]
        public void ExternalAllAssemblyReference ()
        {
            var settings =
                StackWrapperSettings.Default
                .RegisterDomain();


            var externalClass = Injector.From<BaseServiceNotFoundThrower>(settings).Entry.externalClass;

            Assert.That(externalClass, Is.TypeOf<Externalclass>());
        }


        //  ----------

        private interface INoImplementationThrower { void SomeMethod (); }
        [Service] private class BaseNoImplementationThrower {[Served] private INoImplementationThrower no; }

        [Test]
        public void ThrowsImplementationNotFound ()
            => Assert.Throws<ImplementationNotFoundException>(() => Injector.From<BaseNoImplementationThrower>());

        //  ----------

        [Service(Pattern = InstantiationPattern.AlwaysCreate)] private class InvalidEntryTypeThrower { }

        [Test]
        public void ThrowsInvalidEntryType ()
            => Assert.Throws<InvalidEntryTypeException>(() => Injector.From<InvalidEntryTypeThrower>());

        //  ----------

        [Service]
        private class BaseNoParameterlessConstructorThrower
        { public int test; public BaseNoParameterlessConstructorThrower ( int test ) => this.test = test; }

        [Test]
        public void ThrowsMissingParameterlessConstructor ()
            => Assert.Throws<MissingParameterlessConstructorException>(() => Injector.From<BaseNoParameterlessConstructorThrower>());

        //  ----------

        [Service]
        private class BaseNoSetterThrower {[Served] public Base Base { get; } }

        [Test]
        public void ThrowsNoSetter ()
            => Assert.Throws<NoSetterException>(() => Injector.From<BaseNoSetterThrower>());

    }
}
