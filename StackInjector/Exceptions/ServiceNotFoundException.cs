using System;

namespace StackInjector.Exceptions
{
	/// <summary>
	/// Thrown when a type for a service has not been found.
	/// </summary>
	public sealed class ServiceNotFoundException : StackInjectorException
	{

		internal ServiceNotFoundException ( Type type, string message ) : base(type, message) { }

		internal ServiceNotFoundException () { }

		internal ServiceNotFoundException ( string message ) : base(message) { }

		internal ServiceNotFoundException ( string message, Exception innerException ) : base(message, innerException) { }
	}
}
