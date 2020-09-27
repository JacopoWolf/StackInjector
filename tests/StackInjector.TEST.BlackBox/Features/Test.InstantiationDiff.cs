using StackInjector;
using System;
using NUnit.Framework;
using StackInjector.Wrappers;
using StackInjector.Attributes;
using StackInjector.Settings;

namespace StackInjector.TEST.BlackBox.Features
{
#pragma warning disable CS0649

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
        class ServiceA : IDisposable
        {
            public bool sharedCondition;

            public void Dispose()
            {
                Console.WriteLine("I've been disposed!");
            }

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


            Assert.Throws<NullReferenceException>( () => wrapperB.Entry.Work() );

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
            

            
            Assert.DoesNotThrow( () => wrapperB.Entry.Work() ); 
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