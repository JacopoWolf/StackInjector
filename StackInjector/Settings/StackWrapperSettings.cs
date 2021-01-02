using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using StackInjector.Wrappers;

namespace StackInjector.Settings
{
	/// <summary>
	/// Used to manage the behaviour of <see cref="IStackWrapper{TEntry}"/> and <see cref="IAsyncStackWrapper{TEntry, TIn, TOut}"/>
	/// </summary>
	[Serializable]
	public sealed partial class StackWrapperSettings
	{

		// list of settings and their initial empty definition
		#region settings

		// registration
		internal HashSet<Assembly>                  _registredAssemblies                    = new HashSet<Assembly>();
		internal bool                               _registerEntryPointAssembly;
		internal bool                               _registerWrapperAsService;
		internal bool                               _registerAfterCloning;

		// disposing
		internal bool                               _trackInstancesDiff;
		internal bool                               _callDisposeOnInstanceDiff;

		// async management
		internal AsyncWaitingMethod                 _asyncWaitingMethod                     = AsyncWaitingMethod.Exit;
		internal int                                _asyncWaitTime                          = 500;

		// injection
		internal ServedVersionTargetingMethod       _targetingMethod                        = ServedVersionTargetingMethod.None;
		internal bool                               _overrideTargetingMethod;

		internal ServingMethods                     _servingMethod                          = ServingMethods.DoNotServe;
		internal bool                               _overrideServingMethod;
		internal bool                               _cleanUnusedTypesAftInj;

		// features
		internal bool                               _serveEnumerables;

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
			settingsCopy._registredAssemblies = this._registredAssemblies.ToHashSet();

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
		/// </summary>
		/// <returns>the default settings</returns>
		public static StackWrapperSettings Default
			=>
				new StackWrapperSettings()
					.RegisterEntryAssembly()
					.RegisterWrapperAsService()
					.RegisterAfterCloning(false)
					.TrackInstantiationDiff(false, callDispose: false)
					.InjectionVersioningMethod(ServedVersionTargetingMethod.None, @override: false)
					.InjectionServingMethods(Injector.Defaults.ServeAllStrict, @override: false)
					.WhenNoMoreTasks(AsyncWaitingMethod.Exit)
					.ServeIEnumerables();


		/// <summary>
		/// Creates a new StackWrapperSettings with default parameters, but for subtraction class design: <br/>
		/// everything is served by default, and you must instead use <c>[Ignored]</c> on properties and fields you don't want injected
		/// </summary>
		/// <seealso cref="Attributes.IgnoredAttribute"/>
		public static StackWrapperSettings DefaultBySubtraction
			=>
				Default
					.InjectionServingMethods(Injector.Defaults.ServeAll, @override: true);
	}
}