using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StackInjector.Core;

namespace StackInjector.Wrappers.Generic
{
    /// <summary>
    /// rappresents a strongly typed generic entry point 
    /// of an <see cref="IAsyncStackWrapper{TEntry, TIn, TOut}"/>
    /// </summary>
    /// <typeparam name="TIn">type on the submitted item</typeparam>
    /// <typeparam name="TOut">type of the return item</typeparam>
    /// <param name="item">the item to elaborate</param>
    /// <param name="cancellationToken">the cancellation token used to cancel the task</param>
    /// <returns>a task rappresenting the current job</returns>
    public delegate Task<TOut> AsyncStackDigest<in TIn, TOut> 
        (  
            TIn item, 
            CancellationToken cancellationToken 
        );


    /// <summary>
    /// Wraps a Stack of dependency-injected classes, and manages an <see cref="IAsyncEnumerable{T}"/> of completed tasks.
    /// </summary>
    /// <typeparam name="TEntry">the entry point type, from which to inject services from</typeparam>
    /// <typeparam name="TIn">type submitted to the digest function</typeparam>
    /// <typeparam name="TOut">return type of the Digest function</typeparam>
    public interface IAsyncStackWrapper<TEntry, in TIn, TOut> : IAsyncStackWrapperCore
    {
        /// <summary>
        /// <para>Called to elaborate submitted items.</para>
        /// <para>It's supposed to call a method of <typeparamref name="TEntry"/></para>
        /// </summary>
        AsyncStackDigest<TIn, TOut> StackDigest { get; }

        /// <summary>
        /// submit a new item to be elaborated
        /// </summary>
        /// <param name="item">the item to submit</param>
        void Submit ( TIn item );

        /// <summary>
        /// The loop you ca use to <c>await foreach</c> tasks in elaboration, converted to the specified type.
        /// When the pending tasks list is empty, unless <see cref="IDisposable.Dispose"/> is explocitly called
        /// this will wait indefinitively.
        /// </summary>
        /// <returns>An asyncronous enumerable of completed tasks</returns>
        IAsyncEnumerable<TOut> Elaborated ();


    }
}
