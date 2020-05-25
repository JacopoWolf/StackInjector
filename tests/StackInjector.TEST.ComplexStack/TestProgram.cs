using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using StackInjector.Settings;

namespace StackInjector.TEST.ComplexStack
{
    public class Tests
    {

        [Test]
        public void Performance ()
        {
            var watch = Stopwatch.StartNew( );

            using var wrapper = Injector.From<IBaseService>();

            watch.Stop();
            Console.WriteLine($"Time taken: {watch.ElapsedMilliseconds}ms");
            watch.Restart();

            wrapper.Start();

            watch.Stop();
            Console.WriteLine($"Time taken: {watch.ElapsedMilliseconds}ms");
        }

        [Test]
        public void GetServices()
        {
            using var wrapper = Injector.From<IBaseService>();

            var candidates = wrapper.GetServices<IBaseService>();

            if( candidates.Count() != 1 )
                throw new Exception();

            Assert.AreEqual( typeof(Application), candidates.First().GetType() );
        }

        [Test]
        public void NoServiceInEnum()
        {
            using var wrapper = Injector.From<EmptyEnumApplication>();

            var application = wrapper.GetServices<EmptyEnumApplication>().First();

            CollectionAssert.IsEmpty(application.tricks);

        }
    }
}