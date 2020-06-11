using System;
using System.Collections.Generic;
using StackInjector.Behaviours;
using StackInjector.Settings;

namespace StackInjector.Core
{
    /// <summary>
    /// <para>The core common logic of every stack wrapper.</para>
    /// <para>The jobs of this class are reflection, versioning, instantiation, and injection.</para>
    /// </summary>
    internal partial class InjectionCore
    {
        // entry point object of this core
        internal Type entryPoint;

        // manage settings
        internal StackWrapperSettings settings;

        // holds instances
        internal IInstancesHolder instances;

        // tracks instantiated objects
        internal readonly List<object> instancesDiff;

        // used to lock this core on critical sections
        private readonly object _lock = new object();


        internal InjectionCore ( StackWrapperSettings settings )
        {
            this.settings = settings;

            this.instances = new SingleInstanceHolder();

            if( this.settings._trackInstancesDiff )
                this.instancesDiff = new List<object>();
        }
    }
}
