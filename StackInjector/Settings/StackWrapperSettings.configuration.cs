using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace StackInjector.Settings
{
    public sealed partial class StackWrapperSettings
    {

        #region registration

        /// <summary>
        /// Register an external assembly from wich you want classes to be laoded.
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns>the modified settings</returns>
        public StackWrapperSettings RegisterAssemblies ( params Assembly[] assemblies )
        {
            foreach( var assembly in assemblies )
                this._registredAssemblies.Add(assembly);
            return this;
        }

        /// <summary>
        /// Automatically register all domain assemblies, filtering the specified ones.<br/>
        /// <b>Warning: CPU expensive</b>
        /// </summary>
        /// <param name="regexFilter">a regex string used to filter unwanted matching assemblies</param>
        /// <returns>the modified settings</returns>
        public StackWrapperSettings RegisterDomain ( string regexFilter = Injector.Defaults.AssemblyRegexFilter )
        {
            this.RegisterAssemblies
                (
                    AppDomain.CurrentDomain
                        .GetAssemblies()
                        .Where(a => !Regex.IsMatch(a.FullName, regexFilter))
                        .ToArray()
                );

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
            this.RegisterAssemblies(typeof(T).Assembly);
            return this;
        }

        /// <summary>
        /// <para>Register the entry point assembly when starting.</para>
        /// <para>If set, there is no need to specify the entry assembly in <see cref="RegisterAssemblyOf{T}"/>.</para>
        /// <para>Default is true.</para>
        /// </summary>
        /// <returns>the modified settings</returns>
        public StackWrapperSettings RegisterEntryAssembly ( bool register = true )
        {
            this._registerEntryPointAssembly = register;
            return this;
        }

        /// <summary>
        /// Register the wrapper as a service, so it can be accessed in contained classes.
        /// Default is true.
        /// </summary>
        /// <returns>the modified settings</returns>
        public StackWrapperSettings RegisterWrapperAsService ( bool register = true )
        {
            this._registerWrapperAsService = register;
            return this;
        }

        #endregion


        #region disposing

        /// <summary>
        /// Track every new instantiated class to be deleted upon Dispose.
        /// </summary>
        /// <param name="track">if true, track instances diff</param>
        /// <param name="callDispose">if true, call Dispose on services implementing <see cref="System.IDisposable"/></param>
        /// <returns>the modified settings</returns>
        public StackWrapperSettings TrackInstantiationDiff ( bool track = true, bool callDispose = true )
        {
            this._trackInstancesDiff = track;
            this._callDisposeOnInstanceDiff = callDispose;
            return this;
        }

        #endregion

        #region async

        /// <summary>
        /// What to do when an <see cref="Wrappers.IAsyncStackWrapper{TEntry, TIn, TOut}"/> 
        /// has no more pending tasks to execute
        /// </summary>
        /// <param name="waitingMethod">the new waiting method</param>
        /// <param name="waitTime">if <see cref="AsyncWaitingMethod.Timeout"/> is set, this will be max time to wait</param>
        /// <returns>the modified settings</returns>
        public StackWrapperSettings WhenNoMoreTasks ( AsyncWaitingMethod waitingMethod, int waitTime = 1000 )
        {
            this._asyncWaitingMethod = waitingMethod;
            this._asyncWaitTime = waitTime;
            return this;
        }

        #endregion


        #region injection

        /// <summary>
        /// Overrides default targetting method
        /// </summary>
        /// <param name="targetMethod">the new default targetting method</param>
        /// <param name="override">if true, versioning methods for [Served] fields and properties are overriden</param>
        /// <returns>the modified settings</returns>
        public StackWrapperSettings InjectionVersioningMethod ( ServedVersionTargetingMethod targetMethod, bool @override = false )
        {
            this._targetingMethod = targetMethod;
            this._overrideTargetingMethod = @override;
            return this;
        }

        /// <summary>
        /// Overrides default serving method
        /// </summary>
        /// <param name="methods">the new default serving method for all services</param>
        /// <param name="override">if true, serving methods for [Service] calsses are overridden with the specified one</param>
        /// <returns>the modified settings</returns>
        public StackWrapperSettings InjectionServingMethods ( ServingMethods methods, bool @override = false )
        {
            this._servingMethod = methods;
            this._overrideServingMethod = @override;
            return this;
        }

        #endregion



        #region features 

        /// <summary>
        /// Allows <see cref="IEnumerable{T}"/> to be injected with a list of every service implementing T
        /// </summary>
        /// <param name="serve">if true, serve</param>
        /// <returns>the modified settings</returns>
        public StackWrapperSettings ServeIEnumerables ( bool serve = true )
        {
            this._serveEnumerables = serve;
            return this;
        }

        #endregion

    }
}
