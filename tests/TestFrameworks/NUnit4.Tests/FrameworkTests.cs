using NUnit.Framework;
using SilverAssertions;
using System;

namespace NUnit4.Tests;

[TestFixture]
public class FrameworkTests
{
    [Test]
    public void When_nunit4_is_used_it_should_throw_nunit_exceptions_for_assertion_failures()
    {
        // Act
        Action act = () => 0.Should().Be(1);

        // Assert
        Exception exception = act.Should().Throw<Exception>().Which;
        exception.GetType()
            .FullName.Should()
            .ContainEquivalentOf("NUnit.Framework.AssertionException");
    }
}
