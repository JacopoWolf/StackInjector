using System;
using System.Collections.Generic;
using System.Linq;
using StackInjector.Exceptions;

namespace StackInjector.Core
{
    internal partial class InjectionCore
    {

        internal void ServeAll ()
        {
            // those don't need to be inside the lock.
            var injected = new HashSet<object>();
            var toInject = new Queue<object>();


            // ensures that two threads are not trying to Dispose/InjectAll at the same time
            lock( this._lock )
            {
                // entry type must always be a class
                this.EntryType = this.ClassOrVersionFromInterface(this.EntryType);

                // instantiates and enqueues the EntryPoint
                toInject.Enqueue(this.OfTypeOrInstantiate(this.EntryType));

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

        }




        // retrieves the entry point of the specified type
        internal T GetEntryPoint<T> ()
        {
            var entries = this.instances[this.EntryType];

            return (entries.Any())
                ? (T)entries.First()
                : throw new InvalidEntryTypeException
                    (
                        $"No instance found for entry type {this.EntryType.FullName}",
                        innerException: new ServiceNotFoundException(typeof(T), string.Empty)
                    );
        }


        // remove types with no instances
        internal void RemoveUnusedTypes ()
        {
            var unused = this.instances
                            .Where(p => !p.Value.Any())
                            .Select(p=>p.Key)
                            .ToList();

            foreach( var type in unused )
                this.instances.Remove(type);
        }

    }
}
