using System;
using System.Collections.Generic;
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
        internal bool                               serveEnumerables                        = false; //todo add this to wiki's default settings

        #endregion


        private StackWrapperSettings () { }


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
                    .WhenNoMoreTasks(AsyncWaitingMethod.Wait);


        //? maybe add a DefaultInner for nested wrappers

    }
}