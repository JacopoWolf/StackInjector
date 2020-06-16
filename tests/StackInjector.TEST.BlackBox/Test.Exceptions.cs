﻿using System;
using System.Collections.Generic;
using NUnit.Framework;
using StackInjector.Attributes;
using StackInjector.Exceptions;
using StackInjector.Settings;
using StackInjector.TEST.ExternalAssembly;

namespace StackInjector.TEST.BlackBox
{
#pragma warning disable IDE0051, IDE0044, CS0169, CS0649

    internal class TestExceptions
    {
        private class BaseNotAServiceThrower {[Served] private List<int> integers; }

        [Test]
        public void ThrowsNotAService ()
        {
            Assert.Throws<NotAServiceException>(() => Injector.From<BaseNotAServiceThrower>());
        }

        // references class in unregistred external assembly
        private class BaseServiceNotFoundThrower {[Served] public Externalclass externalClass; }
        [Test]
        public void ThrowsServiceNotFound ()
        {
            Assert.Throws<ServiceNotFoundException>(() => Injector.From<BaseServiceNotFoundThrower>());
        }

        [Test]
        public void ExternalAssemblyReference()
        {
            var settings =
                StackWrapperSettings.Default
                .RegisterAssemblyOf<Externalclass>();

            var externalClass = Injector.From<BaseServiceNotFoundThrower>(settings).Entry.externalClass;

            Assert.That(externalClass, Is.TypeOf<Externalclass>());
        }


        private interface INoImplementationThrower { void SomeMethod (); }
        private class BaseNoImplementationThrower {[Served] private INoImplementationThrower no; }

        [Test]
        public void ThrowsImplementationNotFound ()
        {
            Assert.Throws<ImplementationNotFoundException>(() => Injector.From<BaseNoImplementationThrower>());
        }

    }
}
