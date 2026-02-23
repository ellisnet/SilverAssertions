using Xunit;

namespace SilverAssertions.Tests;

// Try to stabilize UIFact tests
[CollectionDefinition("UIFacts", DisableParallelization = true)]
public class UIFactsDefinition;
