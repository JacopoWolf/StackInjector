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
        [Test][Retry(5)]
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
            await foreach( var result in asyncwrapper.Elaborated<double>() )
                if( counter++ < 10 )
                    Console.Write($"{result}; ");
                else
                    break;


            //? kinda useless... but i dunno. should rewrite it
            Assert.AreEqual(feed.Count(), counter);

        }




        [Test]
        public void ServeAsync ()
        {
            Injector.From<TestGenerator>().Start();
        }
    }
}