using System;
using System.Collections.Generic;
using System.Text;

namespace StackInjector.Behaviours
{
    internal interface IInstancesHolder
    {
        object FirstOfType ( Type type );

        IEnumerable<object> InheritingFrom ( Type type );

        IEnumerable<Type> GetTypes ();


        void AddType ( Type type );

        void AddInstance ( Type type, object instance );


        bool ContainsType ( Type type );

    }
}
