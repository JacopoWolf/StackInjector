using System;
using StackInjector.Core;
using StackInjector.Exceptions;
using StackInjector.Settings;
using StackInjector.Wrappers;

[assembly:CLSCompliant(true)]
namespace StackInjector
{
	/// <summary>
	/// <para>Static factory class exposing methods to create new StackWrappers.</para>
	/// <para>Note that using any of the exposed methods will analyze the whole target assembly.</para>
	/// <para>If you want to clone an existing structure, see 
	/// <see cref="Core.Cloning.ICloneableCore.CloneCore(StackWrapperSettings)"/></para>
	/// </summary>
	/// <seealso cref="StackWrapperSettings"/>
	public static partial class Injector
	{
		/// <summary>
		/// <para>Create a new <see cref="IStackWrapper{TEntry}"/> from the <typeparamref name="T"/> entry point.</para>
		/// <para>If no setting are specified the default ones will be used.</para>
		/// </summary>
		/// <typeparam name="T">The type of the entry point</typeparam>
		/// <param name="settings">settings for this StackWrapper</param>
		/// <returns>The Initialized StackWrapper</returns>
		/// <exception cref="InvalidEntryTypeException"></exception>
		/// <exception cref="NotAServiceException"></exception>
		/// <exception cref="ServiceNotFoundException"></exception>
		/// <exception cref="ImplementationNotFoundException"></exception>
		/// <exception cref="StackInjectorException"></exception>
		public static IStackWrapper<T> From<T> ( StackWrapperSettings settings = null )
		{
			if( settings == null )
				settings = StackWrapperSettings.Default;

			// create the core and wrap it
			var core = new InjectionCore( settings )
			{
				EntryType = typeof(T)
			};

			var wrapper = new StackWrapper<T>(core);

			// initialize the injection process
			////core.ReadAssemblies();
			core.Serve();


			return wrapper;
		}



		/// <summary>
		/// <para>Create a new generic asyncronous StackWrapper from the <typeparamref name="TEntry"/>
		/// entry class with the specified delegate to apply to apply as digest.</para>
		/// </summary>
		/// <typeparam name="TEntry">class from which start injection</typeparam>
		/// <typeparam name="TIn">type of elements in input</typeparam>
		/// <typeparam name="TOut">type of elements in output</typeparam>
		/// <param name="digest">delegate used to call the relative method to perform on submitted items</param>
		/// <param name="settings">the settings to use with this object. If null, use default.</param>
		/// <returns>The created wrapper</returns>
		/// <example><c>Injector.AsyncFrom&lt;MyClass>( (e,i,t) => e.Digeset(i,t) )</c></example>
		/// <exception cref="InvalidEntryTypeException"></exception>
		/// <exception cref="NotAServiceException"></exception>
		/// <exception cref="ServiceNotFoundException"></exception>
		/// <exception cref="ImplementationNotFoundException"></exception>
		/// <exception cref="StackInjectorException"></exception>
		public static IAsyncStackWrapper<TEntry, TIn, TOut> AsyncFrom<TEntry, TIn, TOut>
			(
				AsyncStackDigest<TEntry, TIn, TOut> digest,
				StackWrapperSettings settings = null
			)
		{
			if( settings == null )
				settings = StackWrapperSettings.Default;

			// create the core and wrap it
			var core = new InjectionCore( settings )
			{
				EntryType = typeof(TEntry)
			};

			var wrapper = new AsyncStackWrapper<TEntry, TIn,TOut>(core)
			{
				StackDigest = digest
			};

			// initialize the injection process
			////core.ReadAssemblies();
			core.Serve();

			return wrapper;
		}

	}
}