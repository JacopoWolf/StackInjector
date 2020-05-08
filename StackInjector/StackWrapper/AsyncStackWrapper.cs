using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StackInjector.Attributes;
using StackInjector.Settings;

namespace StackInjector
{
    [Service(DoNotServeMembers = true, Version = 2.0)]
    internal partial class AsyncStackWrapper : StackWrapper, IAsyncStackWrapper
    {

        public CancellationTokenSource CancelPendingTasks { get; private set; } = new CancellationTokenSource();


        // used to lock access to tasks
        private readonly object listAccessLock = new object();

        // asyncronously waited for new events if TaskList is empty
        private readonly SemaphoreSlim emptyListAwaiter = new SemaphoreSlim(0);

        private LinkedList<Task<object>> tasks = new LinkedList<Task<object>>();



        /// <summary>
        /// create a new AsyncStackWrapper
        /// </summary>
        /// <param name="settings"></param>
        internal AsyncStackWrapper( StackWrapperSettings settings ) : base(settings)
        {
            // in case the list is empty, release the empty event listener.
            this.CancelPendingTasks.Token.Register( this.ReleaseListAwaiter );
        
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
                $"entry point: {this.EntryPoint.Name}; canceled: {this.CancelPendingTasks.IsCancellationRequested} }}";



        #region IDisposable Support

        private bool disposedValue = false;

        public void Dispose (  )
        {
            if( !this.disposedValue )
            {

                // managed resources
                this.CancelPendingTasks.Cancel();
                this.CancelPendingTasks.Dispose();
                
                // big objects
                this.tasks.Clear();
                this.tasks = null;


                this.disposedValue = true;
            }
        }

        #endregion

    }
}