using System.Collections.Generic;
using System.Linq;

namespace StackInjector.Core
{
    internal partial class InjectionCore
    {

        internal void ServeAll ()
        {
            // ensures that two threads are not trying to Dispose and InjectAll at the same time
            lock( this._lock )
            {

                var toInject = new Queue<object>();

                // saves time in later elaboration
                this.EntryPoint = this.ClassOrFromInterface(this.EntryPoint);

                // instantiates and enqueues the EntryPoint
                toInject.Enqueue(this.InstantiateService(this.EntryPoint));

                // enqueuing loop
                while( toInject.Any() )
                {
                    var next = toInject.Dequeue();
                    var usedServices = this.InjectServicesInto(next);

                    // this object has been injected
                    this.instances.SetInjectionStatus(next);

                    // foreach injected object check if it has already been injected. 
                    // saves time in most situations
                    foreach( var service in usedServices )
                        if( !this.instances.IsInjected(service) )
                            toInject.Enqueue(service);
                }

            }

        }




        // retrieves the entry point of the specified type
        internal T GetEntryPoint<T> ()
        {
            return
                (T)this
                    .instances
                    .OfType(this.EntryPoint)
                    .First();
        }
    }
}
