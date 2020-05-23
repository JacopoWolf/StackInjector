using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using StackInjector.Attributes;
using StackInjector.Core.Cloning;
using StackInjector.Wrappers.Generic;

namespace StackInjector.TEST.ComplexStack
{
    [Service]
    class Application : IBaseService
    {
        [Served]
        ILogger logger;

        [Served]
        IAnsweringService answerer;

        // allows cloning
        [Served]
        ICloneableCore wrapper;

        [Served]
        IEnumerable<IRunBeforeStart> runs;

        // internal clone
        IAsyncStackWrapper<IReadingService,string,string> asyncStack;


        public void Elaborate ()
        {
            Random rnd = new Random(420);

            for( int i = 0; i < 20; i++ )
            {
                var time = rnd.Next(0, 50);

                this.asyncStack.Submit($"test{time}");

                Task.Delay(time).Wait();
            }

        }



        public object EntryPoint ()
        {
            foreach( var run in this.runs )
                this.logger.Log( 100, $"run: {run.Run()}" );

            this.logger.Log(10, "entry point");

            this.asyncStack = 
                this.wrapper
                .CloneCore()
                .ToAsyncWrapper<IReadingService, string, string>
                (
                    async ( instance, entry, cancellation ) =>
                    {
                        var noise = await instance.ReadAsync(cancellation);
                        return this.answerer.ResponseTo( noise + entry );
                    }
                );

            Task.Run
            (
                async () =>
                {
                    await foreach( var str in this.asyncStack.Elaborated() )
                        this.logger.Log(64, str);
                }
            );

            this.Elaborate();

            this.asyncStack.Dispose();
            return null;
        }



    }

    [Service]
    class Reader : IReadingService
    {
        [Served]
        ILogger logger;

        public async Task<string> ReadAsync ( CancellationToken token )
        {
            await Task.Delay(5);
            this.logger.Log(0, "can't trigger!");
            return "noise";
        }
    }

    [Service]
    class Responder : IAnsweringService
    {
        [Served]
        ILogger logger;

        public string ResponseTo ( string to )
        {
            this.logger.Log(5, $"recieved {to}");
            return "lazy response";
        }
    }

    [Service]
    class Logger : ILogger
    {
        [Served]
        ILogFilter filter;

        public void Log ( byte gravity, string message )
        {
            if( this.filter.LogLevel(gravity) )
                Console.WriteLine(message);
        }
    }

    [Service]
    class LogFilter : ILogFilter
    {
        // lazy implementation
        public bool LogLevel ( byte gravity ) => gravity >= 5;
    }


    [Service]
    class Run1 : IRunBeforeStart
    {
        public int Run () => 1;
    }

    [Service]
    class Run2 : IRunBeforeStart
    {
        public int Run () => 2;
    }

    [Service]
    class Run3 : IRunBeforeStart
    {
        public int Run () => 3;
    }

}