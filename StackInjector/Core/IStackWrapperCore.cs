using System;
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

    }
}