using System;
using System.Linq;
using System.Reflection;
using StackInjector.Attributes;
using StackInjector.Settings;

namespace StackInjector
{
    internal partial class StackWrapper
    {

        internal Type Version
        (
            Type targetType,
            double targetVersion,
            ServedVersionTagetingMethod method
        )
        {
            var candidateTypes = this.ServicesWithInstances.TypesAssignableFrom(targetType);

            return method switch
            {
                ServedVersionTagetingMethod.None
                    =>
                        candidateTypes.First(),


                ServedVersionTagetingMethod.Exact
                    =>
                        candidateTypes
                        .First
                        (
                            t =>
                                t.GetCustomAttribute<ServiceAttribute>()
                                    .Version == targetVersion
                        ),


                ServedVersionTagetingMethod.LatestMajor
                    =>
                        candidateTypes
                        .Where(t => t.GetCustomAttribute<ServiceAttribute>().Version >= targetVersion)
                        .OrderByDescending(t => t.GetCustomAttribute<ServiceAttribute>().Version)
                        .First(),


                ServedVersionTagetingMethod.LatestMinor
                    =>
                        candidateTypes
                        .Where
                        (
                            t =>
                            {
                                var v = t.GetCustomAttribute<ServiceAttribute>().Version;
                                return
                                    v >= targetVersion
                                        &&
                                    v < Math.Floor(targetVersion + 1);
                            }
                        )
                        .OrderByDescending(t => t.GetCustomAttribute<ServiceAttribute>().Version)
                        .First(),


                _ => throw new NotImplementedException()
            };
        }

    }
}
