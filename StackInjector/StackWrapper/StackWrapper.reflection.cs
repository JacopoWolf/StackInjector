using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using StackInjector.Attributes;
using StackInjector.Behaviours;
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
        internal Type ClassOrFromInterface ( Type type, ServedAttribute servedAttribute = null )
        {
            if( type.IsInterface )
            {
                try
                {
                    if( servedAttribute is null )
                        return
                            this
                                .ServicesWithInstances
                                .GetAllTypes()
                                .First(t => t.GetInterface(type.Name) != null);
                    else
                        return
                            this.Version
                            (
                                type,
                                servedAttribute,
                                this.Settings.targettingMethod
                            );
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

                ////.ToDictionary
                ////(
                ////    keySelector:        t => t,
                ////    elementSelector:    (Func<Type,object>)( t => null )
                ////);
        }

    }
}
