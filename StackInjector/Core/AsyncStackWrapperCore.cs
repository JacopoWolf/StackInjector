using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StackInjector.Wrappers;

namespace StackInjector.Core
{
    internal abstract partial class AsyncStackWrapperCore<T> : AsyncStackWrapperCore, IAsyncStackWrapperCore<T>
    {

        // used to cancel everything
        protected internal readonly CancellationTokenSource cancelPendingTasksSource = new CancellationTokenSource();

        // exposes the token
        public CancellationToken PendingTasksCancellationToken 
            => this.cancelPendingTasksSource.Token;

        // used to lock access to tasks
        protected internal readonly object listAccessLock = new object();

        // asyncronously waited for new events if TaskList is empty
        protected internal readonly SemaphoreSlim emptyListAwaiter = new SemaphoreSlim(0);

        // pending tasks
        protected internal LinkedList<Task<T>> tasks = new LinkedList<Task<T>>();


        internal AsyncStackWrapperCore ( WrapperCore core, Type toRegister ) : base(core, toRegister)
        {
            // register an event that in case the list is empty, release the empty event listener.
            this.cancelPendingTasksSource.Token.Register(this.ReleaseListAwaiter);
        }



        #region IDisposable Support

        private bool disposedValue = false;

        public override void Dispose ()
        {
            if( !this.disposedValue )
            {

                // managed resources
                this.cancelPendingTasksSource.Cancel();
                this.ReleaseListAwaiter();  // in case it's waiting on the empty list

                this.cancelPendingTasksSource.Dispose();
                this.emptyListAwaiter.Dispose();


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
