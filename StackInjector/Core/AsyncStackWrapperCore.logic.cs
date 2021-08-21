using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
			lock ( this._listAccessLock )
				this.tasks.AddLast(work);

			// if the list was empty just an item ago, signal it's not anymore.
			// this limit avoids useless cross thread calls that would slow everything down.
			if ( this.tasks.Count == 1 )
				this.ReleaseListAwaiter();
		}


		public bool AnyTaskLeft ()
		{
			lock ( this._listAccessLock )
				return this.tasks.Any();
		}

		public bool AnyTaskCompleted ()
		{
			lock ( this._listAccessLock )
				return this.tasks.Any(t => t.IsCompleted);
		}

		public async IAsyncEnumerable<T> Elaborated ()
		{
			// begin elaboration
			lock ( this._listAccessLock )
			{
				if ( this._exclusiveExecution )
					throw new InvalidOperationException();
				this._exclusiveExecution = true;
			}

			while ( !this.cancelPendingTasksSource.IsCancellationRequested )
			{
				// avoid deadlocks 
				if ( this.AnyTaskLeft() )
				{
					var completed = await Task.WhenAny(this.tasks).ConfigureAwait(false);

					lock ( this._listAccessLock )
						this.tasks.Remove(completed);

					yield return completed.Result;
					continue;
				}
				else
				{
					if ( await this.OnNoTasksLeft().ConfigureAwait(true) )
						break;
				}
			}

			// no more elaborating
			lock ( this._listAccessLock )
			{
				this._exclusiveExecution = false;
			}

		}

		public async Task Elaborate ()
		{
			await foreach ( var res in this.Elaborated() )
				this.OnElaborated?.Invoke(this, new AsyncElaboratedEventArgs<T>(res));
		}


		// true if outher loop is to break
		private async Task<bool> OnNoTasksLeft ()
		{

			if ( this.Settings.Runtime._asyncWaitTime == 0 )
				return true;

			return !(await this._emptyListAwaiter.WaitAsync(
							this.Settings.Runtime._asyncWaitTime,
							this.PendingTasksCancellationToken
						)
						.ConfigureAwait(true));
		}
	}
}
