using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using StackInjector;
using StackInjector.TEST.SimpleStack1.Services;
using StackInjector.TEST.SimpleStack1.Services.Implementations;

namespace StackInjector.TEST.SimpleStack1
{
    /*
     *  This whole project works as core for the testing-oriented development of the core of this library
     */

    class TestProgram
    {
        [Test]
        public void WithInterfaces()
        {
            Application.StackInjector.Start<IThingsGenerator>();
        }

    }
}
