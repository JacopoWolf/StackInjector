namespace StackInjector
{
    /// <summary>
    /// Wraps a Stack of dependency-injected classes.
    /// </summary>
    public interface IStackWrapper : IStackWrapperStructure
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