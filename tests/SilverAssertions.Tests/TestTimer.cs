using System;

using ITimer = SilverAssertions.Common.ITimer;

namespace SilverAssertions.Tests;

internal sealed class TestTimer : ITimer
{
    private readonly Func<TimeSpan> getElapsed;

    public TestTimer(Func<TimeSpan> getElapsed)
    {
        this.getElapsed = getElapsed;
    }

    public TimeSpan Elapsed => getElapsed();

    public void Dispose()
    {
    }
}
