using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StackInjector.Settings;

namespace StackInjector
{
    internal class AsyncStackWrapper : StackWrapper, IAsyncStackWrapper
    {


        internal AsyncStackWrapper( StackWrapperSettings settings ) : base(settings)
        { }



        public Task<object> Submit ( object submitted ) => throw new NotImplementedException();


        public IAsyncEnumerable<T> Elaborated<T> () => throw new NotImplementedException();

        

        
    }
}
