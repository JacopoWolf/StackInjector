using System;
using System.Collections.Generic;
using System.Text;
using StackInjector.Attributes;

namespace StackInjector.TEST.SimpleStack1.Services
{
    interface IThingsGenerator : IStackEntryPoint
    {
        string GenerateThing ();
    }
}
