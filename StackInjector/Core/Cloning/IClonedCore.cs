using StackInjector.Wrappers;

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

    }

}
