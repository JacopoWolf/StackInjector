using System;
using System.Collections.Generic;
using System.Text;

namespace StackInjector
{
    /// <summary>
    /// Entry point for asyncronous Stacks.
    /// </summary>
    public interface IAsyncStackEntryPoint
    {

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <returns></returns>
        TOut Digest<TIn, TOut> ( TIn item );

    }
}
