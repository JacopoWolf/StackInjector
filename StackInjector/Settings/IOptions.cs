using System;

namespace StackInjector.Settings
{

	/// <summary>
	/// Base class for <see cref="StackWrapperSettings"/> options.<br/>
	/// Options groups are a class of settings grouped by function/scope of use.
	/// </summary>
	public interface IOptions : ICloneable
	{
		/// <summary>
		/// Create a default instance of this options group valorized with default values.<br/>
		/// Default values may differ from uninitialized ones.
		/// </summary>
		/// <returns></returns>
		protected internal IOptions CreateDefault ();
			
	}

	
}
