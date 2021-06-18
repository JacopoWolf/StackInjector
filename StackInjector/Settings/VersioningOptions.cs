using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace StackInjector.Settings
{

	/// <summary>
	/// Used to determine how to find the correct type from an interface.
	/// </summary>
	public sealed class VersioningOptions : IOptions
	{

		internal ServedVersionTargetingMethod       _targetingMethod                        = ServedVersionTargetingMethod.None;
		internal bool                               _overrideTargetingMethod;

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
		public object Clone () => this.MemberwiseClone();

		IOptions IOptions.CreateDefault () => Default;



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
		/// <returns></returns>
		public VersioningOptions AddAssembliesToLookup ( params Assembly[] assemblies )
		{
			foreach ( var a in assemblies )
			{
				this.AssemblyLookUpOrder.Add(a);
			}
			return this;
		}

		

		#endregion

	}
}
