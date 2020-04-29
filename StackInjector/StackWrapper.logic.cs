﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using StackInjector.Attributes;
using StackInjector.Exceptions;

namespace StackInjector
{
    public sealed partial class StackWrapper
    {
        /// <summary>
        /// reads all [Service] classes 
        /// </summary>
        internal void ReadAssemblies ()
        {
            this.AllServiceTypes =
                this.Settings
                ._registredAssemblies
                .SelectMany
                (
                    assembly =>
                        assembly
                        .GetTypes()
                        .AsParallel()
                        .Where(t => t.IsClass && t.GetCustomAttribute<ServiceAttribute>() != null)
                )
                .ToHashSet();

            // initializes the new instances list with the same number of the used types
            this.Instances = new List<object>(this.AllServiceTypes.Count);
        }


        #region instantiation

        /// <summary>
        /// Instantiates the specified [Served] type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal object InstantiateService ( Type type )
        {
            //todo wrap in try-catch
            //todo check for default constructor. If not present, throw exception

            type = this.ClassOrFromInterface(type);

            var instance = Activator.CreateInstance( type );

            this.Instances.Add(instance);
            return instance;

        }

        internal object OfTypeOrInstantiate ( Type type )
        {
            if( type.GetCustomAttribute<ServiceAttribute>() == null )
                throw new NotAServiceException(type, $"The type {type.FullName} is not annotated with [Service]");

            if( !this.AllServiceTypes.Contains(type) )
                throw new ClassNotFoundException(type,$"The type {type.FullName} is not in a registred assembly!");


            var InstOfType = this.Instances.FindAll(i => type.IsAssignableFrom( i.GetType() ));

            if( InstOfType.Any() )
            {
                return InstOfType.First(); //todo remove; versioning logic here
            }
            else
            {
                return this.InstantiateService(type);
            }
        }

        #endregion

        #region injection

        /// <summary>
        /// Injects services into the specified instance, instantiating them on necessity.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        internal IEnumerable<object> InjectServicesInto ( object instance )
        {
            var instantiated = new List<object>();
            var type = instance.GetType();

            // fields
            {
                var fields =
                    type
                        .GetFields( BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance )
                        .Where( field => field.GetCustomAttribute<ServedAttribute>() != null );

                foreach( var serviceField in fields )
                {
                    var serviceType = this.ClassOrFromInterface(serviceField.FieldType);
                    var serviceInstance = this.OfTypeOrInstantiate(serviceType); //this.Instances.Find( i => serviceType.IsInstanceOfType(i) );
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
                    var serviceType = this.ClassOrFromInterface( propertyField.PropertyType );
                    var serviceInstance = this.OfTypeOrInstantiate( serviceType );
                    propertyField.SetValue(instance, serviceInstance);

                    instantiated.Add(serviceInstance);
                }

            }

            return instantiated;
        }

        #endregion


        //? could parallelize
        internal void ServeAll ()
        {

            var toInject = new Queue<object>();

            // instantiates and enqueues the EntryPoint
            toInject.Enqueue
                (
                    this.InstantiateService(this.EntryPoint)
                );

            while ( toInject.Any() )
            {
                var usedServices = this.InjectServicesInto(toInject.Dequeue());

                foreach( var service in usedServices )
                    toInject.Enqueue(service);
            }

            ////foreach( var type in this.AllServiceTypes )
            ////    this.InstantiateService(type);

            ////foreach( var instance in this.Instances )
            ////    this.InjectServicesInto(instance);
        }

        #region utilities

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
                    //todo versioning logic here
                    return this.AllServiceTypes.First(t => t.GetInterface(type.Name) != null);
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
        /// retrieves the entry point of the specified type
        /// </summary>
        /// <returns></returns>
        internal IStackEntryPoint GetStackEntryPoint ()
        {
            return (IStackEntryPoint)this.Instances.Find(i => this.EntryPoint.IsInstanceOfType(i));
        }

        #endregion
    }
}
