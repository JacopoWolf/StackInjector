﻿using StackInjector.Wrappers;
using StackInjector.Wrappers.Generic;

namespace StackInjector.Core.Cloning
{
    internal class ClonedCore : IClonedCore
    {

        private readonly WrapperCore clonedCore;

        internal ClonedCore ( WrapperCore clonedCore )
            =>
                this.clonedCore = clonedCore;


        public IAsyncStackWrapper ToAsyncWrapper<T> () where T : IAsyncStackEntryPoint
        {
            var wrapper = new AsyncStackWrapper( this.clonedCore );

            this.clonedCore.entryPoint = typeof(T);
            this.clonedCore.ServeAll();

            return wrapper;
        }

        public IAsyncStackWrapper<TEntry, TIn, TOut> ToGenericAsync<TEntry, TIn, TOut> ( AsyncStackDigest<TEntry, TIn, TOut> digest )
        {
            var wrapper = new AsyncStackWrapper<TEntry,TIn,TOut>( this.clonedCore );

            this.clonedCore.entryPoint = typeof(TEntry);
            this.clonedCore.ServeAll();

            return wrapper;
        }


        public IStackWrapper ToWrapper<T> () where T : IStackEntryPoint
        {
            var wrapper = new StackWrapper( this.clonedCore );

            this.clonedCore.entryPoint = typeof(T);
            this.clonedCore.ServeAll();

            return wrapper;
        }
    }
}
