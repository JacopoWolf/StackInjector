using StackInjector.Wrappers;

namespace StackInjector.Core.Cloning
{
	/// <summary>
	/// A cloned core of a wrapper.
	/// </summary>
	public interface IClonedCore
	{

		/// <summary>
		/// converts this to an <see cref="IStackWrapper{TEntry}"/>
		/// </summary>
		/// <typeparam name="T">entry point of the new wrapper</typeparam>
		/// <returns>the new wrapper</returns>
		IStackWrapper<T> ToWrapper<T> ();


		/// <summary>
		/// convert this to an <see cref="IAsyncStackWrapper{TEntry, TIn, TOut}"/>
		/// </summary>
		/// <typeparam name="TEntry">entry instantiation poin of the new wrapper</typeparam>
		/// <typeparam name="TIn">type of input elements</typeparam>
		/// <typeparam name="TOut">type of output elements</typeparam>
		/// <param name="digest">action to perform on elements</param>
		/// <returns>the new wrapper</returns>
		IAsyncStackWrapper<TEntry, TIn, TOut> ToAsyncWrapper<TEntry, TIn, TOut> ( AsyncStackDigest<TEntry, TIn, TOut> digest );

	}

}