using System;
using System.Collections.Generic;
using System.Text;

namespace StackInjector.Behaviours
{
    internal interface IInstancesHolder
    {
        IEnumerable<object> OfType ( Type type );

        IEnumerable<object> InstanceAssignableFrom ( Type type );

        IEnumerable<Type> TypesAssignableFrom ( Type type );

        IEnumerable<Type> GetAllTypes ();


        void AddType ( Type type );

        void AddInstance ( Type type, object instance );


        bool ContainsType ( Type type );

    }
}
