using System.Linq;
using StackInjector.Attributes;
using StackInjector.Core;
using StackInjector.Settings;

namespace StackInjector.Wrappers
{
    [Service(Version = 1.0, Serving = ServingMethods.DoNotServe)]
    internal class StackWrapper : StackWrapperCore, IStackWrapper
    {


        internal StackWrapper ( InjectionCore core ) : base(core, typeof(StackWrapper))
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
                $"StackWrapper{{ {this.Core.instances.AllTypes().Count()} registered types; entry point: {this.Core.entryPoint.Name} }}";







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