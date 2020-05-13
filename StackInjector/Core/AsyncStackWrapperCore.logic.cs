using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackInjector.Core
{
    internal abstract partial class AsyncStackWrapperCore<T>
    {
        // call the semaphore
        protected internal void ReleaseListAwaiter ()
        {
            this.emptyListAwaiter.Release();
        }

        public void Submit ( Task<T> work )
        {
            lock( this.listAccessLock )
                this.tasks.AddLast( work );

            // if the list was empty just an item ago, signal it's not anymore.
            // this limit avoids useless cross thread calls that would slow everything down.
            if( this.tasks.Count == 1 )
                this.ReleaseListAwaiter();
        }

        public bool AnyTaskLeft ()
        {
            lock( this.listAccessLock )
                return this.tasks.Any();
        }

        public async IAsyncEnumerable<T> Elaborated ()
        {
            while( ! this.cancelPendingTasksSource.IsCancellationRequested )
            {
                // avoid deadlocks 
                if( this.tasks.Any() )
                {
                    var completed = await Task.WhenAny(this.tasks).ConfigureAwait(false);

                    lock( this.listAccessLock )
                        this.tasks.Remove(completed);

                    yield return completed.Result;
                    continue;
                }
                else
                {
                    // wait for a signal of the list not being empty anymore
                    await this.emptyListAwaiter.WaitAsync().ConfigureAwait(true);
                    continue;
                }
            }
        }

    }
}