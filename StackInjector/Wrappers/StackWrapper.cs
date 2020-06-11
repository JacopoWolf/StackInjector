using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StackInjector.Attributes;
using StackInjector.Core;
using StackInjector.Settings;

namespace StackInjector.Wrappers
{
    [Service(Version = 1.1, Serving = ServingMethods.DoNotServe)]
    internal class StackWrapper<TEntry> : StackWrapperCore, IStackWrapper<TEntry>
    {

        internal StackWrapper ( InjectionCore core ) : base(core, typeof(StackWrapper<TEntry>)) { }



        public void Start ( Action<TEntry> stackDigest )
            =>
                stackDigest.Invoke(this.Core.GetEntryPoint<TEntry>());

        public TOut Start<TOut> ( Func<TEntry, TOut> stackDigest )
            =>
                stackDigest.Invoke(this.Core.GetEntryPoint<TEntry>());



        public override string ToString ()
            =>
                $"StackWrapper<{typeof(TEntry).Name}>{{ {this.Core.instances.AllTypes().Count()} registered types }}";


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
