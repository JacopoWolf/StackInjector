using System.Collections.Generic;
using System.Linq;

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

            while( toInject.Any() )
            {
                var usedServices = this.InjectServicesInto(toInject.Dequeue());

                foreach( var service in usedServices )
                    toInject.Enqueue(service);
            }
        }



        /// <summary>
        /// retrieves the entry point of the specified type
        /// </summary>
        /// <returns></returns>
        internal IStackEntryPoint GetStackEntryPoint ()
        {
            return
                (IStackEntryPoint)
                this
                    .ServicesWithInstances
                    .OfType
                    (
                        this.ClassOrFromInterface(this.EntryPoint)
                    )
                    .First();
        }
    }
}
