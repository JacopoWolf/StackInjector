﻿#define CONTRACTS_ALL

using System;
using System.Collections.Generic;

namespace StackInjector.Settings
{
	/// <summary>
	/// Used to filter Types from the injection proces. 
	/// When trying to inject masked Types an injection will be thrown.<br/>
	/// Use <see cref="Register(Type[])"/> to Add multiple types at once.
	/// </summary>
	public sealed class MaskOptions : HashSet<Type>, IOptions
	{
		internal bool _isDisabled;
		internal bool _isWhiteList;

		private MaskOptions () { }

		IOptions IOptions.CreateDefault ()
		{
			return Disabled;
		}


		/// <summary>
		/// Register multiple Types at once.<br/>
		/// see also <seealso cref="HashSet{T}.Add(T)"/>
		/// </summary>
		/// <param name="types">types to register</param>
		/// <returns>The modified Mask object</returns>
		public MaskOptions Register ( params Type[] types )
		{
			if ( _isDisabled )
				throw new InvalidOperationException("cannot register to a disabled mask");

			foreach ( var t in types )
				base.Add(t);
			return this;
		}


		/// <summary>
		/// Based on the type of mask, if the type is masked_out/forbidden from injection.<br/>
		/// ie: if A is registered and this is a blacklist, <c>IsMasked(A)</c> will return true.
		/// </summary>
		/// <param name="type">type to be checked</param>
		/// <returns>always false when disabled, true if <paramref name="type"/> is masked.</returns>
		public bool IsMasked ( Type type )
		{
			if ( _isDisabled )
				return false;
			else
				return this._isWhiteList ^ this.Contains(type); //.XOR

		}

		/// <inheritdoc/>
		public object Clone ()
		{
			return MemberwiseClone();
		}


		/// <summary>
		/// allow only registred types.
		/// </summary>
		public static MaskOptions WhiteList => new MaskOptions() { _isWhiteList = true };

		/// <summary>
		/// allow every type <b>except</b> the regisred ones.
		/// </summary>
		public static MaskOptions BlackList => new MaskOptions();

		/// <summary>
		/// allow everything, don't even check.
		/// </summary>
		public static MaskOptions Disabled => new MaskOptions() { _isDisabled = true };


	}
}
