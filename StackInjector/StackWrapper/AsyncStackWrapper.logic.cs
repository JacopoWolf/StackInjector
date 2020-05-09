using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackInjector
{

    internal partial class AsyncStackWrapper
    {

        ///<inheritdoc/>
        public void Submit ( object submitted )
        {
            var task = this.GetAsyncEntryPoint().Digest(submitted,this.cancelPendingTasksSource.Token);


            lock( this.listAccessLock )
                this.tasks.AddLast(task);


            // if the list was empty just an item ago, signal it's not anymore.
            // this limit avoids useless cross thread calls that would slow everything down.
            if( this.tasks.Count == 1 )
                this.ReleaseListAwaiter();
        }



        /// <inheritdoc/>
        public async IAsyncEnumerable<T> Elaborated<T> ()
        {
            while( !this.cancelPendingTasksSource.IsCancellationRequested )
            {
                // avoid deadlocks 
                if( this.tasks.Any() )
                {
                    var completed = await Task.WhenAny(this.tasks).ConfigureAwait(false);

                    lock( this.listAccessLock )
                        this.tasks.Remove(completed);

                    yield return (T)completed.Result;
                }
                else
                {
                    // wait for a signal of the list not being empty anymore
                    await this.emptyListAwaiter.WaitAsync().ConfigureAwait(true);
                    continue;
                }
            }
        }


        /// <summary>
        /// gets the entry point for this stack
        /// </summary>
        /// <returns></returns>
        internal IAsyncStackEntryPoint GetAsyncEntryPoint ()
        {
            return
                (IAsyncStackEntryPoint)
                this
                    .ServicesWithInstances
                    .OfType
                    (
                        this.ClassOrFromInterface(this.EntryPoint)
                    )
                    .First();
        }

    }
}
