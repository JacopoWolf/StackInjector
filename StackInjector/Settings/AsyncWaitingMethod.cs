namespace StackInjector.Settings
{

	/// <summary>
	/// How an <see cref="Wrappers.IAsyncStackWrapper{TEntry, TIn, TOut}"/> should behave when there are no pending tasks.
	/// </summary>
	public enum AsyncWaitingMethod
	{

		/// <summary>
		/// Simply exit the <c>await foreach</c> loop withouth doing anything
		/// </summary>
		Exit,

		/// <summary>
		/// <para>don't exit the loop, but wait for new Tasks to be submitted. </para>
		/// <para>Exiting the loop requires <c>Dispose()</c> to be called on the wrapper or a <c>break</c></para>
		/// </summary>
		Wait,

		/// <summary>
		/// <para>don't immediately exit the loop, but rather wait a certain amount of time for 
		/// new tasks to be submitted tobefore exiting the loop</para>
		/// </summary>
		Timeout

	}
}