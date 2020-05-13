using System;
using System.Linq;
using System.Reflection;
using StackInjector.Attributes;
using StackInjector.Settings;

namespace StackInjector.Core
{
    internal partial class WrapperCore
    {

        internal Type Version
        (
            Type targetType,
            double targetVersion,
            ServedVersionTargetingMethod method
        )
        {
            var candidateTypes = this.instances.TypesAssignableFrom(targetType);

            return method switch
            {
                ServedVersionTargetingMethod.None
                    =>
                        candidateTypes.First(),


                ServedVersionTargetingMethod.Exact
                    =>
                        candidateTypes
                        .First
                        (
                            t =>
                                t.GetCustomAttribute<ServiceAttribute>()
                                    .Version == targetVersion
                        ),


                ServedVersionTargetingMethod.LatestMajor
                    =>
                        candidateTypes
                        .Where(t => t.GetCustomAttribute<ServiceAttribute>().Version >= targetVersion)
                        .OrderByDescending(t => t.GetCustomAttribute<ServiceAttribute>().Version)
                        .First(),


                ServedVersionTargetingMethod.LatestMinor
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
