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
        public object Calculate ( object item ) => Math.Pow((int)item, 2);
    }



    class PowElaborator
    {

        [Served]
        MathService MathService { get; set; }


        public Task<object> Digest ( object item, CancellationToken cancellationToken )
            =>
                 Task.Run(() => this.MathService.Calculate(item), cancellationToken);

    }
}
