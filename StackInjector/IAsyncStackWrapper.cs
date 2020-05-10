using System;
using System.Collections.Generic;
using System.Threading;

namespace StackInjector
{
    /// <summary>
    /// Wraps a Stack of dependency-injected classes, and manages an <see cref="IAsyncEnumerable{T}"/> of completed tasks.
    /// </summary>
    public interface IAsyncStackWrapper : IDisposable, IStackWrapperStructure
    {

        /// <summary>
        /// Used to signal cancellation of every pending job.
        /// </summary>
        public CancellationToken CancelPendingTasksToken { get; }


        /// <summary>
        /// Submit a new object to be elaborated asyncronously in this stack
        /// </summary>
        /// <param name="submitted">The object to elaborate</param>
        void Submit ( object submitted );


        /// <summary>
        /// The loop you ca use to <c>await foreach</c> tasks in elaboration, converted to the specified type.
        /// When the pending tasks list is empty, unless <see cref="IDisposable.Dispose"/> is explocitly called
        /// this will wait indefinitively.
        /// </summary>
        /// <typeparam name="T">Type to cast the object returned by the entry point</typeparam>
        /// <exception cref="InvalidCastException"></exception>
        /// <returns>An asyncronous enumerable of completed tasks</returns>
        IAsyncEnumerable<T> Elaborated<T> ();

    }
}