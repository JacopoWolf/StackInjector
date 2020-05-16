using System;
using StackInjector.Core.Cloning;
using StackInjector.Settings;

namespace StackInjector.Core
{
    internal abstract class AsyncStackWrapperCore : IStackWrapperCore
    {
        public ref readonly StackWrapperSettings Settings
            => ref this.Core.settings;


        private protected readonly WrapperCore Core;


        public AsyncStackWrapperCore ( WrapperCore core, Type toRegister )
        {
            this.Core = core;

            // setting for referencing the calling wrapper as a service
            if( this.Core.settings.registerSelf )
                this.Core.instances.AddInstance(toRegister, this);
        }


        public IClonedCore CloneCore ( StackWrapperSettings settings = null )
        {
            var clonedCore = new WrapperCore( settings ?? this.Core.settings )
            {
                instances = this.Core.instances
            };

            return new ClonedCore(clonedCore);
        }


        public abstract void Dispose ();

    }
}
