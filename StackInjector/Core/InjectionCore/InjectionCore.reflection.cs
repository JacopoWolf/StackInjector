using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
			if ( type.IsInterface )
			{
				IEnumerable<Type> versions = this.instances.TypesAssignableFrom(type);

				// has an implementation for the interface already been found?
				if ( versions.Any() )
				{
					return versions.First();
				}
				else
				{
					versions = this.Version(type, servedAttribute);
					if ( versions.Any() )
					{
						//todo check for multiple valid versions
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


			void MaskPass ( Type type )
			{
				if ( this.settings.Mask.IsMasked(type) ) //todo create custom exception
					throw new InvalidOperationException($"Type {type.Name} is { (this.settings.Mask._isWhiteList ? "not whitelisted" : "blacklisted")}");

			}


		}

	}
}
