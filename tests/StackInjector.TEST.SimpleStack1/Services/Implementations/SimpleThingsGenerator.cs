using System;
using System.Collections.Generic;
using System.Text;
using StackInjector.Attributes;

namespace StackInjector.TEST.SimpleStack1.Services.Implementations
{
    [Service]
    class SimpleThingsGenerator : IThingsGenerator
    {
        public void EntryPoint ()
        {

        }


        public string GenerateThing ()
        {
            return "test";
        }
    }
}
