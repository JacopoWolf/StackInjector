using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using StackInjector.Attributes;
using StackInjector.Settings;
using CTkn = System.Threading.CancellationToken;

namespace StackInjector.TEST.BlackBox.UseCases
{

#pragma warning disable IDE0051, IDE0044, CS0169, CS0649

    internal class Async
    {
        //todo test is broken. Rewrite.
        /*x
        [Test]
        [Timeout(500)]
        [Retry(3)]
        public async Task Simple ()
        {
            
            using var wrapper = Injector.AsyncFrom
                (
                    ( BaseAsync e, int i, CTkn t ) => e.Logic( i, t ),
                    StackWrapperSettings.Default
                );


            var rnd = new Random(42);
            foreach( var item in Enumerable.Range(0, 6).OrderBy(i => rnd.Next()) )
                wrapper.Submit(item);


            var results = new List<int>();
            await foreach( var result in wrapper.Elaborated() )
                results.Add(result);


            CollectionAssert.AreEquivalent
            (
                new int[] { 10, 11, 12, 13, 14, 15 },
                results
            );
            
        }
        */

        [Test]
        [Retry(3)]
        public void AsyncConcurrentExecution ()
        {
            var settings = StackWrapperSettings
                .Default
                .WhenNoMoreTasks(AsyncWaitingMethod.Exit);


            // does nothing but waiting
            var wrapper = Injector.AsyncFrom<EmptyTestClass,int,int>
                (
                    // enough delay for T1 to NOT FINISH execution before the second method call,
                    // if T1 has finished executing then the second method will run empty withouth errors
                    async (e, i, t) => { await Task.Delay(10000); return i; },
                    settings
                );

            // test callback
            wrapper.OnElaborated += ( i ) => Console.Write(i);

            // test submit
            wrapper.Submit(420);

            // 1st call
            var t1 = wrapper.Elaborate();

            // 2nd call. 
            // If t1 is still elaborating this is an invalid operation
            Assert.Throws<InvalidOperationException>(() => wrapper.Elaborate());
        }

    }
}