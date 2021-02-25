using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using StackInjector.Attributes;
using StackInjector.Exceptions;

namespace StackInjector.Core
{
	internal partial class InjectionCore
	{
		// Returns type if it's a [Service] class,
		// otherwise searches for a [Service] implementing the specified interface
		private Type ClassOrVersionFromInterface ( Type type, ServedAttribute servedAttribute = null )
		{
			if( type.IsInterface )
			{
				IEnumerable<Type> versions = this.instances.TypesAssignableFrom(type);

				// is there already an implementation for the interface?
				if ( versions.Any() )
				{
					return versions.First();
				}
				else
				{
					versions = this.Version(type, servedAttribute);
					if ( versions.Any() )
					{
						var t = versions.First();
						MaskPass(t);
						return t;
					}

					if ( servedAttribute is null )
						throw new ImplementationNotFoundException(type, $"can't find [Service] for interface {type.Name}");
					else
						throw new ImplementationNotFoundException(type, $"can't find [Service] for {type.Name} v{servedAttribute.TargetVersion}");
				}
			}
			else
			{
				MaskPass(type);
				return type;
			}


			void MaskPass (Type type)
			{
				if ( this.settings.MaskOptions.IsMasked(type) )
					throw new Exception($"Type {type.Name} is { (this.settings.MaskOptions._isWhiteList ? "not whitelisted" : "blacklisted")}");
				//todo create custom exception
			}
			

		}

		// moved to InjectionCore.versioning.cs
		////reads all [Service] classes
		////internal void ReadAssemblies ()
		////{
		////	if ( this.settings.MaskOptions._registerEntryPointAssembly )
		////		this.settings.MaskOptions._registredAssemblies.Add(this.EntryType.Assembly);

		////	foreach
		////	(
		////		var t in this
		////		.settings
		////		.MaskOptions
		////		._registredAssemblies
		////		.SelectMany
		////		(
		////			assembly =>
		////				assembly
		////				.GetTypes()
		////				.AsParallel()
		////				.Where(t => t.IsClass && t.GetCustomAttribute<ServiceAttribute>() != null)
		////		)
		////	)
		////	{
		////		this.instances.AddType(t);
		////	}
		////}

	}
}
