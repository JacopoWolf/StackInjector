using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StackInjector.Attributes;
using StackInjector.Core;

namespace StackInjector.Wrappers.Generic
{
    [Service(Version = 2.1, DoNotServeMembers = true)]
    internal class AsyncStackWrapper<TEntry, TIn, TOut> : AsyncStackWrapperCore<TOut>, IAsyncStackWrapper<TEntry, TIn, TOut>
    {

        public AsyncStackDigest<TEntry, TIn, TOut> StackDigest { get; internal set; }

        public AsyncStackWrapper ( WrapperCore core ) : base(core, typeof(AsyncStackWrapper<TEntry, TIn, TOut>))
        { }

        public void Submit ( TIn item )
        {
            base.Submit
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
                $"{{ {this.Core.instances.GetAllTypes().Count()} registered types; " +
                $"canceled: {this.cancelPendingTasksSource.IsCancellationRequested} }}";

    }
}
