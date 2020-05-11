using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StackInjector.Attributes;
using StackInjector.Settings;

namespace StackInjector
{
    [Service(DoNotServeMembers = true, Version = 2.0)]
    internal partial class AsyncStackWrapper : StackWrapper, IAsyncStackWrapper
    {
        // used to cancel everything
        private readonly CancellationTokenSource cancelPendingTasksSource = new CancellationTokenSource();

        // exposes the token
        public CancellationToken CancelPendingTasksToken { get => this.cancelPendingTasksSource.Token; }

        // used to lock access to tasks
        private readonly object listAccessLock = new object();

        // asyncronously waited for new events if TaskList is empty
        private readonly SemaphoreSlim emptyListAwaiter = new SemaphoreSlim(0);

        // pending tasks
        private LinkedList<Task<object>> tasks = new LinkedList<Task<object>>();



        /// <summary>
        /// create a new AsyncStackWrapper
        /// </summary>
        /// <param name="settings"></param>
        internal AsyncStackWrapper ( StackWrapperSettings settings ) : base(settings)
        {
            // in case the list is empty, release the empty event listener.
            this.cancelPendingTasksSource.Token.Register(this.ReleaseListAwaiter);

        }


        /// <summary>
        /// call the semaphore
        /// </summary>
        private void ReleaseListAwaiter ()
            =>
                this.emptyListAwaiter.Release();



        public override string ToString ()
            =>
                $"AsyncStackWrapper{{ {this.ServicesWithInstances.GetAllTypes().Count()} registered types; " +
                $"entry point: {this.EntryPoint.Name}; canceled: {this.cancelPendingTasksSource.IsCancellationRequested} }}";



        #region IDisposable Support

        private bool disposedValue = false;

        public new void Dispose ()
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
                this.RemoveInstancesDiff();


                this.disposedValue = true;
            }
        }

        #endregion

    }
}