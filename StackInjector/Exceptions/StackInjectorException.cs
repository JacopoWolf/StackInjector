using System;

namespace StackInjector.Exceptions
{
    /// <summary>
    /// Base class for every StackWrapper exception
    /// </summary>
    public abstract class StackInjectorException : Exception
    {
        /// <summary>
        /// The source type of the exception
        /// </summary>
        public Type SourceType { get; private protected set; }

        internal StackInjectorException ( Type type, string message ) : this(message)
        {
            this.SourceType = type;
        }

        internal StackInjectorException () : base() { }

        internal StackInjectorException ( string message ) : base(message) { }

        internal StackInjectorException ( string message, Exception inner ) : base(message, inner) { }
    }
}
