using System;

namespace StackInjector.Exceptions
{

	/// <summary>
	/// Thrown when the number of instances is greater than the one in the settings/>
	/// </summary>
	public sealed class InstancesLimitReachedException : StackInjectorException
	{
		internal InstancesLimitReachedException ()
		{
		}

		internal InstancesLimitReachedException ( string message ) : base(message)
		{
		}

		internal InstancesLimitReachedException ( string message, Exception innerException ) : base(message, innerException)
		{
		}
	}
}