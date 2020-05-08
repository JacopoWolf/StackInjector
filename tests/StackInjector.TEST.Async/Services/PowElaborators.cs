using System;
using System.Collections.Generic;
using System.Text;
using StackInjector.Attributes;

namespace StackInjector.TEST.Async.Services
{
    [Service]
    class MathService
    {
        public double Calculate ( object item ) => Math.Pow((int)item, 2);
    }



    class PowElaborator : IAsyncStackEntryPoint
    {

        [Served]
        MathService MathService { get; set; }

        public object Digest ( object item ) => this.MathService.Calculate(item);
    }
}
