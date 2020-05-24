using System;
using System.Collections.Generic;
using StackInjector.Core.Cloning;
using StackInjector.Settings;

namespace StackInjector.Core
{

    /// <summary>
    /// base interface for all stackwrappers
    /// </summary>
    public interface IStackWrapperCore : IDisposable, ICloneableCore
    {

        /// <summary>
        /// the settings of this stackwrapper
        /// </summary>
        ref readonly StackWrapperSettings Settings { get; }


        /// <summary>
        /// Find every service valid for the given class or interface 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> GetServices<T> ();

    }
}