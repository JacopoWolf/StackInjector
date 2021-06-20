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
		private object OfTypeOrInstantiate ( Type type )
		{
			var serviceAtt = type.GetCustomAttribute<ServiceAttribute>();

			// manage exceptions
			if ( serviceAtt == null )
				throw new NotAServiceException(type, $"Type {type.FullName} is not a [Service]");


			return serviceAtt.Pattern switch
			{
				InstantiationPattern.AlwaysCreate
					=> this.InstantiateService(type),
				_
					=> (this.instances.ContainsKey(type) && this.instances[type].Any())
						? this.instances[type].First()
						: this.InstantiateService(type),
			};
		}


		// Instantiates the specified [Served] type
		private object InstantiateService ( Type type )
		{
			type = this.ClassOrVersionFromInterface(type);

			//todo add more constructor options
			if ( type.GetConstructor(Array.Empty<Type>()) == null )
				throw new MissingParameterlessConstructorException(type, $"Missing parameteless constructor for {type.FullName}");

			var instance = Activator.CreateInstance(type);

			this.instances.AddType(type); //try add
			this.instances[type].AddLast(instance);
			this.instances.total_count++;

			// if true, track instantiated objects
			if ( this.settings.Injection._trackInstancesDiff )
				this.instancesDiff.Add(instance);

			return instance;

		}

	}
}
