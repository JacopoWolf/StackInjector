using System;
using System.Collections.Generic;
using System.Text;
using StackInjector.Attributes;

namespace StackInjector.TEST.SimpleStack1.Services.Implementations
{
    [Service]
    class SimpleThingsGenerator : IThingsGenerator
    {
        [Served]
        IThingsFilter thingsFilter;

        [Served]
        IThingsConsumer thingsConsumer;


        // this method contains the main core of the stack, used to call every other dependency
        public void EntryPoint ()
        {
            var thing = this.GenerateThing();
            Console.WriteLine($"generated {thing}");

            var filteredthing = this.thingsFilter.FilterThing( thing );
            Console.WriteLine( $"filtered {filteredthing}");

            this.thingsConsumer.ConsumeThing(filteredthing);

        }


        public string GenerateThing ()
        {
            return "123test";
        }
    }
}
