using System;
using System.Collections.Generic;
using System.Linq;

namespace StackInjector.Core
{
    internal partial class WrapperCore
    {

        internal void ServeAll ( )
        {

            ////// setting for referencing this object from instances inside
            ////if( this.settings.registerSelf )
            ////    this.instances.AddInstance(this.GetType(), this);

            var toInject = new Queue<object>();

            // instantiates and enqueues the EntryPoint
            toInject.Enqueue
                (
                    this.InstantiateService(this.entryPoint)
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
        internal T GetEntryPoint<T> ()
        {
            return
                (T)this
                    .instances
                    .OfType
                    (
                        this.ClassOrFromInterface(this.entryPoint)
                    )
                    .First();
        }
    }
}
