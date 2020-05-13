using System;
using System.Collections.Generic;
using System.Text;
using StackInjector.Wrappers;

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


        public IStackWrapper ToWrapper<T> () where T : IStackEntryPoint
        {
            var wrapper = new StackWrapper( this.clonedCore );

            this.clonedCore.entryPoint = typeof(T);
            this.clonedCore.ServeAll();

            return wrapper;
        }
    }
}
