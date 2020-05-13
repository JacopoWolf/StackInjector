using System.Linq;
using StackInjector.Attributes;
using StackInjector.Core;

namespace StackInjector.Wrappers
{
    [Service(Version = 1.0, DoNotServeMembers = true)]
    internal class StackWrapper : AsyncStackWrapperCore, IStackWrapper
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