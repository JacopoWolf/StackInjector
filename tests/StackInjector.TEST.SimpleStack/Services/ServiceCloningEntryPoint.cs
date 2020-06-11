using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using StackInjector.Attributes;
using StackInjector.Core;
using StackInjector.Wrappers;

namespace StackInjector.TEST.SimpleStack1.Services
{
    class ServiceCloningEntryPoint
    {
        [Served]
        IThingsFilter Filter { get; set; }

        [Served]
        IStackWrapperCore Wrapper { get; set; }

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
                    .CloneCore(settings)
                    .ToWrapper<WrappedConsumerEntryPoint>()
            )
            {
                wrapper.Start( e => e.EntryPoint() );
                // at this point the wrapper will be disposed
            }

            return null;
        }
    }


    class WrappedConsumerEntryPoint
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
