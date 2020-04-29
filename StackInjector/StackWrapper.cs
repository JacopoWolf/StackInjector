using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using StackInjector.Attributes;
using StackInjector.Exceptions;
using StackInjector.Settings;

namespace StackInjector
{
    /// <summary>
    /// wraps a series of classes
    /// </summary>
    public sealed partial class StackWrapper
    {

        internal Type EntryPoint { get; set; }

        internal StackWrapperSettings Settings { get; set; }

        private IDictionary<Type, List<object>> ServicesWithInstances { get; set; }

        ////internal HashSet<Type> AllServiceTypes { get; private set; }
        ////internal List<object> Instances { get; private set; }




        /// <summary>
        /// internal constructor.
        /// </summary>
        internal StackWrapper ( StackWrapperSettings settings ) 
            => this.Settings = settings;



        /// <summary>
        /// Start this StackWrapper with the specified entry point and get it's return type converted to the specified type.
        /// Throws an error on a wrongful conversion
        /// </summary>
        public T Start<T> ()
        {
            return (T)this.GetStackEntryPoint().EntryPoint();
        }

        /// <summary>
        /// Start this StackWrapper with the specified entry point and get it's returned object in a generic form
        /// </summary>
        public object Start()
        {
            return this.GetStackEntryPoint().EntryPoint();
        }


        //todo StartAsync

        //todo Clone

    }
}