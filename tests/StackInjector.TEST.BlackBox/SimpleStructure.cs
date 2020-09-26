using System.Collections.Generic;
using StackInjector.Attributes;
using StackInjector.Core;
using StackInjector.Core.Cloning;
using StackInjector.Settings;

namespace StackInjector.TEST.BlackBox
{

    // set as readonly, unused field
#pragma warning disable CS0649, IDE0044, IDE0051

    [Service]
    class EmptyTestClass { }

    /*
     * base -> level 1 (A,B) -> level 2
     */


    #region base

    internal interface IBase { int Logic (); }

    [Service(Version = 1.0)]
    internal class Base : IBase
    {
        [Served]
        private Level1A level1A;

        [Served]
        private Level1B level1B;

        public int Logic () => this.level1A.Logic + 5 + this.level1B.Logic;
    }


    [Service(Version = 2.0)]
    internal class VersionedBase
    {
        [Served(TargetingMethod = ServedVersionTargetingMethod.Exact, TargetVersion = 1.2)]
        public ILevel1 level1;
    }

    [Service]
    internal class ForgotTheServedAttributeBase
    {
        public Level1A level1A;

        public Level1B Level1B { get; set; }

        public int SomeMethod () => this.level1A.Logic + this.Level1B.Logic;
    }

    [Service]
    internal class AccessWrapperBase
    {
        [Served]
        public ICloneableCore wrapper;

        public IStackWrapperCore Clone ()
            =>
                this.wrapper.CloneCore().ToWrapper<AccessWrapperBase>();
    }

    [Service]
    internal class AllLevel1Base
    {
        [Served]
        public IEnumerable<ILevel1> level1s;
    }


    #endregion

    #region level 1

    internal interface ILevel1 { int Logic { get; } }

    [Service(Version = 1.1)]
    internal class Level1A : ILevel1
    {
        [Served]
        private Level2 level2;

        [Ignored]
        public int Logic => this.level2.Logic2 + 15;
    }

    [Service(Version = 1.2)]
    internal class Level1B : ILevel1
    {
        [Served]
        private Level2 level2;

        [Ignored]
        public int Logic => this.level2.Logic2 + 20;
    }

    [Service(Version = 5.0)]
    internal class Level1LatestVersion : ILevel1
    {
        [Ignored]
        public int Logic => 123;
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

}
