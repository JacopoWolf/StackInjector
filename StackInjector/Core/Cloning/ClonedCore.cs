﻿using StackInjector.Wrappers;

namespace StackInjector.Core.Cloning
{
	internal class ClonedCore : IClonedCore
	{

		private readonly InjectionCore clonedCore;

		internal ClonedCore ( InjectionCore clonedCore )
		{
			this.clonedCore = clonedCore;
		}

		public IAsyncStackWrapper<TEntry, TIn, TOut> ToAsyncWrapper<TEntry, TIn, TOut> ( AsyncStackDigest<TEntry, TIn, TOut> digest )
		{
			var wrapper = new AsyncStackWrapper<TEntry,TIn,TOut>( this.clonedCore )
			{
				StackDigest = digest
			};

			this.clonedCore.EntryType = typeof(TEntry);
			this.clonedCore.Serve(cloned: true);

			return wrapper;
		}

		public IStackWrapper<T> ToWrapper<T> ()
		{
			var wrapper = new StackWrapper<T>(this.clonedCore);

			this.clonedCore.EntryType = typeof(T);
			this.clonedCore.Serve(cloned: true);

			return wrapper;
		}
	}
}
