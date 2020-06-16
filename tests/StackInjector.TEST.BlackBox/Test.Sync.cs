﻿using System;
using System.Linq;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using StackInjector.Attributes;
using StackInjector.Settings;
using StackInjector.TEST.BlackBox.SimpleStructure;

namespace StackInjector.TEST.BlackBox
{

#pragma warning disable IDE0051, IDE0044, CS0169, CS0649

    internal class TestSync
    {

        [Test]
        public void FromClass ()
        {
            var baseClass = Injector.From<Base>().Entry;

            // asserts the whole structure works and references are correct.
            Assert.AreEqual(42, baseClass.Logic());
        }


        [Test]
        public void FromInterface ()
        {
            var baseClass = Injector.From<IBase>().Entry;

            // asserts the whole structure works and references are correct.
            Assert.AreEqual(42, baseClass.Logic());
        }


        [Test]
        public void ServedVersioning ()
        {
            var versionedService = Injector.From<VersionedBase>().Entry.level1;

            Assert.That(versionedService, Is.TypeOf<Level1B>());
        }


        [Test]
        public void SettingVersioningLatestMaj ()
        {
            var settings =
                StackWrapperSettings.Default
                .InjectionVersioningMethod(ServedVersionTargetingMethod.LatestMajor,true);

            var versionedService = Injector.From<VersionedBase>( settings ).Entry.level1;

            Assert.That(versionedService, Is.TypeOf<Level1LatestVersion>());
        }


        [Test]
        public void SettingVersioningLatestMin ()
        {
            var settings =
                StackWrapperSettings.Default
                .InjectionVersioningMethod(ServedVersionTargetingMethod.LatestMinor,true);

            var versionedService = Injector.From<VersionedBase>( settings ).Entry.level1;

            Assert.That(versionedService, Is.TypeOf<Level1B>());
        }


        [Service] private class ReferenceLoopA { [Served] public ReferenceLoopB loopB; }

        [Service] private class ReferenceLoopB { [Served] public ReferenceLoopA loopA; }

        [Test]
        public void CircularReference ()
        {
            Assert.That(() => Injector.From<ReferenceLoopA>(), Throws.Nothing);
        }


        [Test]
        public void ServeStrict ()
        {
            var settings =
                StackWrapperSettings.Default
                .InjectionServingMethods( Injector.Defaults.ServeAllStrict, true );

            var entry = Injector.From<ForgotTheServedAttributeBase>(settings).Entry;

            Assert.That(new object[] { entry.level1A, entry.Level1B }, Is.All.Null);
        }


        [Test]
        public void AccessWrapper ()
        {
            var wrapper = Injector.From<AccessWrapperBase>();

            Assert.AreSame(wrapper, wrapper.Entry.wrapper);
        }


        [Test]
        public void CloneSame ()
        {
            var wrapper = Injector.From<AccessWrapperBase>();

            var cloneWrapper = wrapper.Entry.Clone();

            CollectionAssert.AreEquivalent( wrapper.GetServices<object>(), cloneWrapper.GetServices<object>() );
        }

        
        [Test]
        public void ServeEnum ()
        {
            var injected = Injector.From<AllLevel1Base>().Entry.level1s;

            CollectionAssert.AreEquivalent
                (
                    new Type[] { typeof(Level1A), typeof(Level1B), typeof(Level1LatestVersion) },
                    injected.Select( i => i.GetType() ) 
                );
        }

    }
}