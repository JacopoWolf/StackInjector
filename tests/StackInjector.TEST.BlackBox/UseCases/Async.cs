using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using StackInjector.Attributes;
using CTkn = System.Threading.CancellationToken;


namespace StackInjector.TEST.BlackBox.UseCases
{

#pragma warning disable IDE0060, IDE0051, IDE0044, CS0169, CS0649

	internal class Async : CommonTestingMethods
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
				obj1 = new object(),
				obj2 = new object();

			wrapper.Submit(obj1);
			wrapper.Submit(obj2);

			var objs = new List<object>();
			var count = 0;

			await foreach( var obj in wrapper.Elaborated() )
			{
				objs.Add(obj);
				if( ++count > 2 )
					break;
			}

			Assert.Multiple(() =>
			{
				Assert.IsFalse(wrapper.AnyTaskLeft());
				CollectionAssert.AreEquivalent(new object[] { obj1, obj2 }, objs);
			});

		}


		[Test]
		[Retry(3)]
		[Timeout(1000)]
		public void TaskCancellation ()
		{
			var wrapper = Injector.AsyncFrom<AsyncBase,object,object>( (b,i,t) => b.WaitForever(i,t) );
			var task = wrapper.SubmitAndGet(new object());

			var elaborationTask = wrapper.Elaborate();

			wrapper.Dispose();

			Assert.Multiple(() =>
			{
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
		public void SubmitWithEvent ()
		{
			using var wrapper = Injector.AsyncFrom<AsyncBase,object,object>( (b,i,t) => b.ReturnArg(i,t) );

			var called = false;
			wrapper.OnElaborated += ( obj ) => called = true;

			var task = wrapper.SubmitAndGet(new object());
			wrapper.Elaborate();
			task.Wait();

			Assert.IsTrue(called);

		}


	}
}