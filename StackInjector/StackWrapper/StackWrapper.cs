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

        internal StackWrapperSettings Settings { get; set; }


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


        

        public object Clone () => throw new NotImplementedException();


        public IAsyncStackWrapper ToAsync () => throw new NotImplementedException();
    }
}