using System;
using StackInjector.Core.Cloning;
using StackInjector.Settings;

namespace StackInjector.Wrappers
{

    /// <summary>
    /// used to allow cloning of StackWrappers structures
    /// </summary>
    public interface IStackWrapperCore : IDisposable, ICloneableCore
    {

        /// <summary>
        /// the settings of this stackwrapper
        /// </summary>
        ref readonly StackWrapperSettings Settings { get; }

    }
}