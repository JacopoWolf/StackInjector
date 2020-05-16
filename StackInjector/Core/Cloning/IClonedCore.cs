using StackInjector.Wrappers;
using StackInjector.Wrappers.Generic;

namespace StackInjector.Core.Cloning
{
    /// <summary>
    /// A cloned structure of a wrapper.
    /// </summary>
    public interface IClonedCore
    {

        /// <summary>
        /// convert this to an <see cref="IStackWrapper"/>
        /// </summary>
        /// <typeparam name="T">entry point of the new wrapper</typeparam>
        /// <returns>the new wrapper</returns>
        IStackWrapper ToWrapper<T> () where T : IStackEntryPoint;


        /// <summary>
        /// convert this to an <see cref="IAsyncStackWrapper"/>
        /// </summary>
        /// <typeparam name="T">entry point of the new wrapper</typeparam>
        /// <returns>the new wrapper</returns>
        IAsyncStackWrapper ToAsyncWrapper<T> () where T : IAsyncStackEntryPoint;


        /// <summary>
        /// convert this to an <see cref="IAsyncStackWrapper{TEntry, TIn, TOut}"/>
        /// </summary>
        /// <typeparam name="TEntry">entry instantiation poin of the new wrapper</typeparam>
        /// <typeparam name="TIn">type of input elements</typeparam>
        /// <typeparam name="TOut">type of output elements</typeparam>
        /// <param name="digest">action to perform on elements</param>
        /// <returns>the new wrapper</returns>
        IAsyncStackWrapper<TEntry, TIn, TOut> ToGenericAsync<TEntry, TIn, TOut> ( AsyncStackDigest<TEntry, TIn, TOut> digest );

    }

}
