using System;
using System.Collections.Generic;
using System.Reflection;
using StackInjector.Attributes;
using StackInjector.Behaviours;
using StackInjector.Exceptions;
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
        private Type _entryType;
        internal Type EntryType
        {
            get
                => this._entryType;
            set
            {
                var serviceAtt = value.GetCustomAttribute<ServiceAttribute>();
                if( serviceAtt != null && serviceAtt.Pattern == InstantiationPattern.AlwaysCreate )
                    throw new InvalidEntryTypeException(value, $"Entry point {value.Name} cannot have {InstantiationPattern.AlwaysCreate} as instantiation pattern.");

                this._entryType = value;
            }
        }


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
