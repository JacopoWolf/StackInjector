﻿using System.Threading;
using System.Threading.Tasks;

namespace StackInjector
{
    /// <summary>
    /// Entry point for asyncronous StacksWrappers
    /// </summary>
    public interface IAsyncStackEntryPoint
    {

        /// <summary>
        /// Elaborate asyncronouslt the specified item. The Task should be started in this method.
        /// A cancellation token is also given to allow safe disposing of <see cref="IAsyncStackWrapper"/>
        /// </summary>
        /// <param name="item">The item to elaborate. Casting is necessary.</param>
        /// <param name="cancellationToken">used to cancel </param>
        /// <returns>A task rappresenting the execution state of this item</returns>
        Task<object> Digest ( object item, CancellationToken cancellationToken );

    }
}
