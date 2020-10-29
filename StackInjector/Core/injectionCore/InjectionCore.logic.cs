using System;
using System.Collections.Generic;
using System.Linq;

namespace StackInjector.Core
{
    internal partial class InjectionCore
    {

        internal void ServeAll ()
        {
            var injected = new HashSet<object>();

            // ensures that two threads are not trying to Dispose and InjectAll at the same time
            lock( this._lock )
            {

                var toInject = new Queue<object>();

                // saves time in later elaboration
                this.EntryType = this.ClassOrFromInterface(this.EntryType);

                // instantiates and enqueues the EntryPoint
                toInject.Enqueue(this.InstantiateService(this.EntryType));

                // enqueuing loop
                while( toInject.Any() )
                {
                    var next = toInject.Dequeue();
                    var usedServices = this.InjectServicesInto(next);

                    // this object has been injected
                    injected.Add(next);

                    // foreach injected object check if it has already been injected. 
                    // saves time in most situations
                    foreach( var service in usedServices )
                        if( !injected.Contains(service) )
                            toInject.Enqueue(service);
                }

            }

            //? should clear injected HashSet?

        }




        // retrieves the entry point of the specified type
        internal T GetEntryPoint<T> ()
        {
            return
                (T)this
                    .instances[this.EntryType]
                    .First();
        }
    }
}
