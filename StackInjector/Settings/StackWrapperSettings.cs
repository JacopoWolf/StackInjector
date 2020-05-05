﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace StackInjector.Settings
{
    /// <summary>
    /// Used to manage the settings of a <see cref="StackWrapper"/>
    /// </summary>
    [Serializable]
    public sealed partial class StackWrapperSettings
    {

        #region settings

        // assemblies
        internal HashSet<Assembly>                  registredAssemblies = new HashSet<Assembly>();
        internal bool                               registerEntryPointAssembly = false;

        // versioning
        internal ServedVersionTagettingMethod       targettingMethod;
        internal bool                               overrideTargettingMethod;

        // dependency graph
        internal DependencyGraphActions             graphActions;

        #endregion


        #region constructors

        private StackWrapperSettings () { }

        /// <summary>
        /// generates a new empty <see cref="StackWrapperSettings"/>. Nothing is set.
        /// High chance a NullReference might be thrown if not treated correctly.
        /// </summary>
        /// <returns></returns>
        public static StackWrapperSettings Empty ()
        {
            return new StackWrapperSettings();
        }

        //todo add link to default settings Wiki page
        /// <summary>
        /// Creates a new StackWrapperSettings with default parameters.
        /// See what those are at 
        /// </summary>
        /// <returns></returns>
        public static StackWrapperSettings Default ()
        {
            return
                new StackWrapperSettings()
                    .RegisterEntryAssembly()
                    .VersioningMethod(ServedVersionTagettingMethod.None, overrideOnServed: false); //todo implement served override if true
        }

        #endregion

    }
}
