using System;
using StackInjector.Settings;

namespace StackInjector.Attributes
{
    /// <summary>
    /// Indicates this field or property should be injected.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class ServedAttribute : Attribute
    {

        /// <summary>
        /// The target version.
        /// </summary>
        public double TargetVersion { get; set; } = 0.0;

        /// <summary>
        /// <para>How the specified TargetVersion should be found.</para>
        /// <para>could be overriden by <see cref="StackWrapperSettings"/></para>
        /// </summary>
        public ServedVersionTargetingMethod? TargetingMethod { get; set; } = null;

    }

}