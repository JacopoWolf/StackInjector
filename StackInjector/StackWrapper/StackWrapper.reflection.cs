using System;
using System.Linq;
using System.Reflection;
using StackInjector.Attributes;
using StackInjector.Exceptions;

namespace StackInjector
{
    internal partial class StackWrapper
    {
        /// <summary>
        /// Returns type if it's a [Service] class,
        /// otherwise searches for a [Service] implementing the specified interface
        /// </summary>
        /// <param name="type"></param>
        /// <param name="servedAttribute"></param>
        /// <returns></returns>
        /// <exception cref="ImplementationNotFoundException"></exception>
        internal Type ClassOrFromInterface ( Type type, ServedAttribute servedAttribute = null )
        {
            if( type.IsInterface )
            {
                try
                {
                    var v = servedAttribute?.TargetVersion ?? 0.0;

                    var t = ( this.Settings.overrideTargettingMethod )
                                ? this.Settings.targettingMethod
                                : servedAttribute?.TargetingMethod ?? this.Settings.targettingMethod;

                    return this.Version(type, v, t);

                }
                catch( InvalidOperationException )
                {
                    if( servedAttribute is null )
                        throw new ImplementationNotFoundException(type, $"can't find [Service] for interface {type.Name}");
                    else
                        throw new ImplementationNotFoundException(type, $"can't find [Service] for v{servedAttribute.TargetVersion} for {type.Name}");
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
            if( this.Settings.registerEntryPointAssembly )
                this.Settings.registredAssemblies.Add(this.EntryPoint.Assembly);

            foreach
            (
                var t in this
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
            )
            {
                this.ServicesWithInstances.AddType(t);
            }
        }

    }
}
