# SilverAssertions

Fluent API for asserting the results of unit tests that targets .NET.
SilverAssertions allows you to more naturally specify the expected outcome of a
test using a fluent, readable syntax.

SilverAssertions is provided as a .NET 10 library and associated
`SilverAssertions.ApacheLicenseForever` NuGet package. SilverAssertions supports
applications and assemblies that target Microsoft .NET version 10.0 and later.
Microsoft .NET version 10.0 is a Long-Term Supported (LTS) version of .NET, and
was released on Nov 11, 2025; and will be actively supported by Microsoft until
Nov 14, 2028. Please update your C#/.NET code and projects to the latest LTS
version of Microsoft .NET.

The package id carries the `ApacheLicenseForever` suffix as a commitment: it is
published under the Apache License 2.0 and will not be switched to another
license. The assembly and namespace root are plain `SilverAssertions`.

## Installation

```
dotnet add package SilverAssertions.ApacheLicenseForever
```

SilverAssertions is an assertion library only - bring your own test framework.
It detects xUnit, NUnit, MSTest and MSpec automatically and throws that
framework's own assertion-failure exception.

If your test project uses xunit.v3 4.x on the .NET 10 SDK, add a `global.json`
beside your solution so `dotnet test` runs it through Microsoft Testing
Platform:

```json
{
  "test": {
    "runner": "Microsoft.Testing.Platform"
  }
}
```

Without it, `dotnet test` stops with *"Testing with VSTest target is no longer
supported by Microsoft.Testing.Platform on .NET 10 SDK and later."* Note also
that `--nologo` is a VSTest-only switch: passing it in this mode reports
"Zero tests ran" without running anything. Use `dotnet test -- --no-banner`.

## SilverAssertions supports:

* String assertions
* Numeric assertions
* Boolean assertions
* DateTime and DateOnly assertions
* TimeSpan and TimeOnly assertions
* Collection assertions
* Dictionary assertions
* Guid assertions
* Enum assertions
* Object graph comparison (equivalency)
* Type and member assertions
* Exception assertions
* Event monitoring
* Stream assertions
* XML and XDocument assertions
* HttpResponseMessage assertions
* Execution time assertions
* Data table, data row, and data column assertions
* Extensible formatting
* Multiple test frameworks (xUnit, NUnit, MSTest, MSpec)
* Many more...

## Sample Code

### Basic Assertions

```csharp
using SilverAssertions;

string name = "SilverAssertions";
name.Should().StartWith("Silver").And.EndWith("Assertions").And.HaveLength(16);

int value = 42;
value.Should().BeGreaterThan(0).And.BeLessThan(100);

bool isActive = true;
isActive.Should().BeTrue();
```

### Collection Assertions

```csharp
using SilverAssertions;

var numbers = new[] { 1, 2, 3, 4, 5 };
numbers.Should().HaveCount(5).And.Contain(3).And.BeInAscendingOrder();

var names = new[] { "Alice", "Bob", "Charlie" };
names.Should().OnlyContain(n => n.Length > 2);
names.Should().ContainSingle(n => n.StartsWith("A"));
```

### Exception Assertions

```csharp
using SilverAssertions;

Action act = () => throw new InvalidOperationException("something went wrong");

act.Should().Throw<InvalidOperationException>()
    .WithMessage("something went wrong");
```

### Async Exception Assertions

```csharp
using SilverAssertions;

Func<Task> act = async () =>
{
    await Task.Delay(1);
    throw new InvalidOperationException("async failure");
};

await act.Should().ThrowAsync<InvalidOperationException>()
    .WithMessage("async failure");
```

### Object Graph Comparison

```csharp
using SilverAssertions;

var expected = new { Name = "Alice", Age = 30 };
var actual = new { Name = "Alice", Age = 30 };

actual.Should().BeEquivalentTo(expected);
```

### DateTime Assertions

```csharp
using SilverAssertions;

var dateTime = new DateTime(2025, 7, 4, 12, 0, 0);
dateTime.Should().BeAfter(new DateTime(2025, 1, 1));
dateTime.Should().HaveYear(2025).And.HaveMonth(7).And.HaveDay(4);
```

## Documentation

- **AGENT-README.txt** - the complete API reference, with worked examples,
  common pitfalls and the extensibility points. It ships inside the NuGet
  package, so installing SilverAssertions already gives you a local copy;
  point your AI coding agent at that file when it is writing code against this
  library. It is also the most thorough documentation available and reads
  perfectly well for humans, and it is
  [readable on GitHub](https://github.com/ellisnet/SilverAssertions/blob/main/AGENT-README.txt).
- **[SilverAssertions.Tests](https://github.com/ellisnet/SilverAssertions/tree/main/tests/SilverAssertions.Tests)** and
  **[SilverAssertions.Equivalency.Tests](https://github.com/ellisnet/SilverAssertions/tree/main/tests/SilverAssertions.Equivalency.Tests)**
  - the test suites double as executable documentation, with far more sample
  code than this page.

## License

SilverAssertions is licensed under the Apache License 2.0 - see the
[LICENSE](https://github.com/ellisnet/SilverAssertions/blob/main/LICENSE) file.

For licensing and provenance information about the open source code included in
this package, see [THIRD-PARTY-NOTICES.txt](https://github.com/ellisnet/SilverAssertions/blob/main/THIRD-PARTY-NOTICES.txt).
