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

#if FEATURE_REGISTRATION
		public Registration RegistrationOptions {get; private set;}
#endif


		private StackWrapperSettings () { }


		/// <summary>
		/// creates a deep copy of this settings object
		/// </summary>
		/// <returns></returns>
		public StackWrapperSettings Clone ()
		{
			//todo implement Clone() in every suboption
			throw new NotImplementedException();
			var settingsCopy = Default;
			// creats a deep copy of reference objects
			//settingsCopy.MaskOptions._registredAssemblies = this.MaskOptions._registredAssemblies.ToHashSet();

			return settingsCopy;
		}


		public static StackWrapperSettings Default => With(null, null, null);

		
		public static StackWrapperSettings With ( Injection injection, Runtime runtime, Mask mask )
			=> 
			new StackWrapperSettings()
			{
				MaskOptions = mask ?? Mask.BlackList,
				InjectionOptions = injection ?? Injection.Default,
				RuntimeOptions = runtime ?? Runtime.Default
			};


		/// <summary>
		/// Creates a new StackWrapperSettings with default parameters, but for subtraction class design: <br/>
		/// everything is served by default, and you must instead use <c>[Ignored]</c> on properties and fields you don't want injected
		/// </summary>
		/// <seealso cref="Attributes.IgnoredAttribute"/>
		public static StackWrapperSettings DefaultBySubtraction
			=> With(
					Injection.Default
					.InjectionServingMethods(DefaultConstants.ServeAll, true),
					null,
					null
				);


	}
}