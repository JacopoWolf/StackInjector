using System.Linq;
using StackInjector.Attributes;
using StackInjector.Core;

namespace StackInjector.Wrappers
{
    [Service(DoNotServeMembers = true, Version = 2.0)]
    internal partial class AsyncStackWrapper : AsyncStackWrapperCore<object>, IAsyncStackWrapper
    {

        /// <summary>
        /// create a new AsyncStackWrapper
        /// </summary>
        internal AsyncStackWrapper ( WrapperCore core ) : base(core, typeof(AsyncStackWrapper))
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
                $"AsyncStackWrapper{{ {this.Core.instances.GetAllTypes().Count()} registered types; " +
                $"entry point: {this.Core.entryPoint.Name}; canceled: {this.cancelPendingTasksSource.IsCancellationRequested} }}";

    }
}