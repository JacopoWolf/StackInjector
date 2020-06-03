using System;
using System.Collections.Generic;

namespace StackInjector.Behaviours
{
    internal interface IInstancesHolder
    {
        IEnumerable<Type> AllTypes ();

        IEnumerable<object> OfType ( Type type );

        IEnumerable<Type> TypesAssignableFrom ( Type type );

        IEnumerable<object> InstancesAssignableFrom ( Type type );



        void AddType ( Type type );

        void AddInstance ( Type type, object instance );

        void RemoveInstance ( Type type, object instance );


        bool ContainsType ( Type type );


        bool IsInjected ( object instance );

        void SetInjectionStatus ( object instance, bool injected = true );



        IInstancesHolder CloneStructure ();
    }
}
