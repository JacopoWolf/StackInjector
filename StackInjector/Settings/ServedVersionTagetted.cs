namespace StackInjector.Settings
{
    /// <summary>
    /// Type of version targetting. If the condition is unsatisfied, throw an exception and consider the injection failed
    /// </summary>
    public enum ServedVersionTagettingMethod
    {
        /// <summary>
        /// target the exact version of a service
        /// </summary>
        Exact,

        /// <summary>
        /// target a version equal or greater of the specified service, prioritizing the closest one.
        /// </summary>
        From,

        /// <summary>
        /// target the latest version available
        /// </summary>
        Latest,

        /// <summary>
        /// target the first version available
        /// </summary>
        First
    }

}
