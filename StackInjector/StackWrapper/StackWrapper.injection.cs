using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using StackInjector.Attributes;

namespace StackInjector
{
    internal partial class StackWrapper
    {
        /// <summary>
        /// Injects services into the specified instance, instantiating them on necessity.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        internal virtual IEnumerable<object> InjectServicesInto ( object instance )
        {
            var instantiated = new List<object>();
            var type = instance.GetType();

            // setting for ignoring service of members. Defined in [Service] attribute
            if( type.GetCustomAttribute<ServiceAttribute>()?.DoNotServeMembers ?? false )
                return instantiated;

            // fields
            {
                var fields =
                    type
                        .GetFields( BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance )
                        .Where( field => field.GetCustomAttribute<ServedAttribute>() != null );

                foreach( var serviceField in fields )
                {
                    var serviceType = this.ClassOrFromInterface(serviceField.FieldType, serviceField.GetCustomAttribute<ServedAttribute>());
                    var serviceInstance = this.OfTypeOrInstantiate(serviceType);
                    serviceField.SetValue(instance, serviceInstance);

                    instantiated.Add(serviceInstance);
                }
            }

            // properties
            {
                var properties =
                    type
                        .GetProperties( BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance )
                        .Where( property => property.GetCustomAttribute<ServedAttribute>() != null );

                foreach( var propertyField in properties )
                {
                    var serviceType = this.ClassOrFromInterface( propertyField.PropertyType, propertyField.GetCustomAttribute<ServedAttribute>() );
                    var serviceInstance = this.OfTypeOrInstantiate( serviceType );
                    propertyField.SetValue(instance, serviceInstance);

                    instantiated.Add(serviceInstance);
                }

            }

            return instantiated;
        }
    }
}
