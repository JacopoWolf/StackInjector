namespace StackInjector.Settings
{
    /// <summary>
    /// Type of version targetting. If the condition is unsatisfied, throw an exception and consider the injection failed
    /// </summary>
    public enum ServedVersionTargetingMethod
    {
        /// <summary>
        /// Ignore versioning. Target the first implementation available.
        /// Possibly undeterministic if multiple implementation of the interface exist.
        /// </summary>
        None = -1,

        /// <summary>
        /// target the exact version of a service.
        /// Throws an exception if it doesn't exist.
        /// </summary>
        Exact,

        /// <summary>
        /// Target the latest major version available.
        /// </summary>
        LatestMajor,

        /// <summary>
        /// Target the latest minor version from the specified major version
        /// </summary>
        LatestMinor
    }

}
