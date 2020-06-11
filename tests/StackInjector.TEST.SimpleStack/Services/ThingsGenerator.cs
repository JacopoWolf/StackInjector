using System;
using StackInjector.Attributes;
using StackInjector;
using StackInjector.Core;

namespace StackInjector.TEST.SimpleStack1.Services
{
    internal interface IThingsGenerator
    {
        object StartGenerating ();
        string GenerateThing ();
    }

    [Service(Serving = Injector.Defaults.ServeAll)]
    internal class SimpleThingsGenerator : IThingsGenerator
    {
        private SimpleThingsFilter ThingsFilter { get; set; }

        private IThingsConsumer ThingsConsumer { get; set; }


        // this method contains the main core of the stack, used to call every other dependency
        public object StartGenerating ()
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
    internal class AccessWrapperEntryPoint
    {
        [Served]
        IStackWrapperCore Wrapper { get; set; }

        public object EntryPoint ()
        {
            Console.WriteLine( this.Wrapper.ToString() );

            return null;
        }
    }

}
