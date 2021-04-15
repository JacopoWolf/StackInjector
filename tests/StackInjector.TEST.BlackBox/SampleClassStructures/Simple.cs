using System.Collections.Generic;
using StackInjector.Attributes;
using StackInjector.Core;
using StackInjector.Core.Cloning;
using StackInjector.Settings;

namespace StackInjector.TEST.Structures.Simple
{

	// remove unnecessary warning
	// field is unused
	// cannot assign a value different from null

#pragma warning disable CS0649

	#region base

	internal interface IBase { void Logic (  ); }

	[Service(Version = 1.0)]
	internal class Base : IBase
	{
		[Served]
		public Level1_11 level1A;

		[Served]
		public Level1_12 level1B;

		public void Logic ( ) { }
	}

	#endregion

	#region level 1

	internal interface ILevel1 { int Logic { get; } }

	[Service(Version = 1.1)]
	internal class Level1_11 : ILevel1
	{
		[Served]
		public readonly Level2 level2;

		[Ignored]
		public int Logic => 42;
	}

	[Service(Version = 1.2)]
	internal class Level1_12 : ILevel1
	{
		[Served]
		public readonly Level2 level2;

		[Ignored]
		public int Logic => 40 + 2;
	}

	[Service(Version = 5.0)]
	internal class Level1_50 : ILevel1
	{
		[Ignored]
		public int Logic => 123;
	}

	[Service(Version = 2.0)]
	internal class Level1b_20 : Level1_11
	{

	}

	#endregion

	#region level 2

	internal interface ILevel2 { int Logic2 { get; } }

	[Service(Version = 1.0)]
	internal class Level2 : ILevel2
	{
		[Ignored]
		public int Logic2 => 1;
	}

	#endregion

#pragma warning restore

}
