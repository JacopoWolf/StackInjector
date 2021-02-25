using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace StackInjector.Core
{
	internal class InstancesHolder : Dictionary<Type, LinkedList<object>>
	{
		internal uint total_count = 0;

		internal IEnumerable<Type> TypesAssignableFrom ( Type type ) =>
			this
			.Keys
			.Where(t => type.IsAssignableFrom(t));
		

		internal IEnumerable<object> InstancesAssignableFrom ( Type type ) =>
			this
			.Where(pair => type.IsAssignableFrom(pair.Key) && pair.Value.Any())
			.SelectMany(pair => pair.Value);
		

		internal bool AddType ( Type type ) =>
			this.TryAdd(type, new LinkedList<object>() );
		


		internal void CountAllInstances ()
		{
			this.total_count = 0;
			foreach (var pair in this)
				this.total_count += (uint)pair.Value.Count;
		}


		// clones just the structure, the classes references are not cloned
		internal InstancesHolder CloneStructure ()
		{
			var clonedHolder = new InstancesHolder();

			foreach( var t in this.Keys )
				clonedHolder.AddType(t);

			return clonedHolder;

		}
	}
}
