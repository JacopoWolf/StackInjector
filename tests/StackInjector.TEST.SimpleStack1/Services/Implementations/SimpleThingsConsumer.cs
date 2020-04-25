using System;
using System.Collections.Generic;
using System.Text;
using StackInjector.Attributes;

namespace StackInjector.TEST.SimpleStack1.Services.Implementations
{
    [Service]
    class SimpleThingsConsumer : IThingsConsumer
    {
        public void ConsumeThing ( string thing )
        {
            Console.WriteLine($"consumed {thing}");
        }
    }
}
