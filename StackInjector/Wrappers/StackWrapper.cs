using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using StackInjector.Attributes;
using StackInjector.Behaviours;
using StackInjector.Core;
using StackInjector.Settings;

namespace StackInjector.Wrappers
{
    [Service(Version = 1.0, DoNotServeMembers = true)]
    internal class StackWrapper : StackWrapperBase, IStackWrapper
    {


        internal StackWrapper ( WrapperCore core ) : base(core, typeof(StackWrapper))
        { }


        /// <inheritdoc/>
        public T Start<T> ()
            =>
                (T)this.Core.GetEntryPoint<IStackEntryPoint>().EntryPoint();


        /// <inheritdoc/>
        public object Start ()
            =>
                this.Core.GetEntryPoint<IStackEntryPoint>().EntryPoint();



        public override string ToString ()
            =>
                $"StackWrapper{{ {this.Core.instances.GetAllTypes().Count()} registered types; entry point: {this.Core.entryPoint.Name} }}";







        private bool disposed = false;

        public override void Dispose ()
        {
            if( !this.disposed )
            {
                this.Core.RemoveInstancesDiff();

                this.disposed = true;
            }
        }
    }
}