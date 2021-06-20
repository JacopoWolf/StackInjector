using System;
using System.Collections.Generic;
using System.Reflection;

namespace StackInjector.Settings
{

	/// <summary>
	/// Used to determine how to find the correct type from an interface.
	/// </summary>
	public sealed class VersioningOptions : IOptions
	{

		internal ServedVersionTargetingMethod       _targetingMethod                        = ServedVersionTargetingMethod.None;
		internal bool                               _overrideTargetingMethod;

		internal Dictionary<Type, Type>             _customBindings;

		/// <summary>
		/// A set of assemblies overriding where to look up types in, when not empty.
		/// </summary>
		public HashSet<Assembly> AssemblyLookUpOrder { get; private set; } = new HashSet<Assembly>();

		



		internal VersioningOptions () { }



		/// <summary>
		/// The default versioning options, valorized as following:
		/// <list type="table">
		///	<item>
		///		<term><see cref="VersioningMethod(ServedVersionTargetingMethod, bool)"/></term>
		///		<description><see cref="ServedVersionTargetingMethod.None"/>, false</description>
		///	</item><item>
		///		<term><see cref="AddAssembliesToLookup(Assembly[])"/></term>
		///		<description>empty</description>
		///	</item>
		/// </list>
		/// </summary>
		public static VersioningOptions Default => new VersioningOptions();


		/// <inheritdoc/>
		public object Clone ()
		{
			return this.MemberwiseClone();
		}

		IOptions IOptions.CreateDefault ()
		{
			return Default;
		}



		#region configuration methods

		/// <summary>
		/// Overrides default versioning method
		/// </summary>
		/// <param name="targetMethod">the new default targeting method</param>
		/// <param name="override">if true, versioning methods for [Served] fields and properties are overriden</param>
		/// <returns>the modified settings</returns>
		public VersioningOptions VersioningMethod ( ServedVersionTargetingMethod targetMethod, bool @override = false )
		{
			this._targetingMethod = targetMethod;
			this._overrideTargetingMethod = @override;
			return this;
		}


		/// <summary>
		/// add a range of assemblies from which, in order, look for types when injecting interfaces, overriding any default option.
		/// </summary>
		/// <param name="assemblies"></param>
		/// <returns>the modified options</returns>
		public VersioningOptions AddAssembliesToLookup ( params Assembly[] assemblies )
		{
			foreach ( var a in assemblies )
			{
				this.AssemblyLookUpOrder.Add(a);
			}
			return this;
		}

		#region bindings

		/// <summary>
		/// binds the specified implementation with the interface, ensuring that versioning
		/// the specified interface will always return the wanted implementation.
		/// </summary>
		/// <typeparam name="TInterface">Must be an interface type.</typeparam>
		/// <typeparam name="TImpl">The implementation type for the <typeparamref name="TInterface"/> type </typeparam>
		/// <returns>the modified options</returns>
		public VersioningOptions AddInterfaceBinding<TInterface, TImpl> ()
			where TImpl : class, new()
		{
			var ti = typeof(TInterface);
			if ( !ti.IsInterface )
			{
				throw new ArgumentException($"{ti.FullName} is not an interface!");
			}

			if ( this._customBindings == null )
				this._customBindings = new Dictionary<Type, Type>();

			this._customBindings.Add(typeof(TInterface), typeof(TImpl));
			return this;
		}

		/// <summary>
		/// removes the specified interface type.
		/// </summary>
		/// <returns>the modified options</returns>
		public VersioningOptions RemoveInterfaceBinding<TInterface> ()
		{
			this._customBindings.Remove(typeof(TInterface));
			return this;
		}

		/// <summary>
		/// removes all interface bindings
		/// </summary>
		/// <returns>the modified options</returns>
		public VersioningOptions RemoveInterfaceBinding ()
		{
			this._customBindings.Clear();
			this._customBindings = null;
			return this;
		}

		#endregion


		#endregion

	}
}
