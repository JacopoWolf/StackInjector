using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using StackInjector.Attributes;
using StackInjector.Core;
using StackInjector.Core.Cloning;
using StackInjector.Settings;

namespace StackInjector.TEST.BlackBox
{

    // set as readonly, unused field
#pragma warning disable CS0649, IDE0044, IDE0051


    // base -> level 1 -> level 2

    #region base



    [Service]
    class BaseAsync
    {
        [Served]
        ILevel1Async Level1Async;

        public int toSum = 10;

        // kind of a Fibonacci sequence
        public async Task<int> Logic ( int item, CancellationToken token )
        {
            if( token.IsCancellationRequested )
                return -1;

            return await this.Level1Async.OperateOn(this.toSum, item);
        }
        
    }

    #endregion

    #region level 1

    interface ILevel1Async
    {
        Task<int> OperateOn ( int a, int b );
    }

    [Service]
    class Level1Async : ILevel1Async
    {
        [Served]
        Delayer delayer;

        public async Task<int> OperateOn ( int a, int b )
        {
            await this.delayer.SlowAsyncMethod();
            return a + b;
        }
    }


    #endregion

    #region level 2

    [Service]
    class Delayer
    {
        public async Task SlowAsyncMethod ()
        {
            await Task.Delay(50);
        }
    }


    #endregion

}