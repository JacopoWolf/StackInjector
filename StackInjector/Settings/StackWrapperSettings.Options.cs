using System;
using System.Collections.Generic;
using System.Text;

namespace StackInjector.Settings
{
	partial class StackWrapperSettings
	{
		/// <summary>
		/// base class for StackWrapperSettings options
		/// </summary>
		public interface IOptions : ICloneable
		{
			protected internal IOptions CreateDefault ();
			
		}

	}
}
