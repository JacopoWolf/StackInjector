using System.Collections.Generic;
using System.Linq;
using StackInjector.Exceptions;

namespace StackInjector.Core
{
	internal partial class InjectionCore
	{

		internal void Serve ( bool cloned = false )
		{
			// those don't need to be inside the lock.
			var injected = new HashSet<object>();
			var toInject = new Queue<object>();

			if ( cloned )
				this.instances.CountAllInstances();


			// ensures that two threads are not trying to Dispose/InjectAll at the same time
			lock ( this._lock )
			{
				// EntryType must be a class
				this.EntryType = this.ClassOrVersionFromInterface(this.EntryType);

				// instantiates and enqueues the EntryPoint. initializes the loop
				toInject.Enqueue(this.OfTypeOrInstantiate(this.EntryType));
				checkInstancesLimit();

				// enqueuing loop
				while ( toInject.Any() )
				{
					var next = toInject.Dequeue();
					var usedServices = this.InjectServicesInto(next);

					// this object has been injected
					injected.Add(next);

					// foreach injected object check if it has already been injected. 
					// saves time in most situations
					foreach ( var service in usedServices )
					{
						if ( !injected.Contains(service) )
						{
							toInject.Enqueue(service);
							checkInstancesLimit();
						}
					}
				}

				// cleanup
				if ( this.settings.Injection._cleanUnusedTypesAftInj )
					this.RemoveUnusedTypes();



				void checkInstancesLimit ()
				{
					if ( this.instances.total_count > this.settings.Injection._limitInstancesCount )
						throw new InstancesLimitReachedException(
							$"Reached limit of {this.settings.Injection._limitInstancesCount} instances."
						);
				}
			}

		}
	}
}
