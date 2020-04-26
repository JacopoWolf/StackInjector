using System;
using System.Collections.Generic;
using System.Text;

namespace StackInjector.Exceptions
{
    /// <summary>
    /// Thrown when a class has not been found
    /// </summary>
    public sealed class ClassNotFoundException : ClassException
    {

        internal ClassNotFoundException ()
        {

        }
        internal ClassNotFoundException ( string message ) : base(message)
        {
        }

        internal ClassNotFoundException ( string message, Exception innerException ) : base(message, innerException)
        {
        }
    }
}
