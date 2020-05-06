﻿using System;

namespace StackInjector.Exceptions
{
    /// <summary>
    /// thrown when an implementation for a specific class has not been found
    /// </summary>
    public sealed class ImplementationNotFoundException : ClassException
    {
        internal ImplementationNotFoundException ()
        {
        }

        internal ImplementationNotFoundException ( Type type, string message ) : base(type, message)
        {

        }

        internal ImplementationNotFoundException ( string message ) : base(message)
        {
        }

        internal ImplementationNotFoundException ( string message, Exception innerException ) : base(message, innerException)
        {
        }
    }
}
