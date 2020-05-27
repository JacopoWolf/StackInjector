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
        /// Instantiates the specified [Served] type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private object InstantiateService ( Type type )
        {
            type = this.ClassOrFromInterface(type);

            //todo check for default constructor. If not present, throw custom exception
            var instance = Activator.CreateInstance( type );

            this.instances.AddInstance(type, instance);

            // if true, track instantiated objects
            if( this.settings.trackInstancesDiff )
                this.instancesDiff.Add(instance);

            return instance;

        }

        private object OfTypeOrInstantiate ( Type type )
        {
            if( type.GetCustomAttribute<ServiceAttribute>() == null )
                throw new NotAServiceException(type, $"The type {type.FullName} is not annotated with [Service]");

            if( !this.instances.ContainsType(type) )
                throw new ClassNotFoundException(type, $"The type {type.FullName} is not in a registred assembly!");


            var instanceOfType = this.instances.OfType(type).First();

            if( instanceOfType is null )
                return this.InstantiateService(type);
            else
                return instanceOfType;
        }


        /// <summary>
        /// removes instances of the tracked instantiated types and call their Dispose method
        /// </summary>
        protected internal void RemoveInstancesDiff ()
        {
            if( !this.settings.trackInstancesDiff )
                return;

            foreach( var instance in this.instancesDiff )
            {
                this.instances.RemoveInstance(instance.GetType(), instance);

                // if the relative setting is true, check if the instance implements IDisposable and call it
                if( this.settings.callDisposeOnInstanceDiff && instance is IDisposable disposable )
                    disposable.Dispose();
            }

            this.instancesDiff.Clear();

        }

    }
}
