using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StackInjector.Core
{
    /// <summary>
    /// Base interface for all asyncronous stackwrappers.
    /// </summary>
    /// <typeparam name="T">the type tasks will return</typeparam>
    public interface IAsyncStackWrapperCore<T> : IStackWrapperCore
    {
        /// <summary>
        /// called when a new element has been elaborated
        /// </summary>
        event Action<T> OnElaborated;

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
        /// <exception cref="InvalidOperationException"></exception>
        IAsyncEnumerable<T> Elaborated ();

        /// <summary>
        /// Calling this method will begin the elaboration loop.<br/>
        /// If a concurrent call to the <see cref="Elaborated"/> is made, this will return an exception
        /// </summary>
        /// <returns>a task rappresenting the elaboration loop</returns>
        /// <exception cref="InvalidOperationException"></exception>
        Task Elaborate ();

        /// <summary>
        /// check if there are tasks left to elaborate
        /// </summary>
        /// <returns>true if there are pending tasks</returns>
        bool AnyTaskLeft ();

        /// <summary>
        /// check if any pending task has been completed
        /// </summary>
        /// <returns>true if any task completed</returns>
        bool AnyTaskCompleted ();
    }
}
