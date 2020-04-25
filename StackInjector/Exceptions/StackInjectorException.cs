using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace StackInjector.Exceptions
{
    /// <summary>
    /// Base class for all 
    /// </summary>
    public abstract class StackInjectorException : Exception
    {
        /// <summary>
        /// the assembly who thew this exception
        /// </summary>
        public Assembly SourceAssembly { get; private protected set; } = null;


        internal StackInjectorException ( Assembly assembly, string message ) : this(message)
        {
            this.SourceAssembly = assembly;
        }

        internal StackInjectorException() : base( ) { }

        internal StackInjectorException ( string message ) : base(message) { }

        internal StackInjectorException ( string message, Exception inner ) : base(message, inner) { }
    }
}
