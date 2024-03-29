﻿using System;
using StackInjector.Attributes;

namespace StackInjector.Settings
{
	/// <summary>
	/// Defines how injection in this service shall be performed
	/// </summary>
	[Flags]
	public enum ServingMethods
	{
		/// <summary>
		/// empty servicing method. Do not serve, disabling injection for a particular class.
		/// </summary>
		None = 0,

		/// <summary>
		/// Serve only to those properties or fields marked with <see cref="ServedAttribute"/>.
		/// If not set, serve everything
		/// </summary>
		Strict = 0b1000,

		/// <summary>
		/// Allow serving to fields
		/// </summary>
		Fields = 0b0001,

		/// <summary>
		/// Allow serving to properties
		/// </summary>
		Properties = 0b0010
	}

}
