using System;
using System.Collections.Generic;
using StackInjector.Attributes;

namespace StackInjector.TEST.SimpleStack1.Services
{
    internal interface IBadThingsGenerator : IStackEntryPoint
    {
        void BadlyGenerate ();
    }


    internal class NotAServiceGenerator : IBadThingsGenerator
    {
        // property is [Served] but List<string> is not a [Service]
        [Served]
        private List<string> PoorlyWrittenProperty { get; set; }

       
        public void BadlyGenerate () => this.PoorlyWrittenProperty.Clear();

        public object EntryPoint ()
        {

            // an error should be thrown here

            return null;
        }
    }

 
    internal class NullReferenceGenerator : IBadThingsGenerator
    {
        // no [Served] attribute
        private NotAServiceGenerator NotAServiceGenerator { get; set; }


        public void BadlyGenerate () => this.NotAServiceGenerator.BadlyGenerate();

        public object EntryPoint ()
        {
            // should throw a NullReferenceException
            this.BadlyGenerate();

            return null;
        }
    }

}
