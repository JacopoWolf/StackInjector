using System;
using StackInjector.Attributes;
using StackInjector.Wrappers;

namespace StackInjector.TEST.SimpleStack1.Services
{
    internal interface IThingsGenerator : IStackEntryPoint
    {
        string GenerateThing ();
    }

    [Service]
    internal class SimpleThingsGenerator : IThingsGenerator
    {
        [Served]
        private SimpleThingsFilter ThingsFilter { get; set; }

        [Served]
        private IThingsConsumer ThingsConsumer { get; set; }


        // this method contains the main core of the stack, used to call every other dependency
        public object EntryPoint ()
        {
            var thing = this.GenerateThing();
            Console.WriteLine($"generated {thing}");

            var filteredthing = this.ThingsFilter.FilterThing( thing );
            Console.WriteLine($"filtered {filteredthing}");

            this.ThingsConsumer.ConsumeThing(filteredthing);

            return filteredthing;
        }


        public string GenerateThing ()
        {
            return "123test45";
        }
    }


    [Service]
    internal class AccessWrapperEntryPoint : IStackEntryPoint
    {
        [Served]
        IStackWrapper Wrapper { get; set; }

        public object EntryPoint ()
        {
            Console.WriteLine( this.Wrapper.ToString() );

            return null;
        }
    }

}
