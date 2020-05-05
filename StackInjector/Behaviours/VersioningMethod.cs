using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using StackInjector.Attributes;
using StackInjector.Settings;

namespace StackInjector.Behaviours
{
    internal static class VersioningMethod
    {

        public static Type Version
        (
            this StackWrapper stackWrapper,
            Type target,
            ServedAttribute servedAttribute,
            ServedVersionTagettingMethod targettingMethod
        )
        {
            switch( targettingMethod )
            {
                case ServedVersionTagettingMethod.None:
                    return
                        stackWrapper
                            .ServicesWithInstances
                            .TypesAssignableFrom(target)
                            .First();

                case ServedVersionTagettingMethod.Exact:
                    return
                        stackWrapper
                            .ServicesWithInstances
                            .TypesAssignableFrom(target)
                            .First(t => t.GetCustomAttribute<ServiceAttribute>().Version == servedAttribute.TargetVersion);

                case ServedVersionTagettingMethod.LatestMajor:
                    return
                        stackWrapper
                            .ServicesWithInstances
                            .TypesAssignableFrom(target)
                            .Where(t => t.GetCustomAttribute<ServiceAttribute>().Version >= servedAttribute.TargetVersion)
                            .OrderByDescending(t => t.GetCustomAttribute<ServiceAttribute>().Version)
                            .First();

                case ServedVersionTagettingMethod.LatestMinor:
                    return
                        stackWrapper
                            .ServicesWithInstances
                            .TypesAssignableFrom(target)
                            .Where
                            (
                                t =>
                                {
                                    var v = t.GetCustomAttribute<ServiceAttribute>().Version;
                                    return
                                        v >= servedAttribute.TargetVersion
                                            &&
                                        v < Math.Floor(servedAttribute.TargetVersion + 1);
                                }
                            )
                            .OrderByDescending(t => t.GetCustomAttribute<ServiceAttribute>().Version)
                            .First();

                default:
                    throw new NotImplementedException();
            }
        }

    }
}
