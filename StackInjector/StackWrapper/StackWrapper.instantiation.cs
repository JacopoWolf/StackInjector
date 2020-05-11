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
        /// Instantiates the specified [Served] type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal object InstantiateService ( Type type )
        {
            type = this.ClassOrFromInterface(type);

            //todo check for default constructor. If not present, throw custom exception
            var instance = Activator.CreateInstance( type );

            this.ServicesWithInstances.AddInstance(type, instance);

            // if true, track instantiated objects
            if( this.Settings.trackInstancesDiff )
                this.instancesDiff.Add(instance);

            return instance;

        }

        internal object OfTypeOrInstantiate ( Type type )
        {
            if( type.GetCustomAttribute<ServiceAttribute>() == null )
                throw new NotAServiceException(type, $"The type {type.FullName} is not annotated with [Service]");

            if( !this.ServicesWithInstances.ContainsType(type) )
                throw new ClassNotFoundException(type, $"The type {type.FullName} is not in a registred assembly!");


            var instanceOfType = this.ServicesWithInstances.OfType(type).First();

            if( instanceOfType is null )
                return this.InstantiateService(type);
            else
                return instanceOfType;
        }


        /// <summary>
        /// removes instances of the tracked instantiated types and call their Dispose method
        /// </summary>
        protected void RemoveInstancesDiff ()
        {
            if( !this.Settings.trackInstancesDiff )
                return;

            foreach( var instance in this.instancesDiff )
            {
                this.ServicesWithInstances.RemoveInstance(instance.GetType(), instance);

                // if the relative setting is true, check if the instance implements IDisposable and call it
                if( this.Settings.callDisposeOnInstanceDiff && instance is IDisposable disposable )
                    disposable.Dispose();
            }

        }

    }
}
