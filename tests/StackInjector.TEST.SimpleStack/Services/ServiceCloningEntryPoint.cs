using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using StackInjector.Attributes;
using StackInjector.Core;
using StackInjector.Wrappers;

namespace StackInjector.TEST.SimpleStack.Services
{
    class ServiceCloningEntryPoint
    {
        [Served]
        IStackWrapperCore Wrapper { get; set; }

        public void EntryPoint ()
        {

            var settings = 
                this.Wrapper
                .Settings
                .TrackInstantiationDiff();

            using var wrapper =
                this.Wrapper
                .CloneCore(settings)
                .ToWrapper<WrappedConsumerEntryPoint>();
           
            wrapper.Start( e => e.EntryPoint("sas") );


            // at this point the wrapper will be disposed
        }
    }


    class WrappedConsumerEntryPoint
    {
        [Served]
        IThingsConsumer Consumer { get; set; }

        public void EntryPoint ( string str = null )
        {
            this.Consumer.ConsumeThing( str ?? "sus");
        }
    }
}
