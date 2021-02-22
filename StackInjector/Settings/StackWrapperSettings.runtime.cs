using System;
using System.Collections.Generic;
using System.Text;

namespace StackInjector.Settings
{
	partial class StackWrapperSettings
	{
		public sealed class Runtime : IOptions
		{

			// async management
			internal AsyncWaitingMethod                 _asyncWaitingMethod                     = AsyncWaitingMethod.Exit;
			internal int                                _asyncWaitTime                          = 500;


			internal Runtime () { }

			public static Runtime Default => new Runtime();

			IOptions IOptions.CreateDefault () => Default;



			/// <summary>
			/// What to do when an <see cref="Wrappers.IAsyncStackWrapper{TEntry, TIn, TOut}"/> 
			/// has no more pending tasks to execute
			/// </summary>
			/// <param name="waitingMethod">the new waiting method</param>
			/// <param name="waitTime">if <see cref="AsyncWaitingMethod.Timeout"/> is set, this will be max time to wait</param>
			/// <returns>the modified settings</returns>
			public  Runtime WhenNoMoreTasks ( AsyncWaitingMethod waitingMethod, int waitTime = 1000 )
			{
				this._asyncWaitingMethod = waitingMethod;
				this._asyncWaitTime = waitTime;
				return this;
			}

			public object Clone () => this.MemberwiseClone();
		}
	}
}
