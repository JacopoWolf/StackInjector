using System.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Runtime.InteropServices;

namespace StackInjector.Settings
{
    /// <summary>
    /// Used to manage the settings of a <see cref="StackWrapper"/>
    /// </summary>
    [Serializable]
    public sealed class StackWrapperSettings
    {
        // settings 

        internal HashSet<Assembly> _registredAssemblies = new HashSet<Assembly>();
        internal ServedVersionTagetted _defaultTargetting;
        internal bool _createDependencyGraph;
        internal DepGraphActions GraphActions;


        /// <summary>
        /// register an assembly from wich you want classes to be laoded
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public StackWrapperSettings Register( params Assembly[] assemblies )
        {
            foreach( var assembly in assemblies )
                this._registredAssemblies.Add(assembly);
            return this;
        }

        /// <summary>
        /// Set the default targetting method
        /// </summary>
        /// <param name="targetMethod"></param>
        /// <returns></returns>
        public StackWrapperSettings VersionTargetting( ServedVersionTagetted targetMethod )
        {
            this._defaultTargetting = targetMethod;
            return this;
        }

        /// <summary>
        /// forbids two services from referencing each other
        /// </summary>
        /// <returns></returns>
        public StackWrapperSettings ForbidDependencyLoops()
        {
            this._createDependencyGraph = true;
            this.GraphActions |= DepGraphActions.avoidLoops;
            return this;
        }


        #region constructors

        private StackWrapperSettings () { }

        /// <summary>
        /// generates a new empty <see cref="StackWrapperSettings"/>. Nothing is set.
        /// </summary>
        /// <returns></returns>
        public static StackWrapperSettings Create ()
        {
            return new StackWrapperSettings();
        }

        /// <summary>
        /// Creates a new StackWrapperSettings with default parameters
        /// </summary>
        /// <returns></returns>
        public static StackWrapperSettings CreateDefault()
        {
            return
                new StackWrapperSettings()
                .VersionTargetting(ServedVersionTagetted.From);
        }

        #endregion

    }
}
