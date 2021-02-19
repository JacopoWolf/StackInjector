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
	public sealed partial class StackWrapperSettings
	{

		public Mask MaskOptions { get; private set; }
		public Injection InjectionOptions { get; private set; }
		public Runtime RuntimeOptions { get; private set; }


		private StackWrapperSettings () { }


		/// <summary>
		/// creates a deep copy of this settings object
		/// </summary>
		/// <returns></returns>
		public StackWrapperSettings Copy ()
		{
			var settingsCopy = (StackWrapperSettings)this.MemberwiseClone();
			// creats a deep copy of reference objects
			settingsCopy.MaskOptions._registredAssemblies = this.MaskOptions._registredAssemblies.ToHashSet();

			return settingsCopy;
		}



		/// <summary>
		/// generates a new <see cref="StackWrapperSettings"/> with everything set to false
		/// </summary>
		/// <returns>empty settings</returns>
		public static StackWrapperSettings Empty
			=>
				new StackWrapperSettings();


		
		public static StackWrapperSettings Default ( Injection injection = null, Runtime runtime = null, Mask mask = null )
			=> 
			new StackWrapperSettings()
			{
				MaskOptions = mask ?? Mask.Default,
				InjectionOptions = injection ?? Injection.Default,
				RuntimeOptions = runtime ?? Runtime.Default
			};


		/// <summary>
		/// Creates a new StackWrapperSettings with default parameters, but for subtraction class design: <br/>
		/// everything is served by default, and you must instead use <c>[Ignored]</c> on properties and fields you don't want injected
		/// </summary>
		/// <seealso cref="Attributes.IgnoredAttribute"/>
		public static StackWrapperSettings DefaultBySubtraction
			=> Default(Injection.Default.InjectionServingMethods(Injector.Defaults.ServeAll, true));


	}
}