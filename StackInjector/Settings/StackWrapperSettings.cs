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
		public MaskOptions Mask { get; private set; }

		/// <summary>
		/// manages injection options
		/// </summary>
		public InjectionOptions Injection { get; private set; }

		/// <summary>
		/// manages runtime options
		/// </summary>
		public RuntimeOptions Runtime { get; private set; }



		private StackWrapperSettings () { }



		/// <summary>
		/// creates a deep copy of this settings object
		/// </summary>
		/// <returns>a cloned settings object</returns>
		public StackWrapperSettings Clone () =>
			With(
				(InjectionOptions)this.Injection.Clone(),
				(RuntimeOptions)this.Runtime.Clone(),
				(MaskOptions)this.Mask.Clone()
				);


		/// <summary>
		/// The default settings. see
		/// <seealso cref="InjectionOptions.Default"/>,
		/// <seealso cref="RuntimeOptions.Default"/>,
		/// <seealso cref="MaskOptions.Disabled"/>
		/// </summary>
		public static StackWrapperSettings Default => With();

		
		/// <summary>
		/// create a new <see cref="StackWrapperSettings"/> with the specified options.
		/// </summary>
		/// <param name="injection">the injection options</param>
		/// <param name="runtime">the runtime options</param>
		/// <param name="mask">mask options</param>
		/// <returns></returns>
		public static StackWrapperSettings With ( InjectionOptions injection = null, RuntimeOptions runtime = null, MaskOptions mask = null )
			=> 
			new StackWrapperSettings()
			{
				Mask = mask ?? MaskOptions.Disabled,
				Injection = injection ?? InjectionOptions.Default,
				Runtime = runtime ?? RuntimeOptions.Default
			};


		/// <summary>
		/// Creates a new StackWrapperSettings with default parameters, but for subtraction class design: <br/>
		/// everything is served by default, and you must instead use <c>[Ignored]</c> on properties and fields you don't want injected
		/// </summary>
		/// <seealso cref="Attributes.IgnoredAttribute"/>
		public static StackWrapperSettings DefaultBySubtraction => 
			With(
				injection: 
					InjectionOptions.Default
					.ServingMethod(DefaultConstants.ServeAll, true),
				mask:	
					null,
				runtime: 
					null
				);


	}
}