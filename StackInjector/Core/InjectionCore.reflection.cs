using System;
using System.Linq;
using System.Reflection;
using StackInjector.Attributes;
using StackInjector.Exceptions;

namespace StackInjector.Core
{
    internal partial class InjectionCore
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

                    var t = ( this.settings.overrideTargetingMethod )
                                ? this.settings.targetingMethod
                                : servedAttribute?.TargetingMethod ?? this.settings.targetingMethod;

                    return this.Version(type, v, t);

                }
                catch( InvalidOperationException )
                {
                    if( servedAttribute is null )
                        throw new ImplementationNotFoundException(type, $"can't find [Service] for interface {type.Name}");
                    else
                        throw new ImplementationNotFoundException(type, $"can't find [Service] for {type.Name} v{servedAttribute.TargetVersion}");
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
            if( this.settings.registerEntryPointAssembly )
                this.settings.registredAssemblies.Add(this.entryPoint.Assembly);

            foreach
            (
                var t in this
                .settings
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
                this.instances.AddType(t);
            }
        }

    }
}
