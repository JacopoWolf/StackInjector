using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StackInjector.Settings;

namespace StackInjector
{

    internal class AsyncStackWrapper : StackWrapper, IAsyncStackWrapper
    {
        
        public CancellationTokenSource CancelEveything { get; internal set; } = new CancellationTokenSource();

        internal Type TargetType { get; set; } //! unused

        private List<Task<object>> TaskList { get; set; } = new List<Task<object>>();



        internal AsyncStackWrapper( StackWrapperSettings settings ) : base(settings)
        {
            // in case the list is empty, release the empty event listener.
            this.CancelEveything.Token.Register( this.ReleaseListAwaiter );
        
        }

        private void ReleaseListAwaiter ()
            =>
                this.EmptyListAwaiter.Release();


        // asyncronously waited for new events if TaskList is empty
        private SemaphoreSlim EmptyListAwaiter = new SemaphoreSlim(0);

        object ListAccessLock = new object();

        ///<inheritdoc/>
        public void Submit ( object submitted )
        {
            var task = Task.Run
                (
                    () => this.GetAsyncEntryPoint().Digest(submitted),
                    this.CancelEveything.Token
                );


            lock( this.ListAccessLock )
                this.TaskList.Add(task);


            // if the list was empty just an item ago, signal it's not anymore.
            // this limit avoids useless cross thread calls that would slow everything down.
            if( this.TaskList.Count == 1 )
                this.ReleaseListAwaiter();
        }

        


        public async IAsyncEnumerable<T> Elaborated<T> ()
        {
            while( !this.CancelEveything.IsCancellationRequested )
            {
                // avoid deadlocks 
                if( this.TaskList.Any() )
                {
                    var completed = await Task.WhenAny(this.TaskList).ConfigureAwait(false);

                    lock( this.ListAccessLock )
                        this.TaskList.Remove(completed);

                    yield return (T)completed.Result;
                }
                else
                {
                    // wait for a signal of the list not being empty anymore
                    await this.EmptyListAwaiter.WaitAsync().ConfigureAwait(true);
                    continue;
                }
            }
        }

        
        /// <summary>
        /// gets the entry point for this stack
        /// </summary>
        /// <returns></returns>
        internal IAsyncStackEntryPoint GetAsyncEntryPoint()
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
