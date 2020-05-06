using System;

namespace StackInjector.Exceptions
{
    /// <summary>
    /// Thrown when a class has not been found
    /// </summary>
    public sealed class ClassNotFoundException : ClassException
    {

        internal ClassNotFoundException ( Type type, string message ) : base(type, message)
        {

        }

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
