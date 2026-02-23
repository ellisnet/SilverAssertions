using System;
using SilverAssertions.Collections;
using Xunit;
using Xunit.Sdk;

namespace SilverAssertions.Tests;

public class AndWhichConstraintTests
{
    [Fact]
    public void When_many_objects_are_provided_accessing_which_should_throw_a_descriptive_exception()
    {
        // Arrange
        var continuation = new AndWhichConstraint<StringCollectionAssertions, string>(null, new[] { "hello", "world" });

        // Act
        Action act = () => _ = continuation.Which;

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage(
                "More than one object found.  SilverAssertions cannot determine which object is meant.*")
            .WithMessage("*Found objects:*\"hello\"*\"world\"");
    }
}
