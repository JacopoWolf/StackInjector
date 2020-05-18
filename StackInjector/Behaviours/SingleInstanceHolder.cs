using System;
using System.Collections.Generic;
using System.Linq;

namespace StackInjector.Behaviours
{
    internal class SingleInstanceHolder : Dictionary<Type, object>, IInstancesHolder
    {
        public void AddType ( Type type )
            => this[type] = null;

        public void AddInstance ( Type type, object instance )
            => this[type] = instance;

        public void RemoveInstance ( Type type, object instance )
            => this[type] = null;

        public IEnumerable<object> OfType ( Type type )
            => new object[] { this[type] };

        public IEnumerable<object> InstanceAssignableFrom ( Type type )
            => this
                    .Where(p => type.IsAssignableFrom(p.Key))
                    .Select(p => p.Value);

        public IEnumerable<Type> TypesAssignableFrom ( Type type )
            => this
                    .Keys
                    .Where(p => type.IsAssignableFrom(p));

        public bool ContainsType ( Type type )
            => this.ContainsKey(type);

        public IEnumerable<Type> GetAllTypes ()
            => this.Keys;

    }

}