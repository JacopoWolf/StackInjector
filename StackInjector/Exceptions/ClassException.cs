using System;

namespace StackInjector.Exceptions
{
    /// <summary>
    /// base for all class and type related exceptions
    /// </summary>
    public abstract class ClassException : StackInjectorException
    {
        /// <summary>
        /// type of the class not found
        /// </summary>
        public Type ClassType { get; set; } = null;



        internal ClassException() { }

        internal ClassException ( Type type, string message ) : this( message )
        {
            this.ClassType = type;
            this.SourceAssembly = type.Assembly;
        }

        internal ClassException ( string message ) : base(message)
        {
        }

        internal ClassException ( string message, Exception innerException ) : base(message, innerException)
        {
        }


    }
}
