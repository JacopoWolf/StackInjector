using System;

namespace StackInjector.Exceptions
{
	/// <summary>
	/// Thrown when a class has no parameterless constructor to call.
	/// </summary>
	public sealed class InvalidConstructorException : StackInjectorException
	{
		internal InvalidConstructorException () { }

		internal InvalidConstructorException ( Type type, string message ) : base(message)
		{
			this.SourceType = type;
		}

		internal InvalidConstructorException ( string message ) : base(message) { }

		internal InvalidConstructorException ( string message, Exception innerException ) : base(message, innerException) { }

	}
}
