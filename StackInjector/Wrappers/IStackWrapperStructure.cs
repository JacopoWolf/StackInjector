using System;
using StackInjector.Settings;

namespace StackInjector.Wrappers
{

    /// <summary>
    /// used to allow cloning of StackWrappers structures
    /// </summary>
    public interface IStackWrapperStructure : IDisposable
    {

        /// <summary>
        /// the settings of this stackwrapper
        /// </summary>
        ref readonly StackWrapperSettings Settings { get; }


        //todo modify those signatures for generics. Might help.

        /// <summary>
        /// Copy the structure of this existing wrapper and initialize a new one.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="overrideSettings">if set, overrides this object's settings</param>
        /// <returns></returns>
        IStackWrapper FromStructure<T> ( StackWrapperSettings overrideSettings = null ) where T : IStackEntryPoint;

        /// <summary>
        /// Copy the structure of this existing wrapper and initialize a new Asyncronous wrapper.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="overrideSettings">if set, overrides this object's settings</param>
        /// <returns></returns>
        IAsyncStackWrapper AsyncFromStructure<T> ( StackWrapperSettings overrideSettings = null ) where T : IAsyncStackEntryPoint;

    }
}
