using System;
using System.Collections.Generic;
using System.Text;
using StackInjector.Wrappers;

namespace StackInjector.Application
{
    /// <summary>
    /// Static class exposing methods to start 
    /// </summary>
    public static class StackInjector
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static StackWrapper Start<T> () where T : IStackEntryPoint
        {
            //! for testing purposes
            return null;
        }
    }
}
