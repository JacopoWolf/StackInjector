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
        internal HashSet<Assembly>                  registredAssemblies = new HashSet<Assembly>();
        internal bool                               registerEntryPointAssembly;
        internal bool                               registerSelf;

        // versioning
        internal ServedVersionTargetingMethod       targetingMethod;
        internal bool                               overrideTargetingMethod;

        // disposing
        internal bool                               trackInstancesDiff;
        internal bool                               callDisposeOnInstanceDiff;

        // async management
        internal AsyncWaitingMethod                 asyncWaitingMethod;
        internal int                                asyncWaitTime;


        #endregion

        private StackWrapperSettings () { }

        /// <summary>
        /// generates a new empty <see cref="StackWrapperSettings"/>. Nothing is set.
        /// High chance a NullReference might be thrown if not treated correctly.
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
