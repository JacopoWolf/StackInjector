using StackInjector.Core;
using StackInjector.Exceptions;
using StackInjector.Settings;
using StackInjector.Wrappers;
using StackInjector.Wrappers.Generic;

namespace StackInjector
{
    /// <summary>
    /// <para>Static factory class exposing methods to create new StackWrappers</para>
    /// <para>Note that using any of the exposed methods will analyze the whole target assembly,
    /// If you want to clone an existing structure, see 
    /// <see cref="Core.Cloning.ICloneableCore.CloneCore(StackWrapperSettings)"/></para>
    /// </summary>
    public static partial class Injector
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
            var core = new InjectionCore( settings )
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
                settings = StackWrapperSettings.Default;


            // create a new async stack wrapper
            var core = new InjectionCore( settings )
            {
                entryPoint = typeof(T)
            };

            var wrapper = new AsyncStackWrapper(core);

            core.ReadAssemblies();
            core.ServeAll();

            return wrapper;
        }

        /// <summary>
        /// Create a new generic asyncronous StackWrapper from the <typeparamref name="TEntry"/>
        /// entry class with the specified delegate to apply to apply as digest
        /// </summary>
        /// <typeparam name="TEntry">class from which start injection</typeparam>
        /// <typeparam name="TIn">type of elements in input</typeparam>
        /// <typeparam name="TOut">type of elements in output</typeparam>
        /// <param name="digest">delegate used to call the relative method to perform on submitted items</param>
        /// <param name="settings">the settings to use with this object. If null, use default.</param>
        /// <returns></returns>
        public static IAsyncStackWrapper<TEntry, TIn, TOut> AsyncFrom<TEntry, TIn, TOut>
            (
                AsyncStackDigest<TEntry, TIn, TOut> digest,
                StackWrapperSettings settings = null
            )
        {
            if( settings == null )
                settings = StackWrapperSettings.Default;

            // create a new generic async wrapper
            var core = new InjectionCore( settings )
            {
                entryPoint = typeof(TEntry)
            };

            var wrapper = new AsyncStackWrapper<TEntry, TIn,TOut>(core)
            {
                StackDigest = digest
            };

            core.ReadAssemblies();
            core.ServeAll();

            return wrapper;
        }


    }
}
