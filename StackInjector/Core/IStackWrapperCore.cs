using System;
using System.Collections.Generic;
using StackInjector.Core.Cloning;
using StackInjector.Settings;

namespace StackInjector.Core
{

	/// <summary>
	/// Base interface for all StackWrappers.
	/// </summary>
	public interface IStackWrapperCore : IDisposable, ICloneableCore
	{

		/// <summary>
		/// The settings of this stackwrapper.
		/// </summary>
		ref readonly StackWrapperSettings Settings { get; }


		/// <summary>
		/// Find every service valid for the given class or interface.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		IEnumerable<T> GetServices<T> ();


		/// <summary>
		/// The current number of all tracked services.<br/>
		/// Does also include the Wrapper, so if you want all the instances 
		/// <c>wrapper.CountServices()-1</c>
		/// </summary>
		int CountServices ();

	}
}