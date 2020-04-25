using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using StackInjector.Settings;

namespace StackInjector
{
    /// <summary>
    /// Static class exposing methods to start 
    /// </summary>
    public static class Injector
    {

        public static StackWrapper With<T> ( this StackWrapperSettings settings ) where T : IStackEntryPoint
        {
            // create a new stackwrapper with the specified settings
            var wrapper = new StackWrapper( settings );

            wrapper.ReadAssemblies();
            wrapper.InstantiateAndInjectAll();

            return wrapper;
        }


        public static StackWrapper From<T> () where T : IStackEntryPoint
        {
            return  
                StackWrapperSettings
                    .CreateDefault()
                    .Register(typeof(T).Assembly)
                    .With<T>();
        }

    }
}
