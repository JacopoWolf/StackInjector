using System.Collections.Generic;
using System.Reflection;

namespace StackInjector.Settings
{
    public sealed partial class StackWrapperSettings
    {

        #region Assembly registration

        /// <summary>
        /// register an external assembly from wich you want classes to be laoded
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns>the modified settings</returns>
        public StackWrapperSettings RegisterAssemblies ( params Assembly[] assemblies )
        {
            foreach( var assembly in assemblies )
                this.registredAssemblies.Add(assembly);
            return this;
        }

        /// <summary>
        /// Register the assembly of the specified type.
        /// Same as <see cref="RegisterAssemblies(Assembly[])"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>the modified settings</returns>
        public StackWrapperSettings RegisterAssemblyOf<T> ()
        {
            this.registredAssemblies.Add(typeof(T).Assembly);
            return this;
        }

        /// <summary>
        /// <para>register the entry point assembly when Starting.</para>
        /// <para>If set, there is no need to specify the entry assembly in <see cref="RegisterAssemblyOf{T}"/></para>
        /// <para>Default is true.</para>
        /// </summary>
        /// <returns>the modified settings</returns>
        public StackWrapperSettings RegisterEntryAssembly ( bool register = true )
        {
            this.registerEntryPointAssembly = register;
            return this;
        }

        /// <summary>
        /// Register the wrapper as a service, so it can be accessed in contained classes.
        /// Default is true.
        /// </summary>
        /// <returns>the modified settings</returns>
        public StackWrapperSettings RegisterWrapperAsService ( bool register = true )
        {
            this.registerSelf = register;
            return this;
        }

        #endregion


        /// <summary>
        /// Track every new instantiated class to be deleted upon Dispose.
        /// </summary>
        /// <param name="track">if true, track instances diff</param>
        /// <param name="callDispose">if true, call Dispose on services implementing <see cref="System.IDisposable"/></param>
        /// <returns>the modified settings</returns>
        public StackWrapperSettings TrackInstantiationDiff ( bool track = true, bool callDispose = true )
        {
            this.trackInstancesDiff = track;
            this.callDisposeOnInstanceDiff = callDispose;
            return this;
        }

        /// <summary>
        /// Overrides default targetting method
        /// </summary>
        /// <param name="targetMethod">the new default targetting method</param>
        /// <param name="override">if true, versioning methods for [Served] fields and properties are overriden</param>
        /// <returns>the modified settings</returns>
        public StackWrapperSettings VersioningMethod ( ServedVersionTargetingMethod targetMethod, bool @override = false )
        {
            this.targetingMethod = targetMethod;
            this.overrideTargetingMethod = @override;
            return this;
        }

        /// <summary>
        /// Allows <see cref="IEnumerable{T}"/> to be injected with a list of every service implementing T
        /// </summary>
        /// <param name="serve">if true, serve</param>
        /// <returns>the modified settings</returns>
        public StackWrapperSettings ServeIEnumerables ( bool serve = true )
        {
            this.serveEnumerables = serve;
            return this;
        }


        #region Asynchronous settings

        /// <summary>
        /// What to do when an <see cref="Wrappers.IAsyncStackWrapper"/> 
        /// has no more pending tasks to execute
        /// </summary>
        /// <param name="waitingMethod">the new waiting method</param>
        /// <param name="waitTime">if <see cref="AsyncWaitingMethod.Timeout"/> is set, this will be max time to wait</param>
        /// <returns>the modified settings</returns>
        public StackWrapperSettings WhenNoMoreTasks ( AsyncWaitingMethod waitingMethod, int waitTime = 1000 )
        {
            this.asyncWaitingMethod = waitingMethod;
            this.asyncWaitTime = waitTime;
            return this;
        }

        #endregion


    }
}
