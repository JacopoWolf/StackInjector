using System;
using System.Collections.Generic;
using StackInjector.Attributes;

namespace StackInjector.TEST.Versioning.Services
{

    [Service] //todo add [ServiceVersion]
    class Reciever : IStackEntryPoint
    {
        [Served] //todo add versioning method
        INiceFilter NiceFilter { get; set; }

        public IEnumerable<int> SimulatedInputs ()
        {
            // variable example input to base
            yield return 69;
            yield return 42;
            yield return 13;
            yield return 420;
        }


        public object EntryPoint ()
        {
            foreach( var obj in this.SimulatedInputs() )
                if( this.NiceFilter.IsNice(obj) )
                    return obj;
            return null;
        }

    }
}