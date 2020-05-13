﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StackInjector.Core
{
    /// <summary>
    /// base interface for all asyncronous stackwrappers.
    /// </summary>
    /// <typeparam name="T">the type tasks will return</typeparam>
    public interface IAsyncStackWrapperCore<T> : IStackWrapperCore
    {

        /// <summary>
        /// Used to signal cancellation of every pending task
        /// </summary>
        CancellationToken PendingTasksCancellationToken { get; }


        /// <summary>
        /// submit new work to this wrapper
        /// </summary>
        /// <param name="work"></param>
        void Submit ( Task<T> work );

        /// <summary>
        /// The loop you ca use to <c>await foreach</c> tasks in elaboration, converted to the specified type.
        /// When the pending tasks list is empty, unless <see cref="IDisposable.Dispose"/> is explocitly called
        /// this will wait indefinitively.
        /// </summary>
        /// <returns>An asyncronous enumerable of completed tasks</returns>
        IAsyncEnumerable<T> Elaborated ();

        /// <summary>
        /// check if there are tasks left to elaborate
        /// </summary>
        /// <returns>true if there are pending tasks</returns>
        bool AnyTaskLeft ();
    }
}