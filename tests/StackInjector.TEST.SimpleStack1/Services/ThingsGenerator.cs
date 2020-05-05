﻿using System;
using StackInjector.Attributes;

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

}