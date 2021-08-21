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


		public StackWrapperCore ( InjectionCore core )
		{
			this.Core = core;

			// add this wrapper to possible instances
			var registerAs = this.GetType();
			if ( !this.Core.instances.AddType(registerAs) )
				this.Core.instances[registerAs].Clear();
			this.Core.instances[registerAs].AddFirst(this);

		}


		public IEnumerable<T> GetServices<T> ()
		{
			return this.Core
				.instances
				.InstancesAssignableFrom(typeof(T))
				.Select(o => (T)o);
		}

		public int CountServices ()
		{
			return this.Core.instances.CountAllInstances();
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
			var clonedCore = new InjectionCore( settings ??  this.Core.settings.Clone() )
			{
				instances = this.Core.instances.CloneStructure()
			};

			return new ClonedCore(clonedCore);
		}


		public abstract void Dispose ();
	}
}
