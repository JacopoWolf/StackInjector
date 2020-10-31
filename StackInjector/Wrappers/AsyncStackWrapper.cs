using System.Linq;
using System.Threading.Tasks;
using StackInjector.Attributes;
using StackInjector.Core;
using StackInjector.Settings;

namespace StackInjector.Wrappers
{
    [Service(Version = 3.0, Serving = ServingMethods.DoNotServe)]
    internal class AsyncStackWrapper<TEntry, TIn, TOut> : AsyncStackWrapperCore<TOut>, IAsyncStackWrapper<TEntry, TIn, TOut>
    {

        internal AsyncStackDigest<TEntry, TIn, TOut> StackDigest { private get; set; }

        public TEntry Entry
            =>
                this.Core.GetEntryPoint<TEntry>();


        public AsyncStackWrapper ( InjectionCore core ) : base(core, typeof(AsyncStackWrapper<TEntry, TIn, TOut>))
        { }

        public Task<TOut> Submit ( TIn item )
        {
            var task = this.StackDigest.Invoke
                    (
                        this.Entry,
                        item,
                        this.PendingTasksCancellationToken
                    );

            this.Submit( task );
            return task;
        }

        public override string ToString ()
            =>
                $"AsyncStackWrapper<{typeof(TEntry).Name},{typeof(TIn).Name},{typeof(TOut).Name}>" +
                $"{{ {this.Core.instances.Count} registered types; " +
                $"canceled: {this.cancelPendingTasksSource.IsCancellationRequested} }}";

    }
}
