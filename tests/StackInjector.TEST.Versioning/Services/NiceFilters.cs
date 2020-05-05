using StackInjector.Attributes;

namespace StackInjector.TEST.Versioning.Services
{
    internal interface INiceFilter
    {
        bool IsNice ( int obj );
    }

    [Service(Version = 2.0)]
    class Nice : INiceFilter
    {
        public bool IsNice ( int obj ) => obj == 69;
    }

    [Service(Version = 1.0)]
    class TheAnswerFilter : INiceFilter
    {
        public bool IsNice ( int obj ) => obj == 42;
    }

    [Service(Version = 4.20)]
    class BobFilter : Nice
    {
        new public bool IsNice ( int obj ) => obj == 420;
    }


}