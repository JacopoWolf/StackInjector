﻿using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using StackInjector;
using StackInjector.Exceptions;
using StackInjector.TEST.SimpleStack1.Services;
using StackInjector.TEST.SimpleStack1.Services.Implementations;

namespace StackInjector.TEST.SimpleStack1
{
   
    // This whole project works as core for the testing-oriented development of the core of this library

    class TestProgram
    {
        [Test]
        public void WithInterfaces()
        {
            var result = Injector.From<IThingsGenerator>().Start<string>();

            Assert.AreEqual("test", result);
        }

        [Test]
        public void NotAService()
        {
            Assert.Throws
                (
                    typeof(NotAServiceException),
                    () =>
                    {
                        Injector.From<IBadThingsGenerator>().Start();
                    }
                );
        }

    }
}
