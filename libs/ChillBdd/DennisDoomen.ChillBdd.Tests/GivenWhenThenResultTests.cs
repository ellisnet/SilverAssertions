//IMPORTANT NOTE: The code in this file was retrieved from https://github.com/ChillBDD/Chill on 2/8/2025 - where no open source
//    (or other) code licenses or copyrights are asserted. It seems likely that the code below is in the public domain.
//  However, according NuGet - https://www.nuget.org/packages/Chill - as of 2/8/2025, Chill version 4.1.0 published 12/4/2020 -
//    the code for the Chill package is available via the permissible MIT open source license: https://opensource.org/license/MIT

using SilverAssertions;
using Xunit;

namespace DennisDoomen.ChillBdd.Tests
{
    public class GivenWhenThenResultTests : GivenWhenThen<string>
    {
        [Fact]
        public void When_a_deferred_executed_when_later_is_invoked_it_should_return_the_result()
        {
            WhenLater(() => "hello");

            WhenAction();

            Result.Should().Be("hello");
        }
    }
}