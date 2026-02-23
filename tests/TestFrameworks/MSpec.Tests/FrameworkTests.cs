using Machine.Specifications;
using SilverAssertions;
using System;

namespace MSpec.Tests;

[Subject("FrameworkTests")]
public class When_mspec_is_used
{
    Because of = () => Exception = Catch.Exception(() => 0.Should().Be(1));

    It should_fail = () => Exception.Should().NotBeNull().And.BeAssignableTo<Exception>();
    It should_have_a_specific_reason = () => Exception.GetType().FullName.Should().ContainEquivalentOf("Machine.Specifications.SpecificationException");

    private static Exception Exception;
}
