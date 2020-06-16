using System;
using StackInjector.Attributes;

namespace StackInjector.TEST.ExternalAssembly
{

    public interface IExternalClass
    {
        void SomeMethod ();
    }

    [Service]
    public class Externalclass : IExternalClass
    {
        public void SomeMethod () => throw new NotImplementedException();
    }
}
