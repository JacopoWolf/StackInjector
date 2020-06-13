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
        /// The instantiation pattern for this service.
        /// </summary>
        public InstantiationPattern Pattern { get; set; } = InstantiationPattern.Singleton;


        /// <summary>
        /// How properties and fields of this service should be served
        /// </summary>
        public ServingMethods Serving 
        { 
            get => this._serving; 
            set 
            { 
                this._servingDefined = true; 
                this._serving = value;  
            } 
        }

        private ServingMethods _serving;
        internal bool _servingDefined = false;
    }

}