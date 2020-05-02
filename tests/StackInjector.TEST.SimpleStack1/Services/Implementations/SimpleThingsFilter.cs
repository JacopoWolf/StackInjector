﻿using System;
using System.Collections.Generic;
using System.Text;
using StackInjector.Attributes;

namespace StackInjector.TEST.SimpleStack1.Services.Implementations
{
    [Service]
    class SimpleThingsFilter : IThingsFilter
    {
        [Served]
        SpecificThingSubFilter SpecificFilter { get; set; }

        public string FilterThing ( string raw )
        {
            return this.SpecificFilter.FilterThing( raw.Remove(0, 3) );
        }
    }
}