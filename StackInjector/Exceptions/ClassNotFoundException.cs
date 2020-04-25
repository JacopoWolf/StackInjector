using System;

namespace StackInjector.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class ClassNotFoundException : StackInjectorException
    {
        /// <summary>
        /// type of the class not found
        /// </summary>
        public Type ClassType { get; set; } = null;



        internal ClassNotFoundException() { }

        internal ClassNotFoundException ( Type type, string message ) : this( message )
        {
            this.ClassType = type;
            this.SourceAssembly = type.Assembly;
        }

        internal ClassNotFoundException ( string message ) : base(message)
        {
        }

        internal ClassNotFoundException ( string message, Exception innerException ) : base(message, innerException)
        {
        }


    }
}
