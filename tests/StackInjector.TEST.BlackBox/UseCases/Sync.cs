using System;
using System.Linq;
using NUnit.Framework;
using StackInjector.Attributes;
using StackInjector.Core;
using StackInjector.Exceptions;
using StackInjector.Settings;
using StackInjector.TEST.ExternalAssembly;

namespace StackInjector.TEST.BlackBox.UseCases
{
	internal class Sync
    {

        [Test]
        public void FromClass ()
        {
            var baseClass = Injector.From<Base>().Entry;

            // asserts the whole structure works and references are correct.
            Assert.AreEqual(42, baseClass.Logic());
        }


        [Test]
        public void FromInterface ()
        {
            var baseClass = Injector.From<IBase>().Entry;

            // asserts the whole structure works and references are correct.
            Assert.AreEqual(42, baseClass.Logic());
        }


        [Service] private class ReferenceLoopA {[Served] public ReferenceLoopB loopB; }

        [Service] private class ReferenceLoopB {[Served] public ReferenceLoopA loopA; }

        [Test]
        public void CircularReference () => Assert.That(() => Injector.From<ReferenceLoopA>(), Throws.Nothing);


        [Test]
        public void ServeStrict ()
        {
            var settings =
                StackWrapperSettings.Default
                .InjectionServingMethods( Injector.Defaults.ServeAllStrict, true );

            var entry = Injector.From<ForgotTheServedAttributeBase>(settings).Entry;

            Assert.That(new object[] { entry.level1A, entry.Level1B }, Is.All.Null);
        }


        [Test]
        public void RemoveUnusedTypes ()
        {
            var settings =
                StackWrapperSettings.Default
                .RemoveUnusedTypesAfterInjection();

            var wrap1 = Injector.From<Level1A>( settings );

            // base is removed after injecting from a class that doesn't need it
            Assert.Throws<ServiceNotFoundException>(() => wrap1.CloneCore().ToWrapper<Base>());

        }

        [Test]
        public void AccessWrapper ()
        {
            var wrapper = Injector.From<AccessWrapperBase>();

            Assert.AreSame(wrapper, wrapper.Entry.wrapper);
        }

        [Test]
        public void ServeEnum ()
        {
            var injected = Injector.From<AllLevel1Base>().Entry.level1s;

            CollectionAssert.AreEquivalent
                (
                    new Type[] { typeof(Level1A), typeof(Level1B), typeof(Level1LatestVersion), typeof(Level1_2) },
                    injected.Select(i => i.GetType())
                );
        }

        // -----------

        [Service]
        private class AlwaysCreateBase
        {
            [Served] public AlwaysCreateInstance instance1;
            [Served] public AlwaysCreateInstance instance2;
        }

        [Service(Pattern = InstantiationPattern.AlwaysCreate)]
        private class AlwaysCreateInstance { }

        [Test]
        public void AlwaysCreate ()
        {
            var wrapper = Injector.From<AlwaysCreateBase>();

            Assert.Multiple(() =>
            {
                Assert.AreEqual(2, wrapper.GetServices<AlwaysCreateInstance>().Count());
                CollectionAssert.AllItemsAreUnique(wrapper.GetServices<AlwaysCreateInstance>());
            });

        }


        internal class Cloning
        {

            [Test]
            public void RegisterAfterCloning ()
            {
                // empty class
                var wrapper1 = Injector.From<Level1_2>();

                Assert.Multiple(() =>
                {
                    // BaseServiceNotFoundThrower uses a class in an external assembly
                    Assert.Throws<ServiceNotFoundException>(() => wrapper1.CloneCore().ToWrapper<Exceptions.BaseServiceNotFoundThrower>());

                    var settings =
                    StackWrapperSettings.Default
                    .RegisterAssemblyOf<Externalclass>()
                    .RegisterAfterCloning();

                    Assert.DoesNotThrow(() => wrapper1.CloneCore(settings).ToWrapper<Exceptions.BaseServiceNotFoundThrower>());

                });

            }

            [Test]
            public void CloneSame ()
            {
                var wrapper = Injector.From<AccessWrapperBase>();

                var cloneWrapper = wrapper.Entry.Clone();

                CollectionAssert.AreEquivalent(wrapper.GetServices<object>(), cloneWrapper.GetServices<object>());
            }

            [Test]
            public void CloneNoRepetitionsSingleton ()
            {
                var wrapper = Injector.From<IBase>();

                var clone = wrapper.CloneCore().ToWrapper<IBase>();

                Assert.AreSame(clone, clone.GetServices<IStackWrapperCore>().Single());

            }


        }


    }
}