using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using StackInjector.Attributes;
using StackInjector.Settings;

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
            var serving = type.GetCustomAttribute<ServiceAttribute>()?.Serving ?? Injector.Defaults.ServingMethod;

            // don't waste time serving if not necessary
            if( serving == ServingMethods.DoNotServe )
                return instantiated;

            // if false avoids going though the properties/fields list a second time to filter
            var onlyWithAttrib = serving.HasFlag(ServingMethods.Strict);


            if ( serving.HasFlag(ServingMethods.Fields) )
                this.InjectFields(type, instance, ref instantiated, onlyWithAttrib);

            if ( serving.HasFlag(ServingMethods.Properties) )
                this.InjectProperties(type, instance, ref instantiated, onlyWithAttrib);


            return instantiated;
        }



        #region injection methods

        private void InjectFields ( Type type, object instance, ref List<object> instantiated, bool hasAttribute )
        {
            IEnumerable<FieldInfo> fields =
                    type.GetFields( BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance );

            if ( hasAttribute )
                fields = fields.Where( field => field.GetCustomAttribute<ServedAttribute>() != null );

            foreach( var serviceField in fields )
            {
                var serviceType = this.ClassOrFromInterface(serviceField.FieldType, serviceField.GetCustomAttribute<ServedAttribute>());
                var serviceInstance = this.OfTypeOrInstantiate(serviceType);
                serviceField.SetValue(instance, serviceInstance);

                instantiated.Add(serviceInstance);
            }
        }


        private void InjectProperties ( Type type, object instance, ref List<object> instantiated, bool hasAttribute )
        {
            IEnumerable<PropertyInfo> properties =
                    type.GetProperties( BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance );

            if ( hasAttribute )
                properties = properties.Where( property => property.GetCustomAttribute<ServedAttribute>() != null );

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