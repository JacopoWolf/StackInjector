using System.Reflection;

namespace StackInjector.Settings
{
    public sealed partial class StackWrapperSettings
    {
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
        /// Same as <see cref="RegisterAssemblies(Assembly[])"/> but withouth iteration.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>the modified settings</returns>
        public StackWrapperSettings RegisterAssemblyOf<T> ()
        {
            this.registredAssemblies.Add(typeof(T).Assembly);
            return this;
        }

        /// <summary>
        /// register the entry point assembly when Starting.
        /// Default is true.
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

        /// <summary>
        /// Track every new instantiated class to be deleted upon Dispose.
        /// </summary>
        /// <param name="track">if true, track instances diff</param>
        /// <param name="callDispose">if true, call Dispose on services implementing <see cref="System.IDisposable"/></param>
        /// <returns>the modified settings</returns>
        public StackWrapperSettings TrackInstantiationDiff( bool track = true, bool callDispose = true )
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
    }
}
