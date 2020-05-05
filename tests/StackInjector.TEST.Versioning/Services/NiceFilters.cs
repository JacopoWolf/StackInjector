using StackInjector.Attributes;

namespace StackInjector.TEST.Versioning.Services
{
    internal interface INiceFilter
    {
        bool IsNice ( int obj );
    }

    [Service(Version = 2.00)]
    class NiceFilter : INiceFilter
    {
        public virtual bool IsNice ( int obj ) => obj == 69;
    }

    [Service(Version = 1.00)]
    class TheAnswerFilter : INiceFilter
    {
        public bool IsNice ( int obj ) => obj == 42;
    }

    [Service(Version = 4.20)]
    class BobFilter : NiceFilter
    {
        public override bool IsNice ( int obj ) => obj == 420;
    }


}