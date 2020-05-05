using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace StackInjector.Settings
{
    public sealed partial class StackWrapperSettings
    {
        /// <summary>
        /// register an assembly from wich you want classes to be laoded
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public StackWrapperSettings Register ( params Assembly[] assemblies )
        {
            foreach( var assembly in assemblies )
                this.registredAssemblies.Add(assembly);
            return this;
        }

        /// <summary>
        /// register the entry point assembly when Starting
        /// </summary>
        /// <returns></returns>
        public StackWrapperSettings RegisterEntryAssembly()
        {
            this.registerEntryPointAssembly = true;
            return this;
        }

        /// <summary>
        /// Overrides default targetting method
        /// </summary>
        /// <param name="targetMethod">the new default targetting method</param>
        /// <param name="overrideOnServed">if true, versioning methods for [Served] fields and properties are overriden</param>
        /// <returns></returns>
        public StackWrapperSettings VersioningMethod ( ServedVersionTagettingMethod targetMethod , bool overrideOnServed = false )
        {
            this.targettingMethod = targetMethod;
            this.overrideTargettingMethod = overrideOnServed;
            return this;
        }
    }
}
