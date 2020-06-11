using System.Linq;
using System.Collections;
using NUnit.Framework;
using StackInjector.Settings;
using StackInjector.TEST.Async.Services;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace StackInjector.TEST.Async
{
    public class Tests
    {
        [Test][Retry(3)]
        public async Task TestAsync ()
        {
            var feed = Enumerable.Range(1, 11);

            // wait for a maximum of 500 milliseconds
            var settings =
                StackWrapperSettings.Default
                .WhenNoMoreTasks(AsyncWaitingMethod.Timeout, 500 );

            using var asyncwrapper = Injector.AsyncFrom<PowElaborator,int,int>( (e,i,t) => e.Digest(i,t),  settings );

            var feeder = Task.Run
            ( 
                () =>
                {
                    foreach( var item in feed )
                        asyncwrapper.Submit(item);
                } 
            );

            // submit a new element ~100 milliseconds before the wrapper exits the loop
            var test = Task.Run( () => { Task.Delay(400).Wait(); asyncwrapper.Submit(42); });


            var resStr = new List<int>();

            await foreach( var result in asyncwrapper.Elaborated() )
                resStr.Add(result);

            await test;
            await feeder;

            CollectionAssert.AreEquivalent( new int[] { 1, 4, 9, 16, 25, 36, 49, 64, 81, 100, 121, 1764 }, resStr );

        }


        //? this should be reworked
        [Test]
        public async Task TestGenericAsync()
        {
            var feed = Enumerable.Range(1, 11);

            var settings =
                StackWrapperSettings.Default
                .WhenNoMoreTasks(AsyncWaitingMethod.Exit);

            using var wrapper = 
                Injector.AsyncFrom<PowElaborator,int,double>
                ( 
                    // could call any method here
                    async (i,e,t) => (double) await i.Digest(e,t), 
                    settings 
                );

            Console.WriteLine(wrapper);


            foreach( var item in feed )
                wrapper.Submit(item);


            var counter = 0;
            await foreach( var result in wrapper.Elaborated() )
            {
                counter++;
                Console.Write($"{result}; ");
            }

            Assert.AreEqual(feed.Count(), counter);

        }


        [Test]
        public void ServeAsync ()
        {
            Injector.From<TestGenerator>().Start( e => e.EntryPoint() );
        }
    }
}