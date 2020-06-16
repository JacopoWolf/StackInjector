using System.Linq;
using StackInjector.Attributes;
using StackInjector.Core;
using StackInjector.Settings;

namespace StackInjector.Wrappers
{
    [Service(Version = 2.1, Serving = ServingMethods.DoNotServe)]
    internal class AsyncStackWrapper<TEntry, TIn, TOut> : AsyncStackWrapperCore<TOut>, IAsyncStackWrapper<TEntry, TIn, TOut>
    {

        public AsyncStackDigest<TEntry, TIn, TOut> StackDigest { get; internal set; }

        public TEntry Entry
            =>
                this.Core.GetEntryPoint<TEntry>();


        public AsyncStackWrapper ( InjectionCore core ) : base(core, typeof(AsyncStackWrapper<TEntry, TIn, TOut>))
        { }

        public void Submit ( TIn item )
        {
            this.Submit
                (
                    this.StackDigest.Invoke
                    (
                        this.Core.GetEntryPoint<TEntry>(),
                        item,
                        this.PendingTasksCancellationToken
                    )
                );
        }

        public override string ToString ()
            =>
                $"AsyncStackWrapper<{typeof(TEntry).Name},{typeof(TIn).Name},{typeof(TOut).Name}>" +
                $"{{ {this.Core.instances.AllTypes().Count()} registered types; " +
                $"canceled: {this.cancelPendingTasksSource.IsCancellationRequested} }}";

    }
}
