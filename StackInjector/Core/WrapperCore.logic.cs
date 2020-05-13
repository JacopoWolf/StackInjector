using System.Collections.Generic;
using System.Linq;

namespace StackInjector.Core
{
    internal partial class WrapperCore
    {

        internal void ServeAll ()
        {
            var toInject = new Queue<object>();

            // saves time in later elaboration
            this.entryPoint = this.ClassOrFromInterface(this.entryPoint);

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
                    .OfType( this.entryPoint )
                    .First();
        }
    }
}
