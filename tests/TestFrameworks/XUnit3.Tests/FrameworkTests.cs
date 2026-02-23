using SilverAssertions;
using System;
using Xunit;

namespace XUnit3.Tests;

public class FrameworkTests
{
    [Fact]
    public void When_xunit3_is_used_it_should_throw_xunit_exceptions_for_assertion_failures()
    {
        // Act
        Action act = () => 0.Should().Be(1);

        // Assert
        Exception exception = act.Should().Throw<Exception>().Which;
        exception.GetType().FullName.Should().ContainEquivalentOf("xunit");
    }
}
