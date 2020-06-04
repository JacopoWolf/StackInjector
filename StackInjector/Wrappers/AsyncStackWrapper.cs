using System;
using System.Linq;
using StackInjector.Attributes;
using StackInjector.Core;
using StackInjector.Settings;

namespace StackInjector.Wrappers
{
    [Obsolete]
    [Service(Version = 2.0, Serving = ServingMethods.DoNotServe)]
    internal partial class AsyncStackWrapper : AsyncStackWrapperCore<object>, IAsyncStackWrapper
    {

        /// <summary>
        /// create a new AsyncStackWrapper
        /// </summary>
        internal AsyncStackWrapper ( InjectionCore core ) : base(core, typeof(AsyncStackWrapper))
        { }


        public void Submit ( object item )
        {
            var task =
                this
                .Core
                .GetEntryPoint<IAsyncStackEntryPoint>()
                .Digest(item, this.PendingTasksCancellationToken);

            base.Submit(task);
        }


        public override string ToString ()
            =>
                $"AsyncStackWrapper{{ {this.Core.instances.AllTypes().Count()} registered types; " +
                $"entry point: {this.Core.entryPoint.Name}; canceled: {this.cancelPendingTasksSource.IsCancellationRequested} }}";

    }
}