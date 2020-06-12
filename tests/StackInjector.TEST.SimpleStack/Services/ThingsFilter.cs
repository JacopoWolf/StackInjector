using System;
using StackInjector.Attributes;

namespace StackInjector.TEST.SimpleStack.Services
{
    internal interface IThingsFilter
    {
        string FilterThing ( string raw );
    }

    // when generating a new StackWrapper from an interface if multiple classes implement said interface
    // the order in which those classes are defined matters. 
    // In this case it is suggested to use the specific class name.

    [Service]
    internal class SpecificThingSubFilter : IThingsFilter
    {
        public string FilterThing ( string raw )
        {
            Console.WriteLine(raw);
            return raw.Remove(raw.Length - 2);
        }
    }

    [Service]
    internal class SimpleThingsFilter : IThingsFilter
    {
        [Served]
        private SpecificThingSubFilter SpecificFilter { get; set; }

        public string FilterThing ( string raw )
        {
            return this.SpecificFilter.FilterThing(raw.Remove(0, 3));
        }
    }

}
