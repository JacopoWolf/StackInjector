using System;

namespace StackInjector.Exceptions
{
    /// <summary>
    /// Thrown when the entry point is invalid
    /// </summary>
    public class InvalidEntryTypeException : ClassException
    {
        internal InvalidEntryTypeException ()
        {

        }

        internal InvalidEntryTypeException ( Type type, string message ) : base(type, message)
        {

        }

        internal InvalidEntryTypeException ( string message ) : base(message)
        {

        }

        internal InvalidEntryTypeException ( string message, Exception innerException ) : base(message, innerException)
        {

        }

    }

}
