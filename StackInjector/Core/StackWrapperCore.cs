using System;
using System.Collections.Generic;
using System.Linq;
using StackInjector.Core.Cloning;
using StackInjector.Settings;

namespace StackInjector.Core
{
	/// <summary>
	/// Base implementation for stack wrappers
	/// </summary>
	internal abstract class StackWrapperCore : IStackWrapperCore
	{
		public ref readonly StackWrapperSettings Settings
			=> ref this.Core.settings;


		private protected readonly InjectionCore Core;


		public StackWrapperCore ( InjectionCore core, Type toRegister )
		{
			this.Core = core;

			// setting for referencing the calling wrapper as a service
			if( this.Core.settings.MaskOptions._registerWrapperAsService )
			{
				this.Core.instances.AddType(toRegister);
				this.Core.instances[toRegister].Clear();
				this.Core.instances[toRegister].AddFirst(this);
			}
		}


		public IEnumerable<T> GetServices<T> ()
		{
			return this.Core
						   .instances
						   .InstancesAssignableFrom(typeof(T))
						   .Select(o => (T)o);
		}

		public IClonedCore CloneCore ( StackWrapperSettings settings = null )
		{
			var clonedCore = new InjectionCore( settings ?? this.Core.settings )
			{
				instances = this.Core.instances
			};

			return new ClonedCore(clonedCore);
		}

		public IClonedCore DeepCloneCore ( StackWrapperSettings settings = null )
		{
			var clonedCore = new InjectionCore( settings ??  this.Core.settings.Copy() )
			{
				instances = this.Core.instances.CloneStructure()
			};

			return new ClonedCore(clonedCore);
		}


		public abstract void Dispose ();

	}
}
