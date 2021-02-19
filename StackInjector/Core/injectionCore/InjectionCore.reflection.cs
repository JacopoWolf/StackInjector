using System;
using System.Linq;
using System.Reflection;
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
				var versions = this.Version(type, servedAttribute);

				if( versions.Any() )
				{
					return versions.First();
				}
				else
				{
					if( servedAttribute is null )
						throw new ImplementationNotFoundException(type, $"can't find [Service] for interface {type.Name}");
					else
						throw new ImplementationNotFoundException(type, $"can't find [Service] for {type.Name} v{servedAttribute.TargetVersion}");
				}
			}
			else
			{
				return type;
			}
		}


		// reads all [Service] classes 
		internal void ReadAssemblies ()
		{
			if( this.settings.Mask._registerEntryPointAssembly )
				this.settings.Mask._registredAssemblies.Add(this.EntryType.Assembly);

			foreach
			(
				var t in this
				.settings
				.Mask
				._registredAssemblies
				.SelectMany
				(
					assembly =>
						assembly
						.GetTypes()
						.AsParallel()
						.Where(t => t.IsClass && t.GetCustomAttribute<ServiceAttribute>() != null)
				)
			)
			{
				this.instances.AddType(t);
			}
		}

	}
}
