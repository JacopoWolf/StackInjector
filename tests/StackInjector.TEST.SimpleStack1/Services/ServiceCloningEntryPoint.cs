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

            var wrapper = this.Wrapper.FromStructure<WrappedConsumerEntryPoint>();

            wrapper.Start();
            // now there should still be instances of a wrapped consumerEntryPoint and a ithingsconsumer


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
