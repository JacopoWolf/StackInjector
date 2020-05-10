using StackInjector.Behaviours;
using StackInjector.Exceptions;
using StackInjector.Settings;

namespace StackInjector
{
    /// <summary>
    /// <para>Static factory class exposing methods to create new StackWrappers</para>
    /// <para>Note that using any of the exposed methods will analyze the whole target assembly,
    /// If you want to clone an existing structure, see 
    /// <see cref="IStackWrapperStructure.FromStructure{T}(StackWrapperSettings)"/> and
    /// <see cref="IStackWrapperStructure.AsyncFromStructure{T}(StackWrapperSettings)"/></para>
    /// </summary>
    public static class Injector
    {
        //todo SingleInstanceHolder should depend upon a setting

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
                ServicesWithInstances = new SingleInstanceHolder()
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
                    .From<T>();
        }


        /// <summary>
        /// Create a new asyncronous StackWrapper from the <typeparamref name="T"/> entry point
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static IAsyncStackWrapper AsyncFrom<T> ( this StackWrapperSettings settings ) where T : IAsyncStackEntryPoint
        {
            // create a new async stack wrapper
            var wrapper = new AsyncStackWrapper( settings )
            {
                EntryPoint = typeof(T),
                ServicesWithInstances = new SingleInstanceHolder()
            };

            wrapper.ReadAssemblies();
            wrapper.ServeAll();

            return wrapper;
        }


        /// <summary>
        /// Create a new asyncronous StackWrapper from the <typeparamref name="T"/> entry point
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IAsyncStackWrapper AsyncFrom<T> ()
            where T : IAsyncStackEntryPoint
        {
            return
                StackWrapperSettings
                .Default()
                .AsyncFrom<T>();
        }


    }
}
