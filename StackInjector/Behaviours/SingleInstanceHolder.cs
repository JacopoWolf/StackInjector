using System;
using System.Collections.Generic;
using System.Linq;

namespace StackInjector.Behaviours
{
    internal class SingleInstanceHolder : IInstancesHolder
    {
        private readonly Dictionary<Type,object> objects = new Dictionary<Type, object>();
        private readonly HashSet<object> injected = new HashSet<object>();


        public void AddType ( Type type )
            => this.objects[type] = null;

        public void AddInstance ( Type type, object instance )
            => this.objects[type] = instance;

        public void RemoveInstance ( Type type, object instance )
            => this.objects[type] = null;

        public IEnumerable<object> OfType ( Type type )
            => new object[] { this.objects[type] };

        public IEnumerable<object> InstanceAssignableFrom ( Type type )
            => this.objects
                    .Where(p => type.IsAssignableFrom(p.Key))
                    .Select(p => p.Value);

        public IEnumerable<Type> TypesAssignableFrom ( Type type )
            => this.objects
                    .Keys
                    .Where(p => type.IsAssignableFrom(p));

        public bool ContainsType ( Type type )
            => this.objects.ContainsKey(type);

        public IEnumerable<Type> GetAllTypes ()
            => this.objects.Keys;


        public bool IsInjected ( object instance )
            => this.injected.Contains(instance);

        public void SetInjectionStatus ( object instance, bool injected = true )
        {
            if( injected )
                this.injected.Add(instance);
            else
                this.injected.Remove(instance);
        }
    }

}