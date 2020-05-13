namespace StackInjector.Wrappers
{
    /// <summary>
    /// Defines an entry point for a stack
    /// </summary>
    public interface IStackEntryPoint
    {
        /// <summary>
        /// This will be called once the stack is initialized with <see cref="IStackWrapper.Start"/>
        /// </summary>
        object EntryPoint ();
    }
}
