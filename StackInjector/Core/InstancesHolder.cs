using System;
using System.Collections.Generic;
using System.Linq;

namespace StackInjector.Core
{
	internal class InstancesHolder : Dictionary<Type, LinkedList<object>>
	{
		internal int total_count; // 0

		internal IEnumerable<Type> TypesAssignableFrom ( Type type )
		{
			return this
				.Keys
				.Where(t => type.IsAssignableFrom(t));
		}

		internal IEnumerable<object> InstancesAssignableFrom ( Type type )
		{
			return this
				.Where(pair => type.IsAssignableFrom(pair.Key) && pair.Value.Any())
				.SelectMany(pair => pair.Value);
		}

		internal bool AddType ( Type type )
		{
			return this.TryAdd(type, new LinkedList<object>());
		}

		internal int CountAllInstances ()
		{
			this.total_count = 0;
			foreach ( var pair in this )
				this.total_count += pair.Value.Count;
			return this.total_count;
		}


		// clones just the structure, the classes references are not cloned
		internal InstancesHolder CloneStructure ()
		{
			var clonedHolder = new InstancesHolder();

			foreach ( var t in this.Keys )
				clonedHolder.AddType(t);

			return clonedHolder;

		}
	}
}
