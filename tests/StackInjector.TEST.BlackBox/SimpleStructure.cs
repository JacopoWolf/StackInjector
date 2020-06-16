using StackInjector.Attributes;
using StackInjector.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace StackInjector.TEST.BlackBox.SimpleStructure
{

// set as readonly, unused field
#pragma warning disable CS0649, IDE0044

    /*
     * Base -> Level1(A,B) -> Level2
     */


    #region base

    interface IBase { int Logic (); }

    [Service(Version = 1.0)]
    class Base : IBase
    {
        [Served]
        Level1A level1A;

        [Served]
        Level1B level1B;

        public int Logic () => (this.level1A.Logic + 5) + this.level1B.Logic;
    }


    [Service(Version = 2.0)]
    class VersionedBase
    {
        [Served(TargetingMethod = ServedVersionTargetingMethod.Exact, TargetVersion = 1.2)]
        public ILevel1 level1;
    }



    #endregion

    #region level 1

    interface ILevel1 { int Logic { get; } }

    [Service(Version = 1.1)]
    class Level1A : ILevel1
    {
        [Served]
        Level2 level2;
        public int Logic => this.level2.Logic2 + 15;
    }

    [Service(Version = 1.2)]
    class Level1B : ILevel1
    {
        [Served]
        Level2 level2;

        public int Logic => this.level2.Logic2 + 20;
    }

    [Service(Version = 5.0)]
    class Level1LatestVersion : ILevel1
    {
        public int Logic => 123;
    }

    #endregion

    #region level 2

    interface ILevel2 { int Logic2 { get; } }

    [Service(Version = 1.0)]
    class Level2 : ILevel2
    {
        public int Logic2 => 1;
    }

    #endregion

}
