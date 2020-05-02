using System;
using System.Collections.Generic;
using System.Text;
using StackInjector.Attributes;

namespace StackInjector.TEST.SimpleStack1.Services
{
    interface IBadThingsGenerator : IStackEntryPoint
    {
        void BadlyGenerate ();
    }

    [Service]
    class BadThingsGenerator : IBadThingsGenerator
    {
        [Served]
        List<string> poorlyWrittenProperty;

        public void BadlyGenerate () => throw new NotImplementedException();
        public object EntryPoint ()
        {

            // an error should be thrown here

            return null;
        }
    }

}
