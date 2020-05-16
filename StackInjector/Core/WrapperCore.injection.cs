using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using StackInjector.Attributes;

namespace StackInjector.Core
{
    internal partial class WrapperCore
    {
        /// <summary>
        /// Injects services into the specified instance, instantiating them on necessity.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        internal IEnumerable<object> InjectServicesInto ( object instance )
        {
            var instantiated = new List<object>();
            var type = instance.GetType();

            // setting for ignoring service of members. Defined in [Service] attribute
            if( type.GetCustomAttribute<ServiceAttribute>()?.DoNotServeMembers ?? false )
                return instantiated;

            this.InjectFields(type, instance, ref instantiated);
            this.InjectProperties(type, instance, ref instantiated);


            return instantiated;
        }

        #region injection methods

        private void InjectFields ( Type type, object instance, ref List<object> instantiated )
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

        private void InjectProperties ( Type type, object instance, ref List<object> instantiated )
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

        #endregion

    }
}
