using System;

namespace StackInjector.Settings
{
	/// <summary>
	/// Options used at runtime
	/// </summary>
	public sealed class RuntimeOptions : IOptions
	{

		// async management
		internal int                                _asyncWaitTime; // 0


		internal RuntimeOptions () { }


		IOptions IOptions.CreateDefault ()
		{
			return Default;
		}

		/// <inheritdoc/>
		public object Clone ()
		{
			return this.MemberwiseClone();
		}


		/// <summary>
		/// Default runtime options. Settings are valorized as following:
		/// <list type="table">
		/// <item>
		///		<term><see cref="WaitTimeout(int)"/></term>
		///		<description>-1</description>
		/// </item>
		/// </list>
		/// </summary>
		public static RuntimeOptions Default => new RuntimeOptions();


		#region configuration methods

		/// <summary>
		/// What to do when an <see cref="Wrappers.IAsyncStackWrapper{TEntry, TIn, TOut}"/> 
		/// has no more pending tasks to execute
		/// </summary>
		/// <param name="waitTime">this will be max time to wait. <br/>
		///		-1: wait forever<br/>
		///		 0: exit immediatly<br/>
		///		+n: wait specified timeout (in ms)<br/> </param>
		/// <returns>the modified settings</returns>
		public RuntimeOptions WaitTimeout ( int waitTime = 0 )
		{
			if ( waitTime < -1 )
				throw new ArgumentOutOfRangeException(nameof(waitTime), $"{waitTime} cannot be below -1!");

			this._asyncWaitTime = waitTime;
			return this;
		}

		#endregion

	}
}
