using StackInjector.Behaviours;
using StackInjector.Core;
using StackInjector.Exceptions;
using StackInjector.Settings;
using StackInjector.Wrappers;

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
        public static IStackWrapper From<T> ( StackWrapperSettings settings = null ) where T : IStackEntryPoint
        {
            if( settings == null )
                settings = StackWrapperSettings.Default;

            // create a new stackwrapper with the specified settings
            var core = new WrapperCore( settings )
            {
                entryPoint = typeof(T)
            };

            // putting this here allows for registration of this object when serving
            var wrapper = new StackWrapper(core);

            core.ReadAssemblies();
            core.ServeAll();


            return wrapper;
        }



        /// <summary>
        /// Create a new asyncronous StackWrapper from the <typeparamref name="T"/> entry point
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static IAsyncStackWrapper AsyncFrom<T> ( StackWrapperSettings settings = null ) where T : IAsyncStackEntryPoint
        {
            if( settings == null )
            {
                settings = StackWrapperSettings.Default;
            }

            // create a new async stack wrapper
            var core = new WrapperCore( settings )
            {
                entryPoint = typeof(T),
                instances = new SingleInstanceHolder()
            };

            var wrapper = new AsyncStackWrapper(core);

            core.ReadAssemblies();
            core.ServeAll();

            return wrapper;
        }


    }
}
