using StackInjector;
using System;
using NUnit.Framework;
using StackInjector.Wrappers;
using StackInjector.Attributes;
using StackInjector.Settings;
using System.Linq;
using System.Transactions;
using StackInjector.Exceptions;

namespace StackInjector.TEST.BlackBox.Features
{
#pragma warning disable CS0649

    //[Ignore("Feature still under development after massive internal rewrite.")]
    class InstantiationDiff
    {
        [Service]
        class WrapperBase
        {
            [Served]
            public readonly ServiceA serviceA;

            public void Work()
            {
                this.serviceA.sharedCondition = true;
            }

        }

        [Service]
        class ServiceA
        {
            public bool sharedCondition;
        }


        [Test]
        public void SimpleClone ()
        {
            var settings = StackWrapperSettings.Default
                            .TrackInstantiationDiff();

            IStackWrapper<WrapperBase> wrapperB;

            var wrapperA = Injector.From<WrapperBase>(settings);

            wrapperA.Start(e => e.Work());
            wrapperB = wrapperA.CloneCore().ToWrapper<WrapperBase>();


            Assert.Multiple(() =>
            {
                Assert.DoesNotThrow(() => wrapperB.Entry.Work());
                Assert.AreSame
                (
                    wrapperA.GetServices<ServiceA>().First(), 
                    wrapperB.GetServices<ServiceA>().First()
                );
            });
        }

        [Test]
        public void SimpleCloneWithDispose ()
        {
            var settings = StackWrapperSettings.Default
                            .TrackInstantiationDiff();

            IStackWrapper<WrapperBase> wrapperB;

            using( var wrapperA = Injector.From<WrapperBase>(settings) )
            {
                wrapperA.Start(e => e.Work());
                wrapperB = wrapperA.CloneCore().ToWrapper<WrapperBase>();
            }

            // after a deep clone there are no instances in wrapperB
            Assert.Throws<InvalidEntryTypeException>( () => wrapperB.Entry.Work() );

        }


        [Test]
        public void DeepClone ()
        {
            var settings = StackWrapperSettings.Default
                            .TrackInstantiationDiff();

            IStackWrapper<WrapperBase> wrapperB;

            var wrapperA = Injector.From<WrapperBase>(settings);

            wrapperA.Start(e => e.Work());
            wrapperB = wrapperA.DeepCloneCore().ToWrapper<WrapperBase>();

            Assert.Multiple(() =>
            {
                Assert.DoesNotThrow(() => wrapperB.Entry.Work());
                Assert.AreNotSame
                (
                    wrapperA.GetServices<ServiceA>().First(), 
                    wrapperB.GetServices<ServiceA>().First()
                );
            });
        }

        [Test]
        public void DeepCloneWithDispose ()
        {
            var settings = StackWrapperSettings.Default
                            .TrackInstantiationDiff();

            IStackWrapper<WrapperBase> wrapperB;

            using( var wrapperA = Injector.From<WrapperBase>(settings) )
            {
                wrapperA.Start(e => e.Work());
                wrapperB = wrapperA.DeepCloneCore().ToWrapper<WrapperBase>();
            }

            // wrapper B is a deep clone. Being different objects, disposing one won't interact with the other
            Assert.DoesNotThrow( () => wrapperB.Entry.Work() );

        }
    }
}