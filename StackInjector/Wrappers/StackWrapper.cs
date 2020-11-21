using System;
using StackInjector.Attributes;
using StackInjector.Core;
using StackInjector.Settings;

namespace StackInjector.Wrappers
{
    [Service(Version = 3.0, Serving = ServingMethods.DoNotServe)]
    internal class StackWrapper<TEntry> : StackWrapperCore, IStackWrapper<TEntry>
    {

        internal StackWrapper ( InjectionCore core ) : base(core, typeof(StackWrapper<TEntry>)) { }



        public void Start ( Action<TEntry> stackDigest )
            =>
                stackDigest.Invoke(this.Entry);

        public TOut Start<TOut> ( Func<TEntry, TOut> stackDigest )
            =>
                stackDigest.Invoke(this.Entry);



        public TEntry Entry
            =>
                this.Core.GetEntryPoint<TEntry>();



        public override string ToString ()
            =>
                $"StackWrapper<{typeof(TEntry).Name}>{{ {this.Core.instances.Count} registered types }}";



        private bool disposed;

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
