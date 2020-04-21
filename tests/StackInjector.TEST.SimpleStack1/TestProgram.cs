using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using StackInjector;
using StackInjector.TEST.SimpleStack1.Services;

namespace StackInjector.TEST.SimpleStack1
{
    /*
     *  This whole project works as core for the testing-oriented development of the core of this library
     */

    class TestProgram
    {
        [Test]
        public void Main()
        {
            Application.StackInjector.Start<IThingsGenerator>();
        }

    }
}
