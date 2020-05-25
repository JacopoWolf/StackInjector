using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using StackInjector.Wrappers;

namespace StackInjector.Settings
{
    /// <summary>
    /// Used to manage the behaviour of <see cref="IStackWrapper"/> and <see cref="IAsyncStackWrapper"/>
    /// </summary>
    [Serializable]
    public sealed partial class StackWrapperSettings
    {

        #region settings

        // assemblies
        internal HashSet<Assembly>                  registredAssemblies                     = new HashSet<Assembly>();
        internal bool                               registerEntryPointAssembly              = false;
        internal bool                               registerSelf                            = false;

        // versioning
        internal ServedVersionTargetingMethod       targetingMethod                         = ServedVersionTargetingMethod.None;
        internal bool                               overrideTargetingMethod                 = false;

        // disposing
        internal bool                               trackInstancesDiff                      = false;
        internal bool                               callDisposeOnInstanceDiff               = false;

        // async management
        internal AsyncWaitingMethod                 asyncWaitingMethod                      = AsyncWaitingMethod.Exit;
        internal int                                asyncWaitTime                           = 500;

        // features
        internal bool                               serveEnumerables                        = false;

        #endregion


        private StackWrapperSettings () { }


        /// <summary>
        /// creates a deep copy of this settings object
        /// </summary>
        /// <returns></returns>
        public StackWrapperSettings Copy ()
        {
            var settingsCopy = (StackWrapperSettings)this.MemberwiseClone();
            // creats a deep copy of reference objects
            settingsCopy.registredAssemblies = this.registredAssemblies.ToHashSet();

            return settingsCopy;
        }



        /// <summary>
        /// generates a new <see cref="StackWrapperSettings"/> with everything set to false
        /// </summary>
        /// <returns>empty settings</returns>
        public static StackWrapperSettings Empty
            =>
                new StackWrapperSettings();


        /// <summary>
        /// Creates a new StackWrapperSettings with default parameters.
        /// See what those are at <see href="https://github.com/JacopoWolf/StackInjector/wiki/Default-Settings">the Wiki page</see>
        /// </summary>
        /// <returns>the default settings</returns>
        public static StackWrapperSettings Default
            =>
                new StackWrapperSettings()
                    .RegisterEntryAssembly()
                    .RegisterWrapperAsService()
                    .TrackInstantiationDiff(false)
                    .VersioningMethod(ServedVersionTargetingMethod.None, @override: false)
                    .WhenNoMoreTasks(AsyncWaitingMethod.Wait)
                    .ServeIEnumerables();

    }
}