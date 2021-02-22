using System;
using System.Collections.Generic;
using System.Text;

namespace StackInjector.Settings
{
	partial class StackWrapperSettings
	{
		public sealed class Mask : HashSet<Type>, IOptions
		{
			internal bool _isDisabled;
			internal bool _isWhiteList;

			private Mask () { }

			IOptions IOptions.CreateDefault () => Disabled;



			public Mask Register ( params Type[] types )
			{
				this._isDisabled = false;
				foreach ( var t in types )
					base.Add(t);
				return this;
			}


			public bool IsMasked ( Type type )
			{
				if ( _isWhiteList )
					return !this.Contains(type);
				else
					return this.Contains(type);

			}

			public object Clone () => MemberwiseClone();

			public static Mask WhiteList	=> new Mask() { _isWhiteList = true };

			public static Mask BlackList	=> new Mask();

			public static Mask Disabled		=> new Mask() { _isDisabled = true };





		}
	}
}
