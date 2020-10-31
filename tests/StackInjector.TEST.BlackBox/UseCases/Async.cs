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
        /*NOTE: AsyncStackWrapper is an extension od the normal wrapper,
         * there is a common core logic; this means that the tests done in UseCases.Sync
         * are also valid for the AsyncStackWrapper, since the tested code is the same
         * (injection logic)
         */

        // base async test class
        [Service]
        class AsyncBase
        {
            internal async Task<object> WaitForever ( object obj, CTkn tkn )
            {
                // waits forever unless cancelled
                await Task.Delay(-1,tkn);
                return obj;
            }

            internal async Task<object> ReturnArg ( object obj, CTkn tkn )
            {
                // waits forever unless cancelled
                await Task.Delay(0, tkn);
                return obj;
            }
        }



        [Test]
        public void SubmitNoElaboration ()
        {
            using var wrapper = Injector.AsyncFrom<AsyncBase,object,object>( (b,i,t) => b.WaitForever(i,t) );
            wrapper.Submit(new object());

            Assert.Multiple(() =>
            {
                Assert.IsTrue(wrapper.AnyTaskLeft());
                Assert.IsFalse(wrapper.AnyTaskCompleted());
            });

        }

        [Test]
        public void SubmitAndCatchAsyncEnumerable ()
        {
            using var wrapper = Injector.AsyncFrom<AsyncBase,object,object>( (b,i,t) => b.ReturnArg(i,t) );
            var task1 = wrapper.Submit(new object());
            var task2 = wrapper.Submit(new object());

            Assert.Multiple(() =>
            {

            });

        }
    }
}