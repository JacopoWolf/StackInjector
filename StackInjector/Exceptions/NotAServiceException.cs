using System;

namespace StackInjector.Exceptions
{
	/// <summary>
	/// Thrown when the specified class is NOT a marked with <see cref="Attributes.ServiceAttribute"/>.
	/// </summary>
	public class NotAServiceException : StackInjectorException
	{
		internal NotAServiceException () { }

		internal NotAServiceException ( Type type, string message ) : base(type, message) { }

		internal NotAServiceException ( string message ) : base(message) { }

		internal NotAServiceException ( string message, Exception innerException ) : base(message, innerException) { }
	}
}
