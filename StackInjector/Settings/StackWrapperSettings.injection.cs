using System;
using System.Collections.Generic;
using System.Text;

namespace StackInjector.Settings
{
	partial class StackWrapperSettings
	{
		/// <summary>
		/// Options for the injection process.
		/// </summary>
		public sealed class Injection : IOption
		{
			internal ServedVersionTargetingMethod       _targetingMethod                        = ServedVersionTargetingMethod.None;
			internal bool                               _overrideTargetingMethod;

			internal ServingMethods                     _servingMethod                          = DefaultConstants.ServeAllStrict;
			internal bool                               _overrideServingMethod;
			internal bool                               _cleanUnusedTypesAftInj;
			internal uint                               _limitInstancesCount                    = 128;

			internal bool                               _serveEnumerables						= true;

			// disposing
			internal bool                               _trackInstancesDiff;
			internal bool                               _callDisposeOnInstanceDiff;


			internal Injection () { }


			IOption IOption.CreateDefault () => Default;

			/// <inheritdoc/>
			public object Clone () => MemberwiseClone();


			/// <summary>
			/// The default injection. Settings are valorized as following:
			/// <list type="table">
			/// <item>
			///		<term><see cref="VersioningMethod(ServedVersionTargetingMethod, bool)"/></term>
			///		<description><see cref="ServedVersionTargetingMethod.None"/>, false</description>
			///	</item><item>
			///		<term><see cref="ServingMethod(ServingMethods, bool)"/></term><description>
			///		<see cref="DefaultConstants.ServeAllStrict"/>, false</description>
			///	</item><item>
			///		<term><see cref="RemoveUnusedTypesAfterInjection(bool)"/></term>
			///		<description>false</description>
			///	</item><item>
			///		<term><see cref="LimitInstancesCount(uint)"/></term>
			///		<description>128</description>
			///	</item><item>
			///		<term><see cref="ServeIEnumerables(bool)"/></term>
			///		<description>true</description>
			///	</item><item>
			///		<term><see cref="TrackInstantiationDiff(bool, bool)"/></term>
			///		<description>false, false</description>
			///	</item>
			/// </list>
			/// </summary>
			public static Injection Default => new Injection();


			#region configuration methods


			/// <summary>
			/// Track every new instantiated class to be deleted upon Dispose.
			/// </summary>
			/// <param name="track">if true, track instances diff</param>
			/// <param name="callDispose">if true, call Dispose on services implementing <see cref="System.IDisposable"/></param>
			/// <returns>the modified settings</returns>
			public Injection TrackInstantiationDiff ( bool track = true, bool callDispose = true )
			{
				this._trackInstancesDiff = track;
				this._callDisposeOnInstanceDiff = callDispose;
				return this;
			}


			/// <summary>
			/// Overrides default targetting method
			/// </summary>
			/// <param name="targetMethod">the new default targetting method</param>
			/// <param name="override">if true, versioning methods for [Served] fields and properties are overriden</param>
			/// <returns>the modified settings</returns>
			public Injection VersioningMethod ( ServedVersionTargetingMethod targetMethod, bool @override = false )
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
			public Injection ServingMethod ( ServingMethods methods, bool @override = false )
			{
				this._servingMethod = methods;
				this._overrideServingMethod = @override;
				return this;
			}


			/// <summary>
			/// Remove the reference to unused types after the injection is finished. <br/>
			/// Usually not necessay, but can save memory after cloning 
			/// </summary>
			/// <returns>The modified settings</returns>
			public Injection RemoveUnusedTypesAfterInjection ( bool remove = true )
			{
				this._cleanUnusedTypesAftInj = remove;
				return this;
			}

			/// <summary>
			/// Limits the TOTAL number of instances. <br/>
			/// You can use <see cref="uint.MaxValue"/> to remove this limit, altough it is suggest to use the lowest possible value.
			/// </summary>
			/// <param name="count">the limit of total instances</param>
			/// <returns>The modified settings</returns>
			public Injection LimitInstancesCount ( uint count = 128 )
			{
				this._limitInstancesCount = count;
				return this;
			}

			/// <summary>
			/// Allows <see cref="IEnumerable{T}"/> to be injected with a list of every service implementing T
			/// </summary>
			/// <param name="serve">if true, serve</param>
			/// <returns>the modified settings</returns>
			public Injection ServeIEnumerables ( bool serve = true )
			{
				this._serveEnumerables = serve;
				return this;
			}


			#endregion


		}

	}
}
