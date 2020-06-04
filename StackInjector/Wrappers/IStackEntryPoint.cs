namespace StackInjector.Wrappers
{
    /// <summary>
    /// Defines an entry point for a stack
    /// </summary>
    [System.Obsolete("The wrapper for this entry point will be deprecated in a future relase. Use the generic option instead.", false)]
    public interface IStackEntryPoint
    {
        /// <summary>
        /// This will be called once the stack is initialized with <see cref="IStackWrapper.Start"/>
        /// </summary>
        object EntryPoint ();
    }
}
