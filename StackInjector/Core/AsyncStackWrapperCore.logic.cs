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

        public bool AnyTaskCompleted ()
        {
            return this.tasks.Any(t => t.IsCompleted);
        }

        // todo add a method to safely exit the await loop to be able to re-join later or maybe an Unloack() of some sort
        //! should check if exiting an await foreach loop and re-entering will not hack the control or lose data
        public async IAsyncEnumerable<T> Elaborated ()
        {
            this.EnsureExclusiveExecution(true);

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

            lock( this.listAccessLock )
                this.exclusiveExecution = false;

        }

        public Task Elaborate ()
        {
            // must run syncronously
            this.EnsureExclusiveExecution();

            return
                Task.Run(async () =>
                {

                    await foreach( var res in this.Elaborated() )
                        this.OnElaborated?.Invoke(res);
                });

        }


        // true if outher loop is to break
        private async Task<bool> OnNoTasksLeft ()
        {
            // to not repeat code
            Task listAwaiter ()
                => this.emptyListAwaiter.WaitAsync();


            switch( this.Settings._asyncWaitingMethod )
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
                    var timeout = Task.Delay( this.Settings._asyncWaitTime );

                    // if the timeout elapses first, then stop waiting
                    return (await Task.WhenAny(list, timeout).ConfigureAwait(true)) == timeout;
            }
        }

        private void EnsureExclusiveExecution ( bool set = false )
        {
            lock( this.listAccessLock ) // reused lock
            {
                if( this.exclusiveExecution )
                    throw new InvalidOperationException();

                if( set )
                    this.exclusiveExecution = set;
            }
        }



    }
}