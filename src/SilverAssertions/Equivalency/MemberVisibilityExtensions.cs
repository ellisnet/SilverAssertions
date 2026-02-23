using DennisDoomen.Reflectify;
using System;
using System.Collections.Concurrent;

namespace SilverAssertions.Equivalency;

internal static class MemberVisibilityExtensions
{
    private static readonly ConcurrentDictionary<MemberVisibility, MemberKind> Cache = new();

    public static MemberKind ToMemberKind(this MemberVisibility visibility)
    {
        return Cache.GetOrAdd(visibility, static v =>
        {
            MemberKind result = MemberKind.None;

            var flags = Enum.GetValues<MemberVisibility>();
            foreach (MemberVisibility flag in flags)
            {
                if (v.HasFlag(flag))
                {
                    var convertedFlag = flag switch
                    {
                        MemberVisibility.None => MemberKind.None,
                        MemberVisibility.Internal => MemberKind.Internal,
                        MemberVisibility.Public => MemberKind.Public,
                        MemberVisibility.ExplicitlyImplemented => MemberKind.ExplicitlyImplemented,
                        MemberVisibility.DefaultInterfaceProperties => MemberKind.DefaultInterfaceProperties,
                        _ => throw new ArgumentOutOfRangeException(nameof(v), v, null)
                    };

                    result |= convertedFlag;
                }
            }

            return result;
        });
    }
}
