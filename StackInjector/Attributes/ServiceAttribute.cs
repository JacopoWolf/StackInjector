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
        /// Indicates if the framework should use only one instance of this service across all clients.
        /// Might be ignored by the settings.
        /// </summary>
        public bool ReuseInstance { get; set; } = false;

        /// <summary>
        /// how properties and fields of this service should be served
        /// </summary>
        public ServingMethods Serving { get; set; } = Injector.Defaults.ServingMethod;
    }

}
