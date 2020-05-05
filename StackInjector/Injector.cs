using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
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
        public static StackWrapper From<T> ( this StackWrapperSettings settings ) where T : IStackEntryPoint
        {
            // create a new stackwrapper with the specified settings
            var wrapper = new StackWrapper( settings )
            {
                EntryPoint = typeof(T)
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
        public static StackWrapper From<T> () where T : IStackEntryPoint
        {
            // default configuration
            return  
                StackWrapperSettings
                    .Default()
                    .Register(typeof(T).Assembly)
                    .From<T>();
        }

    }
}
