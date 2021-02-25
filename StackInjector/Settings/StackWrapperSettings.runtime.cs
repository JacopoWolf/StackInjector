using System;
using System.Collections.Generic;
using System.Text;

namespace StackInjector.Settings
{
	partial class StackWrapperSettings
	{
		/// <summary>
		/// Options used at runtime
		/// </summary>
		public sealed class Runtime : IOption
		{

			// async management
			internal AsyncWaitingMethod                 _asyncWaitingMethod                     = AsyncWaitingMethod.Exit;
			internal int                                _asyncWaitTime                          = 500;


			internal Runtime () { }


			IOption IOption.CreateDefault () => Default;

			/// <inheritdoc/>
			public object Clone () => this.MemberwiseClone();


			/// <summary>
			/// Default runtime options. Settings are valorized as following:
			/// <list type="table">
			/// <item>
			///		<term><see cref="WhenNoMoreTasks(AsyncWaitingMethod, int)"/></term>
			///		<description><see cref="AsyncWaitingMethod.Exit"/>, 500</description>
			/// </item>
			/// </list>
			/// </summary>
			public static Runtime Default => new Runtime();


			#region configuration methods

			/// <summary>
			/// What to do when an <see cref="Wrappers.IAsyncStackWrapper{TEntry, TIn, TOut}"/> 
			/// has no more pending tasks to execute
			/// </summary>
			/// <param name="waitingMethod">the new waiting method</param>
			/// <param name="waitTime">if <see cref="AsyncWaitingMethod.Timeout"/> is set, this will be max time to wait</param>
			/// <returns>the modified settings</returns>
			public Runtime WhenNoMoreTasks ( AsyncWaitingMethod waitingMethod, int waitTime )
			{
				this._asyncWaitingMethod = waitingMethod;
				this._asyncWaitTime = waitTime;
				return this;
			}

			#endregion

		}
	}
}
