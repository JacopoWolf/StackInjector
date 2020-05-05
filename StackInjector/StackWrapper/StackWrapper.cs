using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using StackInjector.Attributes;
using StackInjector.Behaviours;
using StackInjector.Exceptions;
using StackInjector.Settings;

namespace StackInjector
{

    internal partial class StackWrapper : IStackWrapper
    {

        internal Type EntryPoint { get; set; }

        internal StackWrapperSettings Settings { get; set; }


        //private IDictionary<Type, object> ServicesWithInstances { get; set; }
        internal IInstancesHolder ServicesWithInstances { get; set; }




    /// <summary>
    /// internal constructor.
    /// </summary>
    internal StackWrapper ( StackWrapperSettings settings )
            => this.Settings = settings;



        /// <inheritdoc/>
        T IStackWrapper.Start<T> ()
        {
            return (T)this.GetStackEntryPoint().EntryPoint();
        }

        /// <inheritdoc/>
        object IStackWrapper.Start ()
        {
            return this.GetStackEntryPoint().EntryPoint();
        }

    }
}