using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StackInjector.Attributes;
using StackInjector.Core;
using StackInjector.Wrappers;

namespace StackInjector.TEST.Async.Services
{
    [Service]
    class MathService
    {
        public int Calculate ( int item ) => (int)Math.Pow(item, 2);
    }



    class PowElaborator
    {

        [Served]
        MathService MathService { get; set; }


        public Task<int> Digest ( int item, CancellationToken cancellationToken )
            =>
                 Task.Run(() => this.MathService.Calculate(item), cancellationToken);

    }
}
