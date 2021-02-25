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
		/// <summary>
		/// manages masking options.
		/// </summary>
		public Mask MaskOptions { get; private set; }

		/// <summary>
		/// manages injection options
		/// </summary>
		public Injection InjectionOptions { get; private set; }

		/// <summary>
		/// manages runtime options
		/// </summary>
		public Runtime RuntimeOptions { get; private set; }



		private StackWrapperSettings () { }



		/// <summary>
		/// creates a deep copy of this settings object
		/// </summary>
		/// <returns>a cloned settings object</returns>
		public StackWrapperSettings Clone () =>
			With(
				(Injection)this.InjectionOptions.Clone(),
				(Runtime)this.RuntimeOptions.Clone(),
				(Mask)this.MaskOptions.Clone()
				);


		/// <summary>
		/// The default settings. see
		/// <seealso cref="Injection.Default"/>,
		/// <seealso cref="Runtime.Default"/>,
		/// <seealso cref="Mask.Disabled"/>
		/// </summary>
		public static StackWrapperSettings Default => With(null, null, null);

		
		/// <summary>
		/// create a new <see cref="StackWrapperSettings"/> with the specified options.
		/// </summary>
		/// <param name="injection">the injection options</param>
		/// <param name="runtime">the runtime options</param>
		/// <param name="mask">mask options</param>
		/// <returns></returns>
		public static StackWrapperSettings With ( Injection injection, Runtime runtime, Mask mask )
			=> 
			new StackWrapperSettings()
			{
				MaskOptions = mask ?? Mask.Disabled,
				InjectionOptions = injection ?? Injection.Default,
				RuntimeOptions = runtime ?? Runtime.Default
			};


		/// <summary>
		/// Creates a new StackWrapperSettings with default parameters, but for subtraction class design: <br/>
		/// everything is served by default, and you must instead use <c>[Ignored]</c> on properties and fields you don't want injected
		/// </summary>
		/// <seealso cref="Attributes.IgnoredAttribute"/>
		public static StackWrapperSettings DefaultBySubtraction => 
			With(
				injection: 
					Injection.Default
					.ServingMethod(DefaultConstants.ServeAll, true),
				mask:	
					null,
				runtime: 
					null
				);


	}
}