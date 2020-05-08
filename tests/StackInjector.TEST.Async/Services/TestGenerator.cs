using System;
using System.Collections.Generic;
using System.Text;
using StackInjector.Attributes;

namespace StackInjector.TEST.Async.Services
{
    [Service]
    class TestGenerator : IStackEntryPoint
    {

        [Served]
        IStackWrapper wrapper;

        
        IAsyncStackWrapper elaborationWrapper;


        IEnumerable<int> GenerateDirty()
        {
            for( int i = 2; i < 100; i++ )
                yield return i + 42; // add noise
        }
        

        public object EntryPoint ()
        {
            


            return 0;
        }
    }
}
