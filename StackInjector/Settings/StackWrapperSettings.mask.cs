using System;
using System.Collections.Generic;
using System.Text;

namespace StackInjector.Settings
{
	partial class StackWrapperSettings
	{
		public sealed class Mask : HashSet<Type>
		{
			internal bool isWhiteList;

			private Mask () { }


			public Mask Register ( params Type[] types )
			{
				foreach ( var t in types )
					base.Add(t);
				return this;
			}


			public bool IsMasked ( Type type )
			{
				if ( isWhiteList )
					return this.Contains(type);
				else
					return !this.Contains(type);

			}



			public static Mask WhiteList => new Mask() { isWhiteList = true };

			public static Mask BlackList => new Mask();


		}
	}
}
