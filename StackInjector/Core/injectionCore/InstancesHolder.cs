using System;
using System.Collections.Generic;
using System.Linq;

namespace StackInjector.Core
{
    internal class InstancesHolder : Dictionary<Type,LinkedList<object>>
    {

        internal IEnumerable<Type> TypesAssignableFrom ( Type type )
            => this
                .Keys
                .Where(t => type.IsAssignableFrom(t));


        //? would it actually work
        internal IEnumerable<object> InstancesAssignableFrom ( Type type )
            => this
                .Where(pair => type.IsAssignableFrom(pair.Key) && pair.Value.Any())
                .SelectMany(pair => pair.Value);


        internal void AddType ( Type type )
        {
            this.TryAdd(type, new LinkedList<object>());
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
