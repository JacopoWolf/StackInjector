using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using StackInjector.Attributes;
using StackInjector.Exceptions;
using StackInjector.Settings;

namespace StackInjector.Core
{
	internal partial class InjectionCore
	{

		// Injects services into the specified instance, instantiating them on necessity.
		private IEnumerable<object> InjectServicesInto ( object instance )
		{
			var servicesUsed = new List<object>();
			var type = instance.GetType();
			var serviceAtt = type.GetCustomAttribute<ServiceAttribute>();

			// if override is set to true, then use the settings's serving methods
			// otherwise check if the type has a service attribute and
			// if its property has been defined.
			var serving =
				(this.settings._overrideServingMethod || serviceAtt is null || !(serviceAtt._servingDefined))
					? this.settings._servingMethod
					: serviceAtt.Serving;

			// don't waste time serving if not necessary
			if( serving == ServingMethods.DoNotServe )
				return servicesUsed;

			// if false avoids going though the properties/fields list a second time to filter
			var onlyWithAttrib = serving.HasFlag(ServingMethods.Strict);


			if( serving.HasFlag(ServingMethods.Fields) )
				this.InjectFields(type, instance, ref servicesUsed, onlyWithAttrib);


			if( serving.HasFlag(ServingMethods.Properties) )
				this.InjectProperties(type, instance, ref servicesUsed, onlyWithAttrib);


			return servicesUsed;
		}



		#region injection methods

		private void InjectFields ( Type type, object instance, ref List<object> used, bool strict )
		{
			var fields =
					type.GetFields( BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance )
					.Where( f => f.GetCustomAttribute<IgnoredAttribute>() is null );

			if( strict )
				fields = fields.Where(field => field.GetCustomAttribute<ServedAttribute>() != null);

			foreach( var serviceField in fields )
			{
				var serviceInstance =
					this.InstTypeOrServiceEnum
					(
						type,
						serviceField.FieldType,
						serviceField.GetCustomAttribute<ServedAttribute>(),
						ref used
					);

				serviceField.SetValue(instance, serviceInstance);
			}
		}


		private void InjectProperties ( Type type, object instance, ref List<object> used, bool strict )
		{
			var properties =
					type.GetProperties( BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance )
					.Where( p => p.GetCustomAttribute<IgnoredAttribute>() is null );

			if( strict )
				properties = properties.Where(property => property.GetCustomAttribute<ServedAttribute>() != null);

			foreach( var serviceProperty in properties )
			{
				if( serviceProperty.GetSetMethod() is null )
					throw new NoSetterException(type, $"Property {serviceProperty.Name} of {type.FullName} has no setter!");

				var serviceInstance =
					this.InstTypeOrServiceEnum
					(
						type,
						serviceProperty.PropertyType,
						serviceProperty.GetCustomAttribute<ServedAttribute>(),
						ref used
					);

				serviceProperty.SetValue(instance, serviceInstance);
			}
		}


		// returns the instantiated object 
		private object InstTypeOrServiceEnum ( Type host, Type type, ServedAttribute servedAttribute, ref List<object> used )
		{
			if
			(
				this.settings._serveEnumerables
				&&
				type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>)
			)
			{
				var generic = type.GetGenericArguments()[0];

				// list of sorted valid types
				var validTypes = this.Version( generic, servedAttribute ).ToArray();

				// creates generic list using reflection
				var listType = typeof(List<>).MakeGenericType(generic);
				// cast to allow calling .Add()
				var instances = (IList)Activator.CreateInstance( listType );

				// gather instances if necessary
				foreach( var serviceType in validTypes )
				{
					var obj = this.OfTypeOrInstantiate(serviceType, host);
					instances.Add(obj);
					used.Add(obj);
				}
				return instances;
			}
			else
			{
				var serviceType = this.ClassOrVersionFromInterface( type, servedAttribute );

				var obj = this.OfTypeOrInstantiate(serviceType,host);
				used.Add(obj);
				return obj;
			}
		}

		#endregion

	}
}