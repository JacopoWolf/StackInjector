using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StackInjector.Attributes;
using StackInjector.Core;
using StackInjector.Wrappers;

namespace StackInjector.TEST.Async.Services
{
    // [Service] is not necessary since this class is not a service, but an entry point.
    // it would've been necessary if it implemented and interface and that was the entry point.
    class TestGenerator
    {
        // this object is being server in an asyncronous wrapper, therefore we can refence it's container!
        [Served]
        IStackWrapperCore Wrapper { get; set; }

        [Served]
        AnswerFilter Filter { get; set; }


        IEnumerable<int> GenerateDirty()
        {
            for( int i = 2; i < 19; i++ )
                yield return i + 42; // add noise
        }
        

        public object EntryPoint ()
        {
            // logging
            Console.WriteLine( this.Wrapper.ToString() );

            // clone the existing structure and initialize a new asyncronous elaborator! Saves time
            using var elaborationWrapper = this.Wrapper.CloneCore().ToAsyncWrapper<PowElaborator,int,int>( (e,i,t) => e.Digest(i,t) );

            // logging
            Console.WriteLine(elaborationWrapper.ToString());

            // simulate some syncronous reading input
            foreach( var num in this.GenerateDirty() )
                elaborationWrapper.Submit(this.Filter.FilterAnswer(num));

            // simulate a decently written consumption asyncronous method
            async Task<IEnumerable<double>> waitForRes ()
            {
                List<double> results = new List<double>();

                int counter = 2;
                await foreach( var res in elaborationWrapper.Elaborated() )
                    if( counter++ < 18 ) // we don't want to wait forever.
                        results.Add( (double)res );
                    else
                        break;

                return results;
            }

            // print the results cuz why not. You did all of this calculation you earned it my techy boi
            foreach( var d in waitForRes().Result )
                Console.Write($"{d}; ");
            Console.WriteLine();


            elaborationWrapper.Dispose();
            Console.WriteLine(elaborationWrapper);


            return 0;
        }
    }


    [Service]
    class AnswerFilter
    {

        public int FilterAnswer ( int item ) => item - 42;

    }


}
