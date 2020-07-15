using System;

namespace StackInjector.Exceptions
{
    /// <summary>
    /// Thrown when a class has no parameterless constructor to call.
    /// </summary>
    public class MissingParameterlessConstructorException : StackInjectorException
    {
        internal MissingParameterlessConstructorException () { }

        internal MissingParameterlessConstructorException ( Type type, string message, MissingMethodException inner ) : base(message, inner)
        {
            this.SourceType = type;
        }

        internal MissingParameterlessConstructorException ( string message ) : base(message) { }

        internal MissingParameterlessConstructorException ( string message, Exception innerException ) : base(message, innerException) { }

    }
}
