using System;
using System.Linq;
using StackInjector.Exceptions;

namespace StackInjector.Core
{
	internal partial class InjectionCore
	{
		// retrieves the entry point of the specified type
		internal T GetEntryPoint<T> ()
		{
			var entries = this.instances[this.EntryType];

			return (entries.Any())
				? (T)entries.First()
				: throw new InvalidEntryTypeException
					(
						this.EntryType,
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


		// removes instances of the tracked instantiated types and call their Dispose method. Thread safe.
		protected internal void RemoveInstancesDiff ()
		{
			if( !this.settings.Injection._trackInstancesDiff )
				return;

			// ensures that two threads are not trying to Dispose and InjectAll at the same time
			lock( this._lock )
			{
				foreach( var instance in this.instancesDiff )
				{
					this.instances[instance.GetType()].Remove(instance);

					// if the relative setting is true, check if the instance implements IDisposable and call it
					if( this.settings.Injection._callDisposeOnInstanceDiff && instance is IDisposable disposable )
						disposable.Dispose();
				}

				this.instancesDiff.Clear();
			}
		}
	}
}