using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using StackInjector.Attributes;
using StackInjector.Exceptions;

namespace StackInjector
{
    internal partial class StackWrapper
    {



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
        }

        #region utilities



        /// <summary>
        /// retrieves the entry point of the specified type
        /// </summary>
        /// <returns></returns>
        internal IStackEntryPoint GetStackEntryPoint ()
        {
            return
                (IStackEntryPoint)
                this
                    .ServicesWithInstances[this.ClassOrFromInterface(this.EntryPoint)];
        }

        #endregion
    }
}
