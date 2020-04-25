using System;
using System.Collections.Generic;
using System.Text;
using StackInjector.Settings;

namespace StackInjector.Attributes
{
    /// <summary>
    /// Indicates this field or property should be injected
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple =false, Inherited = true)]
    public sealed class ServedAttribute : Attribute
    {
        /// <summary>
        /// the target version
        /// </summary>
        public Version Version { get; set; } = Version.Parse("0.0.0");

        /// <summary>
        /// Target version of the service
        /// </summary>
        public ServedVersionTagetted Target { get; set; } = ServedVersionTagetted.From;

    }

}
