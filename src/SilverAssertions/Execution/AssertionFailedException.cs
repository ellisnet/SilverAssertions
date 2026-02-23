using System;
using System.Runtime.Serialization;

namespace SilverAssertions.Execution;

/// <summary>
/// Represents the default exception in case no test framework is configured.
/// </summary>
[Serializable]
#pragma warning disable CA1032, RCS1194 // AssertionFailedException should never be constructed with an empty message
public class AssertionFailedException : Exception
#pragma warning restore CA1032, RCS1194
{
    public AssertionFailedException(string message)
        : base(message)
    {
    }

    [Obsolete("This constructor uses a base Exception constructor that has been deprecated by Microsoft.")]
    protected AssertionFailedException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
