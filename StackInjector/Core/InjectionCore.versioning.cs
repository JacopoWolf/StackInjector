using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using StackInjector.Attributes;
using StackInjector.Settings;

namespace StackInjector.Core
{
    internal partial class InjectionCore
    {
        // performs versioning on the specified type
        private IEnumerable<Type> Version ( Type targetType, ServedAttribute servedAttribute )
        {

            var targetVersion = servedAttribute?.TargetVersion ?? 0.0;
            var method =
                (this.settings._overrideTargetingMethod || servedAttribute is null || !(servedAttribute._targetingDefined))
                    ? this.settings._targetingMethod
                    : servedAttribute.TargetingMethod;

            var candidateTypes = this.instances.TypesAssignableFrom(targetType);


            return method switch
            {
                ServedVersionTargetingMethod.None
                    =>
                        candidateTypes,


                ServedVersionTargetingMethod.Exact
                    =>
                        candidateTypes
                        .Where
                        (
                            t =>
                                t.GetCustomAttribute<ServiceAttribute>()
                                    .Version == targetVersion
                        ),


                ServedVersionTargetingMethod.LatestMajor
                    =>
                        candidateTypes
                        .Where
                        (
                            t =>
                                t.GetCustomAttribute<ServiceAttribute>()
                                    .Version >= targetVersion
                        )
                        .OrderByDescending(t => t.GetCustomAttribute<ServiceAttribute>().Version),


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
                        .OrderByDescending(t => t.GetCustomAttribute<ServiceAttribute>().Version),


                _ => throw new NotImplementedException()
            };
        }

    }
}
