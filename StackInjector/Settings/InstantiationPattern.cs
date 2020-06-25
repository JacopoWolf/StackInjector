namespace StackInjector.Settings
{

    /// <summary>
    /// Specify the instantiation pattern for a given service.
    /// </summary>
    public enum InstantiationPattern
    {

        /// <summary>
        /// <para>Create and use a single instance of a specified type.</para>
        /// <para>Allows instantiation diff and tracking of created instances.</para>
        /// </summary>
        Singleton,

        /// <summary>
        /// <para>always create a new instance id you encounter it.</para>
        /// <para>It is <b>not</b> possible to track instantiated classes.</para>
        /// </summary>
        AlwaysCreate


    }
}