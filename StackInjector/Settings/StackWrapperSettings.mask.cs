using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace StackInjector.Settings
{
	partial class StackWrapperSettings
	{

		public sealed class Mask
		{
			// registration
			internal HashSet<Assembly>                  _registredAssemblies                    = new HashSet<Assembly>();
			internal bool                               _registerEntryPointAssembly;
			internal bool                               _registerWrapperAsService;
			internal bool                               _registerAfterCloning;

			public static Mask Default =>
						new Mask()
						{
							_registerEntryPointAssembly = true,
							_registerWrapperAsService = true,
							_registerAfterCloning = false
						};
						//.RegisterEntryAssembly()
						//.RegisterWrapperAsService()
						//.RegisterAfterCloning(false);

			internal Mask () { }


			/// <summary>
			/// Register an external assembly from wich you want classes to be laoded.
			/// </summary>
			/// <param name="assemblies"></param>
			/// <returns>the modified settings</returns>
			public Mask RegisterAssemblies ( params Assembly[] assemblies )
			{
				if ( assemblies is null )
					throw new ArgumentNullException(nameof(assemblies));

				foreach ( var assembly in assemblies )
					this._registredAssemblies.Add(assembly);
				return this;
			}

			/// <summary>
			/// Automatically register all domain assemblies, filtering the specified ones.<br/>
			/// <b>Warning: CPU expensive</b>
			/// </summary>
			/// <param name="regexFilter">a regex string used to filter unwanted matching assemblies</param>
			/// <returns>the modified settings</returns>
			public Mask RegisterDomain ( string regexFilter = Injector.Defaults.AssemblyRegexFilter )
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
			public Mask RegisterAssemblyOf<T> ()
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
			public Mask RegisterEntryAssembly ( bool register = true )
			{
				this._registerEntryPointAssembly = register;
				return this;
			}

			/// <summary>
			/// Register the wrapper as a service, so it can be accessed in contained classes.
			/// Default is true.
			/// </summary>
			/// <returns>the modified settings</returns>
			public Mask RegisterWrapperAsService ( bool register = true )
			{
				this._registerWrapperAsService = register;
				return this;
			}


			/// <summary>
			/// If set, when cloned the StackWrapper will re-scans all assemblies before the injection.
			/// Used to update assemblies with new types.
			/// </summary>
			/// <returns>the modified settings</returns>
			public Mask RegisterAfterCloning ( bool register = true )
			{
				this._registerAfterCloning = register;
				return this;
			}

		}

	}
}
