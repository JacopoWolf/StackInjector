using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StackInjector.Core
{
	internal abstract partial class AsyncStackWrapperCore<T> : StackWrapperCore, IAsyncStackWrapperCore<T>
	{

		public event EventHandler<AsyncElaboratedEventArgs<T>> OnElaborated;


		// used to cancel everything
		protected internal readonly CancellationTokenSource cancelPendingTasksSource = new CancellationTokenSource();

		// exposes the token
		public CancellationToken PendingTasksCancellationToken
			=> this.cancelPendingTasksSource.Token;

		// used to lock access to tasks
		private readonly object _listAccessLock = new object();

		// used to endure Elaborated() and Elaborate() are called together
		private bool _exclusiveExecution;

		public bool IsElaborating => this._exclusiveExecution;

		// asyncronously waited for new events if TaskList is empty
		private readonly SemaphoreSlim _emptyListAwaiter = new SemaphoreSlim(0);

		// pending tasks
		protected internal LinkedList<Task<T>> tasks = new LinkedList<Task<T>>();


		// register an event that in case the list is empty, release the empty event listener.
		internal AsyncStackWrapperCore ( InjectionCore core, Type toRegister ) : base(core, toRegister)
		{
			this.cancelPendingTasksSource.Token.Register(this.ReleaseListAwaiter);
		}



		#region IDisposable Support

		private bool disposedValue;

		public override void Dispose ()
		{
			if ( !this.disposedValue )
			{

				// managed resources
				this.cancelPendingTasksSource.Cancel(); // cancel all pending tasks
				this.ReleaseListAwaiter();  // in case it's waiting on the empty list

				this.cancelPendingTasksSource.Dispose();
				this._emptyListAwaiter.Dispose();


				// big objects
				this.tasks.Clear();
				this.tasks = null;

				// clean instantiated objects
				this.Core.RemoveInstancesDiff();


				this.disposedValue = true;
			}
		}
		#endregion
	}

}