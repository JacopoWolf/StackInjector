using System.Collections.Generic;
using System.Linq;

namespace StackInjector
{
    internal partial class StackWrapper
    {

        internal void ServeAll ()
        {

            // setting for referencing this object from instances inside
            if( this.Settings.registerSelf )
                this.ServicesWithInstances.AddInstance(this.GetType(), this);

            var toInject = new Queue<object>();

            // instantiates and enqueues the EntryPoint
            toInject.Enqueue
                (
                    this.InstantiateService(this.EntryPoint)
                );

            // enqueuing loop
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
