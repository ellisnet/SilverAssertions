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

SilverAssertions is a fork of the code of the popular FluentAssertions library
version 7.1.0 - see below for licensing details.

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

Note that significant additional sample code is available in the
`SilverAssertions.Tests` project; and in the
`SilverAssertions.Equivalency.Tests` project.

## License

The project is licensed under the Apache License, Version 2.0. see:
https://en.wikipedia.org/wiki/Apache_License

All code from FluentAssertions version 7.1.0 was licensed under the Apache
License, version 2.0 - as of Jan 27, 2025. This project (SilverAssertions)
complies with all provisions of the open source license of FluentAssertions
version 7.1.0 (code) - and will make all modified, adapted and derived code
freely available as open source, under the same license as the FluentAssertions
code license - version 7.1.0 - Apache License, version 2.0.
