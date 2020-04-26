using System;
using System.Collections.Generic;
using System.Text;
using StackInjector.Attributes;

namespace StackInjector.TEST.SimpleStack1.Services.Implementations
{
    [Service]
    class SpecificThingSubFilter : IThingsFilter
    {
        public string FilterThing ( string raw )
        {
            return raw.Remove(raw.Length-2);
        }
    }
}
