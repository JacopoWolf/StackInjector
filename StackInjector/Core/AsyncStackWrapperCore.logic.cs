using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackInjector.Settings;

namespace StackInjector.Core
{
	internal abstract partial class AsyncStackWrapperCore<T>
	{
		// call the semaphore
		protected internal void ReleaseListAwaiter ()
		{
			this._emptyListAwaiter.Release();
		}

		internal void Submit ( Task<T> work )
		{
			lock( this._listAccessLock )
				this.tasks.AddLast(work);

			// if the list was empty just an item ago, signal it's not anymore.
			// this limit avoids useless cross thread calls that would slow everything down.
			if( this.tasks.Count == 1 )
				this.ReleaseListAwaiter();
		}


		public bool AnyTaskLeft ()
		{
			lock( this._listAccessLock )
				return this.tasks.Any();
		}

		public bool AnyTaskCompleted ()
		{
			lock( this._listAccessLock )
				return this.tasks.Any(t => t.IsCompleted);
		}

		public async IAsyncEnumerable<T> Elaborated ()
		{
			this.EnsureExclusiveExecution(true);

			while( !this.cancelPendingTasksSource.IsCancellationRequested )
			{
				// avoid deadlocks 
				if( this.AnyTaskLeft() )
				{
					var completed = await Task.WhenAny(this.tasks).ConfigureAwait(false);



					lock( this._listAccessLock )
						this.tasks.Remove(completed);

					yield return completed.Result;
					continue;
				}
				else
				{
					if( await this.OnNoTasksLeft().ConfigureAwait(true) )
						break;
				}
			}

			lock( this._listAccessLock )
				this._exclusiveExecution = false;

		}

		public async Task Elaborate ()
		{
			await foreach( var res in this.Elaborated() )
				this.OnElaborated?.Invoke(res);
		}


		// true if outher loop is to break
		private async Task<bool> OnNoTasksLeft ()
		{
			// to not repeat code
			Task listAwaiter ()
			{
				return this._emptyListAwaiter.WaitAsync();
			}

			switch( this.Settings.RuntimeOptions._asyncWaitingMethod )
			{

				case AsyncWaitingMethod.Exit:
				default:

					return true;


				case AsyncWaitingMethod.Wait:

					// wait for a signal of the list not being empty anymore
					await listAwaiter().ConfigureAwait(true);
					return false;


				case AsyncWaitingMethod.Timeout:
					var list = listAwaiter();
					var timeout = Task.Delay( this.Settings.RuntimeOptions._asyncWaitTime );

					// if the timeout elapses first, then stop waiting
					return (await Task.WhenAny(list, timeout).ConfigureAwait(true)) == timeout;
			}
		}

		private void EnsureExclusiveExecution ( bool set = false )
		{
			lock( this._listAccessLock ) // reused lock
			{
				if( this._exclusiveExecution )
					throw new InvalidOperationException();

				if( set )
					this._exclusiveExecution = set;
			}
		}



	}
}