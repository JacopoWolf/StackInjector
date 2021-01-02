using System.Collections.Generic;
using System.Linq;

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

				if( this.settings._cleanUnusedTypesAftInj )
					this.RemoveUnusedTypes();

			}

		}
	}
}
