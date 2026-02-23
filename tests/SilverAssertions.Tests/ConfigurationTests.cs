using System;
using System.Threading.Tasks;
using SilverAssertions.Common;
using Xunit;

namespace SilverAssertions.Tests;

[Collection("ConfigurationTests")]
public class ConfigurationTests
{
    [Fact]
    public void When_concurrently_accessing_current_Configuration_no_exception_should_be_thrown()
    {
        // Act
        Action act = () => Parallel.For(
            0,
            10000,
            new ParallelOptions
            {
                MaxDegreeOfParallelism = 8
            },
            __ =>
            {
                Configuration.Current.ValueFormatterAssembly = string.Empty;
                _ = Configuration.Current.ValueFormatterDetectionMode;
            }
        );

        // Assert
        act.Should().NotThrow();
    }
}

// Due to tests that call Configuration.Current
[CollectionDefinition("ConfigurationTests", DisableParallelization = true)]
public class ConfigurationTestsDefinition;
