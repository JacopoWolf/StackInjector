﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using StackInjector.Attributes;
using StackInjector.Settings;

namespace StackInjector.Core
{
	internal partial class InjectionCore
	{
		private static readonly Type _istackwrappercore = typeof(IStackWrapperCore);

		// performs versioning on the specified type
		private IEnumerable<Type> Version ( Type targetType, ServedAttribute servedAttribute )
		{
			if (targetType.IsSubclassOf( _istackwrappercore ) || targetType == _istackwrappercore)
				return this.instances.TypesAssignableFrom(targetType);

			if (this.settings.Versioning._customBindings != null &&
				this.settings.Versioning._customBindings.TryGetValue(targetType,out var t) )
			{
				return Enumerable.Repeat(t, 1);
			}
			

			var targetVersion = servedAttribute?.TargetVersion ?? 0.0;
			var method =
				(this.settings.Versioning._overrideTargetingMethod || servedAttribute is null || !(servedAttribute._targetingDefined))
					? this.settings.Versioning._targetingMethod
					: servedAttribute.TargetingMethod;

			IEnumerable<TypeInfo> candidateTypes =
				(this.settings.Versioning.AssemblyLookUpOrder.Any())
				?
				this.settings.Versioning
					.AssemblyLookUpOrder
					.SelectMany( a => a.DefinedTypes )
					.Where( t => t.IsClass && !t.IsAbstract
						&& targetType.IsAssignableFrom(t) && !this.settings.Mask.IsMasked(t) )
				:
				targetType
					.Assembly
					.DefinedTypes
					.Where( t => t.IsClass && !t.IsAbstract && targetType.IsAssignableFrom(t) );


			return method switch
			{
				ServedVersionTargetingMethod.None
					=>
						candidateTypes,


				ServedVersionTargetingMethod.Exact
					=>
						candidateTypes
						.Where
						(
							t =>
								t.GetCustomAttribute<ServiceAttribute>()
									.Version == targetVersion
						),


				ServedVersionTargetingMethod.LatestMajor
					=>
						candidateTypes
						.Where
						(
							t =>
								t.GetCustomAttribute<ServiceAttribute>()
									.Version >= targetVersion
						)
						.OrderByDescending(t => t.GetCustomAttribute<ServiceAttribute>().Version),


				ServedVersionTargetingMethod.LatestMinor
					=>
						candidateTypes
						.Where
						(
							t =>
							{
								var v = t.GetCustomAttribute<ServiceAttribute>().Version;
								return
									v >= targetVersion
										&&
									v < Math.Floor(targetVersion + 1);
							}
						)
						.OrderByDescending(t => t.GetCustomAttribute<ServiceAttribute>().Version),


				_ => throw new NotImplementedException()
			};
		}

	}
}
