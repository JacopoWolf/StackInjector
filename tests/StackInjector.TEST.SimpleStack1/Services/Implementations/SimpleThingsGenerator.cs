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


        // this method contains the main core of the stack, used to call every other dependency
        public void EntryPoint ()
        {
            var thing = this.GenerateThing();
            this.thingsFilter.FilterThing( thing );
        }


        public string GenerateThing ()
        {
            return "123test";
        }
    }
}
