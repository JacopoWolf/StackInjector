using System;
using System.Diagnostics;
using NUnit.Framework;
using StackInjector.Settings;

namespace StackInjector.TEST.ComplexStack
{
    public class Tests
    {

        [Test]
        public void Performance ()
        {
            var settings =
                StackWrapperSettings.Default
                .ServeMultiple();

            var watch = Stopwatch.StartNew( );

            using var wrapper = Injector.From<IBaseService>( settings );

            watch.Stop();
            Console.WriteLine($"Time taken: {watch.ElapsedMilliseconds}ms");
            watch.Restart();

            wrapper.Start();

            watch.Stop();
            Console.WriteLine($"Time taken: {watch.ElapsedMilliseconds}ms");
        }
    }
}