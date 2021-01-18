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

		public void Submit ( TIn item )
		{
			var task = this.StackDigest.Invoke
					(
						this.Entry,
						item,
						this.PendingTasksCancellationToken
					);

			base.Submit(task);
		}

		public Task<TOut> SubmitAndGet ( TIn item )
		{
			var task = this.StackDigest.Invoke
					(
						this.Entry,
						item,
						this.PendingTasksCancellationToken
					);

			base.Submit(task);
			return task;
		}

		public override string ToString ()
		{
			return $"AsyncStackWrapper<{typeof(TEntry).Name},{typeof(TIn).Name},{typeof(TOut).Name}>" +
						   $"{{ {this.Core.instances.Count} registered types; " +
						   $"canceled: {this.cancelPendingTasksSource.IsCancellationRequested} }}";
		}
	}
}
