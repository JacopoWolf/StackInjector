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
            //todo wrap in try-catch
            //todo check for default constructor. If not present, throw exception

            type = this.ClassOrFromInterface(type);

            var instance = Activator.CreateInstance( type );

            //this.ServicesWithInstances[type] = instance;
            this.ServicesWithInstances.AddInstance(type, instance);

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
    }
}
