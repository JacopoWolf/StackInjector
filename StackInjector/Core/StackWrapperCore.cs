using System;
using StackInjector.Core.Cloning;
using StackInjector.Settings;
using StackInjector.Wrappers;

namespace StackInjector.Core
{
    internal abstract class StackWrapperCore : IStackWrapperCore
    {
        public ref readonly StackWrapperSettings Settings
            => ref this.Core.settings;


        private protected readonly WrapperCore Core;


        public StackWrapperCore ( WrapperCore core, Type toRegister )
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


        ////public IStackWrapper FromStructure<T> ( StackWrapperSettings overrideSettings = null ) where T : IStackEntryPoint
        ////{

        ////    var clonedCore = new WrapperCore( overrideSettings ?? this.Core.settings )
        ////    {
        ////        entryPoint = typeof(T),
        ////        instances = this.Core.instances
        ////    };

        ////    var wrapper = new StackWrapper( clonedCore );

        ////    wrapper.Core.ServeAll();

        ////    return wrapper;
        ////}

        ////public IAsyncStackWrapper AsyncFromStructure<T> ( StackWrapperSettings overrideSettings = null ) where T : IAsyncStackEntryPoint
        ////{
        ////    var clonedCore = new WrapperCore( overrideSettings ?? this.Core.settings )
        ////    {
        ////        entryPoint = typeof(T),
        ////        instances = this.Core.instances
        ////    };

        ////    var wrapper = new AsyncStackWrapper( clonedCore );

        ////    clonedCore.ServeAll();

        ////    return wrapper;
        ////}




        public abstract void Dispose ();
        
    }
}
