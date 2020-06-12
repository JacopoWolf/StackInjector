using System;
using System.Collections.Generic;
using System.Text;
using StackInjector.Attributes;

namespace StackInjector.TEST.SimpleStack.Services
{
    [Service(Pattern = Settings.InstantiationPattern.AlwaysCreate)]
    class BadEntryPointGenerator
    {
        public void Test()
        {
            Console.WriteLine("something");
        }
    }
}
