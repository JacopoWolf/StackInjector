using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using StackInjector.Attributes;
using StackInjector.Exceptions;
using StackInjector.Settings;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StackInjector
{
    /// <summary>
    /// wraps a series of classes
    /// </summary>
    public sealed class StackWrapper
    {
        internal StackWrapperSettings Settings { get; set; }

        internal List<Type> UsedTypes { get; private set; } = new List<Type>();

        internal List<object> Instances { get; private set; } = new List<object>();




        /// <summary>
        /// reads all [Service] classes 
        /// </summary>
        internal void ReadAssemblies ()
        {
            this.UsedTypes.AddRange
                (
                    this.Settings._registredAssemblies
                    .SelectMany
                    (
                        assembly =>
                            assembly
                            .GetTypes()
                            .AsParallel()
                            .Where(t => t.IsClass && t.GetCustomAttribute<ServiceAttribute>() != null)
                    )
                );
        }


        internal object InstantiateService ( Type type )
        {
            //todo wrap in try-catch

            //todo check for default constructor. If not present, throw exception
            type = this.ClassOrFromInterface(type);
            var instance =  Activator.CreateInstance( type );
            this.Instances.Add(instance);
            return instance;

        }


        internal void InjectServicesInto ( object instance )
        {
            var type = instance.GetType();

            var servicesFields = 
                type
                    .GetFields( BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance )
                    .Where( fi => fi.GetCustomAttribute<ServedAttribute>() != null );

            foreach( var serviceField in servicesFields )
            {
                var serviceType = this.ClassOrFromInterface(serviceField.FieldType);
                var serviceInstance = this.Instances.Find( i => serviceType.IsInstanceOfType(i) );
                serviceField.SetValue(instance, serviceInstance);
            }

        }


        internal void InstantiateAndInjectAll()
        {
            foreach ( var type in this.UsedTypes )
                this.InstantiateService(type);

            foreach( var instance in this.Instances )
                this.InjectServicesInto(instance);
        }




        // utility method
        internal Type ClassOrFromInterface ( Type type )
        {
            if( type.IsInterface )
            {
                //todo implement cool versioning logic here
                try
                {
                    return this.UsedTypes.First(t => t.GetInterface(type.Name) != null);
                }
                catch( InvalidOperationException )
                {
                    throw new ImplementationNotFoundException(type, $"can't find [Service] for interface {type.Name} in {type.Assembly.FullName}");
                }
            }
            else
                return type;
        }

        internal IStackEntryPoint GetStackEntryPoint ()
        {
            return (IStackEntryPoint)this.Instances.Find(i => typeof(IStackEntryPoint).IsInstanceOfType(i));
        }





        /// <summary>
        /// internal constructor.
        /// </summary>
        internal StackWrapper ( StackWrapperSettings settings ) => this.Settings = settings;

        /// <summary>
        /// Start this Wrapper
        /// </summary>
        public void Start ()
        {
            this.GetStackEntryPoint().EntryPoint();
        }

    }
}