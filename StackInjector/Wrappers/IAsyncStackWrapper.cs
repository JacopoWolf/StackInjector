﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using StackInjector.Core;

namespace StackInjector.Wrappers
{
	/// <summary>
	/// rappresents a strongly typed generic entry point 
	/// of an <see cref="IAsyncStackWrapper{TEntry, TIn, TOut}"/>
	/// </summary>
	/// <typeparam name="TEntry">the entry type of this object</typeparam>
	/// <typeparam name="TIn">type on the submitted item</typeparam>
	/// <typeparam name="TOut">type of the return item</typeparam>
	/// <param name="instance">the instance of the entry</param>
	/// <param name="item">the item to elaborate</param>
	/// <param name="cancellationToken">the cancellation token used to cancel the task</param>
	/// <returns>a task rappresenting the current job</returns>
	public delegate Task<TOut> AsyncStackDigest<TEntry, in TIn, TOut>
		(
			TEntry instance,
			TIn item,
			CancellationToken cancellationToken
		);


	/// <summary>
	/// Wraps a Stack of dependency-injected classes, and manages an <see cref="IAsyncEnumerable{T}"/> of completed tasks.
	/// </summary>
	/// <typeparam name="TEntry"></typeparam>
	/// <typeparam name="TIn">type submitted to the digest function</typeparam>
	/// <typeparam name="TOut">return type of the Digest function</typeparam>
	public interface IAsyncStackWrapper<TEntry, in TIn, TOut> : IAsyncStackWrapperCore<TOut>, IEntryGetter<TEntry>
	{

		/// <summary>
		/// submit a new item to be elaborated
		/// </summary>
		/// <param name="item">the item to submit</param>
		void Submit ( TIn item );


		/// <summary>
		/// submits a new item to be elaborated and returns the submitted task
		/// </summary>
		/// <param name="item">the item to submit to elaboration</param>
		/// <returns>a task tapresenting the elaboration of <paramref name="item"/></returns>
		Task<TOut> SubmitAndGet ( TIn item );

	}
}
