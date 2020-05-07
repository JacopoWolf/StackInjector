using StackInjector.Behaviours;
using StackInjector.Exceptions;
using StackInjector.Settings;

namespace StackInjector
{
    /// <summary>
    /// Static class exposing methods to create a new StackWrapper
    /// </summary>
    public static class Injector
    {
        /// <summary>
        /// Create a new StackWrapper from the <typeparamref name="T"/> entry point with the specified settings
        /// </summary>
        /// <typeparam name="T">The type of the entry point</typeparam>
        /// <param name="settings">settings for this StackWrapper</param>
        /// <returns>The Initialized StackWrapper</returns>
        /// <exception cref="ClassNotFoundException"></exception>
        /// <exception cref="NotAServiceException"></exception>
        /// <exception cref="ImplementationNotFoundException"></exception>
        public static IStackWrapper From<T> ( this StackWrapperSettings settings ) where T : IStackEntryPoint
        {

            // create a new stackwrapper with the specified settings
            var wrapper = new StackWrapper( settings )
            {
                EntryPoint = typeof(T),
                ServicesWithInstances = new SingleInstanceHolder() //todo assign this decently
            };

            wrapper.ReadAssemblies();
            wrapper.ServeAll();


            return wrapper;
        }

        /// <summary>
        /// Create a new StackWrapper from the <typeparamref name="T"/> entry point
        /// </summary>
        /// <typeparam name="T">The type of the entry point</typeparam>
        /// <returns>The initialized StackWrapper</returns>
        /// <exception cref="ClassNotFoundException"></exception>
        /// <exception cref="NotAServiceException"></exception>
        /// <exception cref="ImplementationNotFoundException"></exception>
        public static IStackWrapper From<T> () where T : IStackEntryPoint
        {
            // default configuration
            return
                StackWrapperSettings
                    .Default()
                    .RegisterAssemblies(typeof(T).Assembly)
                    .From<T>();
        }

    }
}
