﻿using System;
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

		private object OfTypeOrInstantiate ( Type type )
		{
			var serviceAtt = type.GetCustomAttribute<ServiceAttribute>();

			// manage exceptions
			if( serviceAtt == null )
				throw new NotAServiceException(type, $"The type {type.FullName} is not annotated with [Service]");

			//todo 4.0: allow disabling of this check and add services at runtime
			if( !this.instances.ContainsKey(type) )
				throw new ServiceNotFoundException(type, $"The type {type.FullName} is not in a registred assembly!");


			return serviceAtt.Pattern switch
			{
				InstantiationPattern.AlwaysCreate
					=> this.InstantiateService(type),
				_
					=> (this.instances[type].Any())
						? this.instances[type].First()
						: this.InstantiateService(type),
			};
		}


		// removes instances of the tracked instantiated types and call their Dispose method. Thread safe.
		protected internal void RemoveInstancesDiff ()
		{
			if( !this.settings._trackInstancesDiff )
				return;

			// ensures that two threads are not trying to Dispose and InjectAll at the same time
			lock( this._lock )
			{
				foreach( var instance in this.instancesDiff )
				{
					this.instances[instance.GetType()].Remove(instance);

					// if the relative setting is true, check if the instance implements IDisposable and call it
					if( this.settings._callDisposeOnInstanceDiff && instance is IDisposable disposable )
						disposable.Dispose();
				}

				this.instancesDiff.Clear();
			}
		}

	}
}
