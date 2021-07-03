using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using StackInjector.Attributes;
using StackInjector.Core;
using StackInjector.Settings;
using CTkn = System.Threading.CancellationToken;

namespace StackInjector.TEST.BlackBox
{
	[TestFixture]
	public class Injector_Async
	{
		/*NOTE: AsyncStackWrapper is an extension od the normal wrapper,
		 * there is a common core logic; this means that the tests done in UseCases.Sync
		 * are also valid for the AsyncStackWrapper, since the tested code is the same
		 * (injection logic)
		 */

		// base async test class
		[Service]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822", Justification = "methods don't need to be static.")]
		private class AsyncBase
		{
			internal async Task<object> WaitForever ( object obj, CTkn tkn )
			{
				// waits forever unless cancelled
				await Task.Delay(-1, tkn);
				return obj;
			}

			internal async Task<object> ReturnArg ( object obj, CTkn tkn )
			{
				if ( tkn.IsCancellationRequested )
					return obj;

				await Task.CompletedTask;
				return obj;
			}
		}


		[Test]
		public void SubmitNoElaboration ()
		{
			using var wrapper = Injector.AsyncFrom<AsyncBase,object,object>( (b,i,t) => b.WaitForever(i,t) );
			wrapper.Submit(new object());

			Assert.Multiple(() =>
			{
				Assert.IsTrue(wrapper.AnyTaskLeft());
				Assert.IsFalse(wrapper.AnyTaskCompleted());
			});
		}


		[Test]
		public void SubmitNoCatch ()
		{
			var wrapper = Injector.AsyncFrom<AsyncBase,object,object>( (b,i,t) => b.ReturnArg(i,t) );

			var task = wrapper.SubmitAndGet(new object());
			task.Wait();

			Assert.Multiple(() =>
			{
				Assert.IsTrue(wrapper.AnyTaskLeft());
				Assert.IsTrue(wrapper.AnyTaskCompleted());
			});
		}


		[Test]
		public async Task SubmitAndCatchAsyncEnumerable ()
		{
			var wrapper = Injector.AsyncFrom<AsyncBase,object,object>( (b,i,t) => b.ReturnArg(i,t) );
			object
				obj1 = new(),
						obj2 = new();

			wrapper.Submit(obj1);
			wrapper.Submit(obj2);

			var objs = new List<object>();
			var count = 0;

			await foreach ( var obj in wrapper.Elaborated() )
			{
				objs.Add(obj);
				if ( ++count > 2 )
					break;
			}

			Assert.Multiple(() =>
			{
				Assert.IsFalse(wrapper.AnyTaskLeft());
				CollectionAssert.AreEquivalent(new object[] { obj1, obj2 }, objs);
			});

		}


		[Test]
		[Timeout(1000)]
		[Retry(3)]
		public void TaskCancellation ()
		{
			var wrapper = Injector.AsyncFrom<AsyncBase,object,object>( (b,i,t) => b.WaitForever(i,t) );
			var task = wrapper.SubmitAndGet(new object());
			var tkn = wrapper.PendingTasksCancellationToken;

			Assume.That(!tkn.IsCancellationRequested);

			var elaborationTask = wrapper.Elaborate();

			wrapper.Dispose();

			Assert.Multiple(() =>
			{
				Assert.That(tkn.IsCancellationRequested);
				var aggregate = Assert.Throws<AggregateException>(()=>task.Wait());
				Assert.IsInstanceOf<TaskCanceledException>(aggregate.InnerException);
			});	
		}



		[Test]
		public void ThrowsInvalidOnMultipleElaboration ()
		{
			using var wrapper = Injector.AsyncFrom<AsyncBase,object,object>( (b,i,t) => b.WaitForever(i,t) );
			wrapper.Submit(new object());

			wrapper.Elaborate();

			Assert.Multiple(() =>
			{
				Assert.IsTrue(wrapper.IsElaborating);
				var aggregate = Assert.Throws<AggregateException>(() => wrapper.Elaborate().Wait());
				Assert.IsInstanceOf<InvalidOperationException>(aggregate.InnerException);
			});


		}


		[Test]
		[Timeout(1000)]
		public void SubmitWithEvent ()
		{
			using var wrapper = Injector.AsyncFrom<AsyncBase,object,object>(
						(b,i,t) => b.ReturnArg(i,t),
						StackWrapperSettings.With(
							runtime:
								RuntimeOptions.Default
								.WhenNoMoreTasks(AsyncWaitingMethod.Wait,-1)
							)
						);

			// test holders
			var semaphore = new SemaphoreSlim(0);

			object
				token = new(),
				tokentest = null;
			IStackWrapperCore wrappertest = null;

			wrapper.OnElaborated += ( sender, args ) =>
			{
				tokentest = args.Result;
				wrappertest = (IStackWrapperCore)sender;
				semaphore.Release();
			};

			wrapper.Elaborate();

			Assert.Multiple(async () =>
			{
				await wrapper.SubmitAndGet(token);
				await semaphore.WaitAsync();
				Assert.AreSame(token, tokentest);
				Assert.AreSame(wrapper, wrappertest);
			});
		}


		[Test]
		[Timeout(100)]
		public void StopOnTimeout ()
		{
			var wrapper = Injector.AsyncFrom<AsyncBase,object,object>(
				(b,i,t) => b.ReturnArg(i,t),
				StackWrapperSettings.With(
					runtime:
						RuntimeOptions.Default
						.WhenNoMoreTasks(AsyncWaitingMethod.Timeout,1)
				)
			);

			Assume.That(!wrapper.IsElaborating);

			wrapper.Elaborate().Wait();

			Assert.That(!wrapper.IsElaborating && !wrapper.AnyTaskLeft());

		}

	}
}
