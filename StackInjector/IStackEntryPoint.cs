namespace StackInjector
{
    /// <summary>
    /// Entry point class for a simple stack
    /// </summary>
    public interface IStackEntryPoint
    {
        /// <summary>
        /// This will be called once the stack is initialized
        /// </summary>
        object EntryPoint ();
    }
}
