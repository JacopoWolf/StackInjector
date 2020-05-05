using System;
using System.Collections.Generic;
using System.Linq;
using StackInjector.Behaviours;

namespace StackInjector.Behaviours
{
    internal class SingleInstanceHolder : Dictionary<Type, object>, IInstancesHolder
    {
        public void AddType ( Type type ) 
            => this[type] = null;

        public void AddInstance ( Type type, object instance ) 
            => this[type] = instance;

        public object FirstOfType ( Type type ) 
            => this[type];

        public IEnumerable<object> InheritingFrom ( Type type )
            =>  this
                    .Where(p => type.IsAssignableFrom(p.Key))
                    .Select(p => p.Value);

        public bool ContainsType ( Type type ) 
            => this.ContainsKey(type);

        public IEnumerable<Type> GetTypes ()
            => this.Keys;
    }
}