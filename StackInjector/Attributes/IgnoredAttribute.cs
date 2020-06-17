using System;

namespace StackInjector.Attributes
{

    /// <summary>
    /// Indicates this field or property should be ignored from injection
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class IgnoredAttribute : Attribute
    {

    }

}
