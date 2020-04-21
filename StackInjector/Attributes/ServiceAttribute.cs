using System;
using System.Collections.Generic;
using System.Text;

namespace StackInjector.Attributes
{
    /// <summary>
    /// Indicates this class can be used as a Service
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class ServiceAttribute : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236


        /// <summary>
        /// Indicates if the framework should use only one instance of this service across all clients
        /// </summary>
        public bool Unique { get; set; } = false;

        /// <summary>
        /// The version of this specific service
        /// </summary>
        public Version Version { get; set; } = Version.Parse("1.0.0");

    }
}
