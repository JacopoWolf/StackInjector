using System;
using System.Collections.Generic;

namespace StackInjector.Behaviours
{
    //? should this be documented internally
    internal interface IInstancesHolder
    {
        IEnumerable<object> OfType ( Type type );

        IEnumerable<object> InstanceAssignableFrom ( Type type );

        IEnumerable<Type> TypesAssignableFrom ( Type type );

        IEnumerable<Type> GetAllTypes ();


        void AddType ( Type type );

        void AddInstance ( Type type, object instance );

        void RemoveInstance ( Type type, object instance );

        bool ContainsType ( Type type );

    }
}
