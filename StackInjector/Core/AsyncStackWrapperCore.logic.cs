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
            this.emptyListAwaiter.Release();
        }

        public void Submit ( Task<T> work )
        {
            lock( this.listAccessLock )
                this.tasks.AddLast(work);

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
            while( !this.cancelPendingTasksSource.IsCancellationRequested )
            {
                // avoid deadlocks 
                if( this.AnyTaskLeft() )
                {
                    var completed = await Task.WhenAny(this.tasks).ConfigureAwait(false);

                    lock( this.listAccessLock )
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
        }

        // true if outher loop is to break
        private async Task<bool> OnNoTasksLeft ()
        {
            // to not repeat code
            Task listAwaiter ()
                => this.emptyListAwaiter.WaitAsync();


            switch( this.Settings.asyncWaitingMethod )
            {

                case AsyncWaitingMethod.Exit:
                default:

                    return true;


                case AsyncWaitingMethod.Wait:

                    // wait for a signal of the list not being empty anymore
                    await listAwaiter().ConfigureAwait(true);
                    return false;


                case AsyncWaitingMethod.WaitTimeout:
                    var list = listAwaiter();
                    var timeout = Task.Delay( this.Settings.asyncWaitTime );

                    // if the timeout elapses first, then stop waiting
                    return (await Task.WhenAny(list, timeout).ConfigureAwait(true)) == timeout;
            }
        }


        public bool AnyTaskCompleted ()
            =>
                this.tasks.Any(t => t.IsCompleted);


    }
}