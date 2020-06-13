using System;
using System.Reflection;

namespace StackInjector.Exceptions
{
    /// <summary>
    /// Base class for every StackWrapper exception
    /// </summary>
    public abstract class StackInjectorException : Exception
    {
        /// <summary>
        /// The source class of the exception
        /// </summary>
        public Type ClassType { get; private protected set; } = null;

        internal StackInjectorException ( Type assembly, string message ) : this(message)
        {
            this.ClassType = assembly;
        }

        internal StackInjectorException () : base() { }

        internal StackInjectorException ( string message ) : base(message) { }

        internal StackInjectorException ( string message, Exception inner ) : base(message, inner) { }
    }
}
