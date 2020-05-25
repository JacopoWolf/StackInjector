using StackInjector.Settings;

namespace StackInjector.Core.Cloning
{

    /// <summary>
    /// Allows for a wrapper to have its core cloned.
    /// </summary>
    public interface ICloneableCore
    {

        /// <summary>
        /// Clone the core of this wrapper and <b>SHARE</b> common resources, copying and using the already existing structure
        /// and making instantiation of objects faster.
        /// </summary>
        /// <param name="settings">if set, overrides the previus core settings.</param>
        /// <returns>A generic object allowing conversion of the cloned core</returns>
        IClonedCore CloneCore ( StackWrapperSettings settings = null );


        /// <summary>
        /// Make a true clone of the core of this wrapper, wich will behave completely independently from
        /// the original one, <b>NOT SHARING</b> any resource. Takes up more memory.
        /// </summary>
        /// <param name="settings">if set, overrides the previus core settings.</param>
        /// <returns>A generic object allowing conversion of the cloned core</returns>
        IClonedCore DeepCloneCore ( StackWrapperSettings settings = null );


    }

}
