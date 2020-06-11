﻿using StackInjector.Core;
using StackInjector.Exceptions;
using StackInjector.Settings;
using StackInjector.Wrappers;

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
        /// Create a new <see cref="StackWrapper{TEntry}"/> from the <typeparamref name="T"/> entry point with the specified settings
        /// </summary>
        /// <typeparam name="T">The type of the entry point</typeparam>
        /// <param name="settings">settings for this StackWrapper</param>
        /// <returns>The Initialized StackWrapper</returns>
        /// <exception cref="ClassNotFoundException"></exception>
        /// <exception cref="NotAServiceException"></exception>
        /// <exception cref="ImplementationNotFoundException"></exception>
        public static IStackWrapper<T> From<T> ( StackWrapperSettings settings = null )
        {
            if( settings == null )
                settings = StackWrapperSettings.Default;

            // create the core and wrap it
            var core = new InjectionCore( settings )
                {
                    entryPoint = typeof(T)
                };

            var wrapper = new StackWrapper<T>(core);

            // initialize the injection process
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

            // create the core and wrap it
            var core = new InjectionCore( settings )
                {
                    entryPoint = typeof(TEntry)
                };

            var wrapper = new AsyncStackWrapper<TEntry, TIn,TOut>(core)
                {
                    StackDigest = digest
                };

            // initialize the injection process
            core.ReadAssemblies();
            core.ServeAll();

            return wrapper;
        }

    }
}