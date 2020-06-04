﻿using StackInjector.Core;

namespace StackInjector.Wrappers
{
    /// <summary>
    /// Wraps a Stack of dependency-injected classes
    /// </summary>
    [System.Obsolete("This interface and its implementation will be deprecated in a future release. Use the generic option instead.", false)]
    public interface IStackWrapper : IStackWrapperCore
    {

        /// <summary>
        /// Start this StackWrapper with the specified entry point and get it's returned object in a generic form
        /// </summary>
        object Start ();

        /// <summary>
        /// Start this StackWrapper with the specified entry point and get it's return type converted to the specified type.
        /// Throws an error on a wrongful conversion
        /// </summary>
        T Start<T> ();

    }
}