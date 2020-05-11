using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using StackInjector.Attributes;

namespace StackInjector.TEST.SimpleStack1.Services
{
    class ServiceCloningEntryPoint : IStackEntryPoint
    {
        [Served]
        IThingsFilter Filter { get; set; }

        [Served]
        IStackWrapperStructure Wrapper { get; set; }

        public object EntryPoint ()
        {
            this.Filter.FilterThing("sas");


            var settings = 
                this.Wrapper
                .Settings
                .TrackInstantiationDiff();

            using
            ( 
                var wrapper =
                    this.Wrapper
                    .FromStructure<WrappedConsumerEntryPoint>(overrideSettings: settings) 
            )
            {
                wrapper.Start();
                // at this point the wrapper will be disposed
            }

            return null;
        }
    }


    class WrappedConsumerEntryPoint : IStackEntryPoint
    {
        [Served]
        IThingsConsumer Consumer { get; set; }

        public object EntryPoint ()
        {
            this.Consumer.ConsumeThing("sus");

            return null;
        }
    }
}
