using System;

namespace StackInjector.Exceptions
{
    /// <summary>
    /// thrown when the specified class is NOT a [Service]
    /// </summary>
    public class NotAServiceException : ClassException
    {
        internal NotAServiceException ()
        {

        }

        internal NotAServiceException ( Type type, string message ) : base(type, message)
        {

        }

        internal NotAServiceException ( string message ) : base(message)
        {
        }

        internal NotAServiceException ( string message, Exception innerException ) : base(message, innerException)
        {
        }
    }
}
