using System;

namespace StackInjector.Exceptions
{
    /// <summary>
    /// Thrown when the specified type has a field/property withouth a setter
    /// </summary>
    public class NoSetterException : StackInjectorException
    {
        internal NoSetterException () { }

        internal NoSetterException ( Type type, string message ) : base(type, message) { }

        internal NoSetterException ( string message ) : base(message) { }

        internal NoSetterException ( string message, Exception innerException ) : base(message, innerException) { }
    }
}
