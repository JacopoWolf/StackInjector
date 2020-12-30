using System;
using System.Linq;
using System.Reflection;
using StackInjector.Attributes;
using StackInjector.Exceptions;
using StackInjector.Settings;

namespace StackInjector.Core
{
	internal partial class InjectionCore
	{
		// Instantiates the specified [Served] type
		private object InstantiateService ( Type type )
		{
			type = this.ClassOrVersionFromInterface(type);


			if( type.GetConstructor(Array.Empty<Type>()) == null )
				throw new MissingParameterlessConstructorException(type, $"Missing parameteless constructor for {type.FullName}");

			var instance = Activator.CreateInstance(type);


			this.instances[type].AddLast(instance);

			// if true, track instantiated objects
			if( this.settings._trackInstancesDiff )
				this.instancesDiff.Add(instance);

			return instance;

		}

		private object OfTypeOrInstantiate ( Type type, Type hostType = null )
		{
			var serviceAtt = type.GetCustomAttribute<ServiceAttribute>();

			// manage exceptions
			if( serviceAtt == null )
				throw new NotAServiceException(type, $"The type {type.FullName} is not annotated with [Service]");

			//todo 4.0: allow disabling of this check and add services at runtime
			if( !this.instances.ContainsKey(type) )
				throw new ServiceNotFoundException(type, $"The type {type.FullName} is not in a registred assembly!");


			switch( serviceAtt.Pattern )
			{
				default:
				case InstantiationPattern.Singleton:
					return (this.instances[type].Any())
						? this.instances[type].First()
						: this.InstantiateService(type);

				case InstantiationPattern.AlwaysCreate:
					AlwaysCreateCheck(type, hostType);
					return this.InstantiateService(type);
			}


			// static local check
			static void AlwaysCreateCheck ( Type type, Type host )
			{
				if( host is null )
					return;
				else if( type == host || type.IsSubclassOf(host) )
					throw new StackInjectorException(
							host,
							$"Service {type.FullName} is marked as {InstantiationPattern.AlwaysCreate} and cannot have itself or a derivate type served.",
							new InvalidOperationException()
						);
			}

		}
	}
}
