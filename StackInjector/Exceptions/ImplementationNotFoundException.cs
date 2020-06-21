using System;

namespace StackInjector.Exceptions
{
    /// <summary>
    /// Thrown when an implementation for an interface has not been found.
    /// </summary>
    public sealed class ImplementationNotFoundException : StackInjectorException
    {
        internal ImplementationNotFoundException () { }

        internal ImplementationNotFoundException ( Type type, string message ) : base(type, message) { }

        internal ImplementationNotFoundException ( string message ) : base(message) { }

        internal ImplementationNotFoundException ( string message, Exception innerException ) : base(message, innerException) { }
    }
}
