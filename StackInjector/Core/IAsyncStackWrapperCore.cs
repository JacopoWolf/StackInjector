using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace StackInjector.Core
{
    /// <summary>
    /// base interface for all asyncronous stackwrappers
    /// </summary>
    public interface IAsyncStackWrapperCore : IStackWrapperCore
    {

        /// <summary>
        /// Used to signal cancellation of every pending task
        /// </summary>
        public CancellationToken PendingTasksCancellationToken { get; }


        /// <summary>
        /// check if there are tasks left to elaborate
        /// </summary>
        /// <returns>true if there are pending tasks</returns>
        bool AnyTaskLeft ();
    }
}
