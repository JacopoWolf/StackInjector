using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StackInjector
{
    /// <summary>
    /// Wraps a Stack of dependency-injected classes, and manages an <see cref="IAsyncEnumerable{T}"/> of completed tasks.
    /// </summary>
    public interface IAsyncStackWrapper : IDisposable, IStackWrapperStructure
    {
        /// <summary>
        /// Used to cancel every pending job.
        /// </summary>
        public CancellationToken CancelPendingTasksToken { get; }

        /// <summary>
        /// Submit a new object to be elaborated asyncronously in this stack
        /// </summary>
        /// <param name="submitted">The object to elaborate</param>
        /// <returns>The elaborated object</returns>
        void Submit ( object submitted );


        /// <summary>
        /// The loop you ca use to <c>await foreach</c> Tasks in elaboration, converted to the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IAsyncEnumerable<T> Elaborated<T> ();

    }
}
