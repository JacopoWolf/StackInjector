using System.Linq;
using System.Collections;
using NUnit.Framework;
using StackInjector.Settings;
using StackInjector.TEST.Async.Services;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace StackInjector.TEST.Async
{
    public class Tests
    {
        [Test]
        public async Task TestAsync ()
        {
            var feed = Enumerable.Range(1, 11);

            using var asyncwrapper = Injector.AsyncFrom<PowElaborator>();

            // takes up to 1 second, waiting for 0.1 seconds every submission
            var feeder = Task.Run
            ( 
                () =>
                {
                    foreach( var item in feed )
                    {
                        asyncwrapper.Submit(item);

                        Thread.Sleep(10);   // <<<----- I want to highlight this this is for asyncronous testing purposes
                                            // and it's the cause of the 100+ milliseconds of elaboration required for this test
                    }
                } 
            );

            var counter = 0;
            await foreach( var result in asyncwrapper.Elaborated() )
                if( counter++ < 10 )
                    Console.Write($"{result}; ");
                else
                    asyncwrapper.Dispose(); // test if disposing of asyncwrapper will stop the loop

            Assert.AreEqual(feed.Count(), counter);

        }


        // this test is 5-10 milliseconds FASTER than the generic counterpart.
        // I guess type safety really fastens it up a lot
        [Test]
        public async Task TestGenericAsync()
        {
            var feed = Enumerable.Range(1, 11);

            using var wrapper = Injector.AsyncFrom<PowElaborator,int,double>( async (i,e,t) => (double) await i.Digest(e,t)  );

            // takes up to 1 second, waiting for 0.1 seconds every submission
            var feeder = Task.Run
            (
                () =>
                {
                    foreach( var item in feed )
                    {
                        wrapper.Submit(item);

                        Thread.Sleep(10);   // same as above
                    }
                }
            );

            var counter = 0;
            await foreach( var result in wrapper.Elaborated() )
                if( counter++ < 10 )
                    Console.Write($"{result}; ");
                else
                    break;

            Assert.AreEqual(feed.Count(), counter);

        }


        [Test]
        public void ServeAsync ()
        {
            Injector.From<TestGenerator>().Start();
        }
    }
}