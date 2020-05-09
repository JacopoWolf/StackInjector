using System.Reflection;

namespace StackInjector.Settings
{
    public sealed partial class StackWrapperSettings
    {
        /// <summary>
        /// register an external assembly from wich you want classes to be laoded
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
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
        /// <returns></returns>
        public StackWrapperSettings RegisterAssemblyOf<T> ()
        {
            this.registredAssemblies.Add(typeof(T).Assembly);
            return this;
        }

        /// <summary>
        /// register the entry point assembly when Starting.
        /// Default is true.
        /// </summary>
        /// <returns></returns>
        public StackWrapperSettings RegisterEntryAssembly ( bool register = true )
        {
            this.registerEntryPointAssembly = register;
            return this;
        }

        /// <summary>
        /// Register the wrapper as a service, so it can be accessed in contained classes.
        /// Default is true.
        /// </summary>
        /// <returns></returns>
        public StackWrapperSettings RegisterWrapperAsService ( bool register = true )
        {
            this.registerSelf = register;
            return this;
        }

        /// <summary>
        /// Overrides default targetting method
        /// </summary>
        /// <param name="targetMethod">the new default targetting method</param>
        /// <param name="override">if true, versioning methods for [Served] fields and properties are overriden</param>
        /// <returns></returns>
        public StackWrapperSettings VersioningMethod ( ServedVersionTagetingMethod targetMethod, bool @override = false )
        {
            this.targettingMethod = targetMethod;
            this.overrideTargettingMethod = @override;
            return this;
        }
    }
}
