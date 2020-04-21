using System;
using System.Collections.Generic;
using System.Text;

namespace StackInjector
{
    /// <summary>
    /// Entry point class for a simple stack
    /// </summary>
    public interface IStackEntryPoint
    {
        /// <summary>
        /// This will be called once the stack is initialized
        /// </summary>
        void EntryPoint ();
    }
}
