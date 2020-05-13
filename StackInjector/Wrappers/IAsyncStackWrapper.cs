using System;
using System.Collections.Generic;
using System.Threading;
using StackInjector.Core;

namespace StackInjector.Wrappers
{
    /// <summary>
    /// Wraps a Stack of dependency-injected classes, and manages an <see cref="IAsyncEnumerable{T}"/> of completed tasks.
    /// </summary>
    public interface IAsyncStackWrapper : IAsyncStackWrapperCore<object>
    {

        /// <summary>
        /// Submit a new object to be elaborated asyncronously in this stack
        /// </summary>
        /// <param name="submitted">The object to elaborate</param>
        void Submit ( object submitted );

    }
}