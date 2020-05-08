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
        /// <returns></returns>
        object Digest ( object item );

    }
}
