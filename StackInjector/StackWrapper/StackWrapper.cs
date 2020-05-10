using System;
using System.Linq;
using StackInjector.Attributes;
using StackInjector.Behaviours;
using StackInjector.Settings;

namespace StackInjector
{
    [Service(ReuseInstance = true, Version = 1.0, DoNotServeMembers = true)]
    internal partial class StackWrapper : IStackWrapper
    {

        internal Type EntryPoint { get; set; }

        public StackWrapperSettings Settings { get; internal set; }


        internal IInstancesHolder ServicesWithInstances { get; set; }




        /// <summary>
        /// internal constructor.
        /// </summary>
        internal StackWrapper ( StackWrapperSettings settings )
            =>
                this.Settings = settings;



        /// <inheritdoc/>
        public T Start<T> ()
            =>
                (T)this.GetStackEntryPoint().EntryPoint();


        /// <inheritdoc/>
        public object Start ()
            =>
                this.GetStackEntryPoint().EntryPoint();



        public override string ToString ()
            =>
                $"StackWrapper{{ {this.ServicesWithInstances.GetAllTypes().Count()} registered types; entry point: {this.EntryPoint.Name} }}";



        public IStackWrapper FromStructure<T> ( StackWrapperSettings overrideSettings = null ) where T : IStackEntryPoint
        {
            var clonedWrapper = new StackWrapper( overrideSettings ?? this.Settings )
            {
                EntryPoint = typeof(T),
                ServicesWithInstances = this.ServicesWithInstances
            };

            clonedWrapper.ServeAll();

            return clonedWrapper;
        }

        public IAsyncStackWrapper AsyncFromStructure<T> ( StackWrapperSettings overrideSettings = null ) where T : IAsyncStackEntryPoint
        {
            var clonedWrapper = new AsyncStackWrapper( overrideSettings ?? this.Settings )
            {
                EntryPoint = typeof(T),
                ServicesWithInstances = this.ServicesWithInstances
            };

            clonedWrapper.ServeAll();

            return clonedWrapper;
        }

    }
}