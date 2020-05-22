using System;
using StackInjector.Attributes;

namespace StackInjector.TEST.SimpleStack1.Services
{
    internal interface IThingsConsumer
    {
        void ConsumeThing ( string thing );
    }

    [Service]
    internal class SimpleThingsConsumer : IThingsConsumer
    {
        public void ConsumeThing ( string thing )
        {
            Console.WriteLine($"consumed {thing}");
        }
    }

}
