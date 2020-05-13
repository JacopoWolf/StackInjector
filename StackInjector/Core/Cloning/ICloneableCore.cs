using System;
using System.Collections.Generic;
using System.Text;
using StackInjector.Settings;

namespace StackInjector.Core.Cloning
{

    /// <summary>
    /// Allows for a wrapper to have its core cloned.
    /// </summary>
    public interface ICloneableCore
    {

        /// <summary>
        /// Clone the core of this wrapper, copying the already existing structure
        /// and making instantiation of objects faster.
        /// </summary>
        /// <param name="settings">if set, overrides the previus core settings.</param>
        /// <returns>A generic object allowing conversion of the cloned core</returns>
        IClonedCore CloneCore ( StackWrapperSettings settings = null );

    }

}
