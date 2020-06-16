using System;
using StackInjector.Core;

namespace StackInjector.Wrappers
{
    /// <summary>
    /// Wraps a stack of auto injected classes from the specific entry point.
    /// </summary>
    /// <typeparam name="TEntry">The entry point of the stack</typeparam>
    public interface IStackWrapper<TEntry> : IStackWrapperCore, IEntryGetter<TEntry>
    {
        /// <summary>
        /// call the function you prefer on your entry point 
        /// </summary>
        /// <param name="stackDigest">the method to call on the entry instance</param>
        void Start ( Action<TEntry> stackDigest );

        /// <summary>
        /// call the function you prefer on your entry point and catch its return value
        /// </summary>
        /// <typeparam name="TOut">the return type</typeparam>
        /// <param name="stackDigest">the method to call on the entry instance</param>
        /// <returns>The object returned by the called function</returns>
        TOut Start<TOut> ( Func<TEntry, TOut> stackDigest );
    }
}
