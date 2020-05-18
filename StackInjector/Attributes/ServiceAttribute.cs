using System;
using StackInjector.Settings;

namespace StackInjector.Attributes
{
    /// <summary>
    /// Indicates this class can be used as a Service
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class ServiceAttribute : Attribute
    {
        /// <summary>
        /// The version of this service implementation
        /// </summary>
        public double Version { get; set; } = -0.0;

        /// <summary>
        /// How properties and fields of this service should be served
        /// </summary>
        public ServingMethods Serving { get; set; } = Injector.Defaults.ServingMethod;
    }

}