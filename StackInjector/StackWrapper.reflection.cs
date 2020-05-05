using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using StackInjector.Attributes;
using StackInjector.Exceptions;

namespace StackInjector
{
    public sealed partial class StackWrapper
    {

        //todo comment
        internal Type GetVersion ( Type oftype, ServedAttribute versioningInfo )
        {
            

            return null;
        }


        /// <summary>
        /// Returns type if it's a [Service] class,
        /// otherwise searches for a [Service] implementing the specified interface
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal Type ClassOrFromInterface ( Type type )
        {
            if( type.IsInterface )
            {
                try
                {
                    //todo remove versioning logic here
                    return this.ServicesWithInstances.Keys.First(t => t.GetInterface(type.Name) != null);
                }
                catch( InvalidOperationException )
                {
                    throw new ImplementationNotFoundException(type, $"can't find [Service] for interface {type.Name} in {type.Assembly.FullName}");
                }
            }
            else
                return type;
        }


        /// <summary>
        /// reads all [Service] classes 
        /// </summary>
        internal void ReadAssemblies ()
        {
            this.ServicesWithInstances =
                this
                .Settings
                .registredAssemblies
                .SelectMany
                (
                    assembly =>
                        assembly
                        .GetTypes()
                        .AsParallel()
                        .Where(t => t.IsClass && t.GetCustomAttribute<ServiceAttribute>() != null)
                )
                .ToDictionary
                (
                    t => t,
                    t => new List<object>(1) // new list with space for at least a new instance
                );
        }

    }
}
