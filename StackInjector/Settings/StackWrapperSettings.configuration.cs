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
        /// Set the default targetting method
        /// </summary>
        /// <param name="targetMethod"></param>
        /// <returns></returns>
        public StackWrapperSettings VersioningMethod ( ServedVersionTagettingMethod targetMethod )
        {
            this.targettingMethod = targetMethod;
            return this;
        }
    }
}
