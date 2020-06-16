namespace StackInjector.Wrappers
{
    /// <summary>
    /// Common wrappers interface with methods to get the entry point
    /// </summary>
    /// <typeparam name="TEntry"></typeparam>
    public interface IEntryGetter<TEntry>
    {
        /// <summary>
        /// Returns the instance of the entry point of this wrapper.
        /// </summary>
        TEntry Entry { get; }
    }
}
