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
    public sealed partial class StackWrapperSettings
    {

        // settings 
        internal HashSet<Assembly>                  registredAssemblies = new HashSet<Assembly>();
        internal ServedVersionTagettingMethod       targettingMethod;
        internal DependencyGraphActions             graphActions;



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

        //todo add link to default settings Wiki page
        /// <summary>
        /// Creates a new StackWrapperSettings with default parameters.
        /// See what those are at 
        /// </summary>
        /// <returns></returns>
        public static StackWrapperSettings CreateDefault()
        {
            return
                new StackWrapperSettings()
                    .VersioningMethod(ServedVersionTagettingMethod.From);
        }

        #endregion

    }
}
