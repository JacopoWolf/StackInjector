using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using StackInjector.Attributes;
using StackInjector.Exceptions;

namespace StackInjector.Wrappers
{
    /// <summary>
    /// wraps a series of classes
    /// </summary>
    public sealed class StackWrapper
    {
        
        internal List<Type> UsedTypes { get; private set; } = new List<Type>();

        internal List<object> Instances { get; private set; } = new List<object>();


        /// <summary>
        /// reads all [Service] classes in the given assembly
        /// </summary>
        /// <param name="assembly"></param>
        internal void ReadAssembly( Assembly assembly )
        {
            this.UsedTypes.AddRange
                (
                     assembly
                     .GetTypes()
                     .Where(t => t.IsClass && t.GetCustomAttribute<ServiceAttribute>() != null)
                );
        }

        internal Type ClassOrFromInterface( Type type )
        {
            if( type.IsInterface )
            {
                //todo implement cool versioning logic here
                try
                {
                    return this.UsedTypes.First(t => t.GetInterface(type.Name) != null);
                }
                catch ( InvalidOperationException )
                {
                    throw new ImplementationNotFoundException(type, $"can't find implementation for {type.Name} in {type.Assembly.FullName}");
                }

            }
            else
                return type;
        }

        internal void InstantiateAndInjectServices ( Type type )
        {
            type = this.ClassOrFromInterface(type);

            // generates default instance
            ////object typeInstance = Activator.CreateInstance( type );

            var servicesFields = type
                .GetFields( BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance )
                .Where( fi => fi.GetCustomAttribute<ServedAttribute>() != null );

            foreach( var serviceField in servicesFields )
            {
                var serviceType = this.ClassOrFromInterface(serviceField.FieldType);

                //todo inject dependencies here


            }

        }

        
        /// <summary>
        /// internal constructor.
        /// </summary>
        internal StackWrapper() { }

    }
}