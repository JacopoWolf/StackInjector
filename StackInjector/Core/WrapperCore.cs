using System;
using System.Collections.Generic;
using System.Text;
using StackInjector.Behaviours;
using StackInjector.Settings;

namespace StackInjector.Core
{
    internal partial class WrapperCore
    {
        // entry point object of this core
        internal Type entryPoint;

        // manage settings
        internal StackWrapperSettings settings;

        // holds instances
        internal IInstancesHolder instances;

        // tracks instantiated objects
        internal readonly List<object> instancesDiff;


        internal WrapperCore ( StackWrapperSettings settings )
        {
            this.settings = settings;

            this.instances = new SingleInstanceHolder();

            if( this.settings.trackInstancesDiff )
                this.instancesDiff = new List<object>();
        }
    }
}
