================================================================================
AGENT-README: SilverAssertions
A Comprehensive Guide for AI Coding Agents
================================================================================

OVERVIEW
--------
SilverAssertions is a fluent API for asserting the results of unit tests in .NET.
It allows you to naturally specify expected outcomes using a readable, chainable
syntax based on the .Should() extension method pattern.

It is a fork of the popular FluentAssertions library version 7.1.0, licensed
under Apache License 2.0.

IMPORTANT: If you are familiar with FluentAssertions, the API surface of
SilverAssertions is essentially identical. However, the namespace is
"SilverAssertions" instead of "FluentAssertions". Do NOT mix the two libraries.

Source Repository: https://github.com/ellisnet/SilverAssertions
License: Apache License 2.0

================================================================================

INSTALLATION
------------
NuGet Package: SilverAssertions.ApacheLicenseForever
Dependencies (transitive, automatically restored - no specific versions pinned
in this document because they change frequently):
  - JetBrains.Annotations
  - System.Configuration.ConfigurationManager

Requirements: .NET 10.0 or higher

To add to a .NET 10+ test project:

    dotnet add package SilverAssertions.ApacheLicenseForever

Or in a .csproj file (let NuGet pick the latest compatible version):

    <PackageReference Include="SilverAssertions.ApacheLicenseForever" />

IMPORTANT: The package name is "SilverAssertions.ApacheLicenseForever" (not just
"SilverAssertions"). Always use this full package name when installing.

================================================================================

KEY NAMESPACE
--------------

    using SilverAssertions;

That's it. One namespace gives you access to all assertion methods via the
.Should() extension method. This is the ONLY using statement you need for
the vast majority of assertion scenarios.

================================================================================

SUPPORTED TEST FRAMEWORKS
---------------------------
SilverAssertions works with all major .NET test frameworks:

  - xUnit v3
  - NUnit v4
  - MSTest v4
  - MSpec (Machine.Specifications)

No additional configuration is needed. SilverAssertions auto-detects the test
framework in use.

================================================================================

CORE CONCEPT: THE .Should() PATTERN
=====================================

All assertions follow the same pattern:

    value.Should().BeXxx();
    value.Should().HaveXxx();
    value.Should().NotBeXxx();

Assertions can be chained with .And:

    value.Should().BeXxx().And.HaveXxx().And.NotBeXxx();

Custom failure messages can be added with "because":

    value.Should().BeTrue("because the user is authenticated");
    value.Should().BeGreaterThan(0, "because counts must be positive");

================================================================================

STRING ASSERTIONS
==================

    using SilverAssertions;

    string name = "SilverAssertions";

Equality and content:
    name.Should().Be("SilverAssertions");
    name.Should().NotBe("FluentAssertions");
    name.Should().BeEquivalentTo("silverassertions");  // Case-insensitive

Start/End:
    name.Should().StartWith("Silver");
    name.Should().EndWith("Assertions");
    name.Should().StartWithEquivalentOf("silver");     // Case-insensitive
    name.Should().EndWithEquivalentOf("ASSERTIONS");   // Case-insensitive

Contains:
    name.Should().Contain("Assert");
    name.Should().ContainAll("Silver", "Assertions");
    name.Should().ContainAny("Silver", "Gold");
    name.Should().ContainEquivalentOf("SILVER");       // Case-insensitive
    name.Should().NotContain("Fluent");

Length:
    name.Should().HaveLength(16);

Empty/Null:
    "".Should().BeEmpty();
    "".Should().BeNullOrEmpty();
    "   ".Should().BeNullOrWhiteSpace();
    name.Should().NotBeEmpty();
    name.Should().NotBeNullOrEmpty();
    name.Should().NotBeNullOrWhiteSpace();

Pattern matching:
    name.Should().Match("Silver*");                    // Wildcard pattern
    name.Should().MatchEquivalentOf("silver*");        // Case-insensitive wildcard
    name.Should().MatchRegex(@"^Silver\w+$");          // Regex pattern

One-of:
    name.Should().BeOneOf("SilverAssertions", "FluentAssertions");

Upper/Lower case:
    "HELLO".Should().BeUpperCased();
    "hello".Should().BeLowerCased();

Chaining:
    name.Should().StartWith("Silver").And.EndWith("Assertions").And.HaveLength(16);

================================================================================

NUMERIC ASSERTIONS
===================

    using SilverAssertions;

Works with: int, long, short, byte, float, double, decimal, and their
nullable equivalents (int?, double?, etc.)

    int value = 42;

Equality:
    value.Should().Be(42);
    value.Should().NotBe(0);

Comparison:
    value.Should().BeGreaterThan(0);
    value.Should().BeGreaterThanOrEqualTo(42);
    value.Should().BeLessThan(100);
    value.Should().BeLessThanOrEqualTo(42);
    value.Should().BePositive();
    value.Should().BeNegative();

Range:
    value.Should().BeInRange(1, 100);
    value.Should().NotBeInRange(200, 300);

Set membership:
    value.Should().BeOneOf(40, 41, 42, 43);

Approximate equality (for floating-point):
    double result = 3.14159;
    result.Should().BeApproximately(3.14, 0.01);       // Within precision
    result.Should().NotBeApproximately(5.0, 0.01);

Closeness:
    result.Should().BeCloseTo(3.14, 0.01);             // Within delta
    result.Should().NotBeCloseTo(5.0, 0.01);

Chaining:
    value.Should().BeGreaterThan(0).And.BeLessThan(100);

================================================================================

BOOLEAN ASSERTIONS
===================

    bool isActive = true;

    isActive.Should().BeTrue();
    isActive.Should().NotBeFalse();

    bool isDeleted = false;
    isDeleted.Should().BeFalse();
    isDeleted.Should().NotBeTrue();

Nullable booleans:
    bool? maybeTrue = true;
    maybeTrue.Should().BeTrue();
    maybeTrue.Should().HaveValue();
    maybeTrue.Should().NotBeNull();

    bool? nothing = null;
    nothing.Should().NotHaveValue();
    nothing.Should().BeNull();

================================================================================

COLLECTION ASSERTIONS
======================

    using SilverAssertions;

Works with: arrays, List<T>, IEnumerable<T>, ICollection<T>, etc.

    var numbers = new[] { 1, 2, 3, 4, 5 };

Count:
    numbers.Should().HaveCount(5);
    numbers.Should().HaveCountGreaterThan(3);
    numbers.Should().HaveCountGreaterThanOrEqualTo(5);
    numbers.Should().HaveCountLessThan(10);
    numbers.Should().HaveCountLessThanOrEqualTo(5);
    numbers.Should().HaveSameCount(new[] { 10, 20, 30, 40, 50 });

Contains:
    numbers.Should().Contain(3);
    numbers.Should().Contain(new[] { 2, 4 });
    numbers.Should().ContainSingle(n => n > 4);        // Exactly one match
    numbers.Should().ContainItemsAssignableTo<int>();
    numbers.Should().NotContain(99);
    numbers.Should().NotContainNulls();

Ordering:
    numbers.Should().BeInAscendingOrder();
    new[] { 5, 4, 3 }.Should().BeInDescendingOrder();
    numbers.Should().ContainInOrder(1, 2, 3);          // In sequence (not necessarily consecutive)
    numbers.Should().ContainInConsecutiveOrder(1, 2, 3); // Must be consecutive

Equality:
    numbers.Should().Equal(1, 2, 3, 4, 5);             // Exact order match
    numbers.Should().BeEquivalentTo(new[] { 5, 3, 1, 4, 2 }); // Same items, any order

Subset:
    numbers.Should().BeSubsetOf(new[] { 1, 2, 3, 4, 5, 6, 7 });

Intersection:
    numbers.Should().IntersectWith(new[] { 3, 4, 5, 6, 7 });

Empty/Null:
    new int[0].Should().BeEmpty();
    numbers.Should().NotBeEmpty();
    ((int[])null).Should().BeNull();
    numbers.Should().NotBeNull();
    new int[0].Should().BeNullOrEmpty();

Element access:
    numbers.Should().HaveElementAt(0, 1);              // Element at index 0 is 1
    numbers.Should().HaveElementPreceding(3, 2);       // 2 precedes 3
    numbers.Should().HaveElementSucceeding(3, 4);      // 4 succeeds 3
    numbers.Should().StartWith(1);
    numbers.Should().EndWith(5);

Content validation:
    numbers.Should().OnlyContain(n => n > 0);          // All elements match predicate
    numbers.Should().OnlyHaveUniqueItems();

Satisfy:
    numbers.Should().AllSatisfy(n => n.Should().BePositive());
    numbers.Should().SatisfyRespectively(
        n => n.Should().Be(1),
        n => n.Should().Be(2),
        n => n.Should().Be(3),
        n => n.Should().Be(4),
        n => n.Should().Be(5));
    numbers.Should().Satisfy(
        n => n > 0,
        n => n > 1,
        n => n > 2,
        n => n > 3,
        n => n > 4);

Type checking:
    var mixed = new object[] { "hello", 42, true };
    mixed.Should().AllBeAssignableTo<object>();
    mixed.Should().ContainItemsAssignableTo<string>();
    mixed.Should().NotContainItemsAssignableTo<double>();

Equivalency:
    numbers.Should().ContainEquivalentOf(3);
    numbers.Should().BeEquivalentTo(new[] { 5, 4, 3, 2, 1 });

================================================================================

DICTIONARY ASSERTIONS
======================

    var dict = new Dictionary<string, int>
    {
        ["Alice"] = 30,
        ["Bob"] = 25,
        ["Charlie"] = 35
    };

    dict.Should().ContainKey("Alice");
    dict.Should().NotContainKey("Dave");
    dict.Should().ContainValue(30);
    dict.Should().NotContainValue(99);
    dict.Should().ContainKeys("Alice", "Bob");
    dict.Should().ContainValues(30, 25);
    dict.Should().HaveCount(3);
    dict.Should().NotBeEmpty();

================================================================================

DATETIME ASSERTIONS
====================

    var dateTime = new DateTime(2025, 7, 4, 12, 0, 0);

Comparison:
    dateTime.Should().BeAfter(new DateTime(2025, 1, 1));
    dateTime.Should().BeBefore(new DateTime(2025, 12, 31));
    dateTime.Should().BeOnOrAfter(new DateTime(2025, 7, 4));
    dateTime.Should().BeOnOrBefore(new DateTime(2025, 7, 4));

Component access:
    dateTime.Should().HaveYear(2025);
    dateTime.Should().HaveMonth(7);
    dateTime.Should().HaveDay(4);
    dateTime.Should().HaveHour(12);
    dateTime.Should().HaveMinute(0);
    dateTime.Should().HaveSecond(0);

Chaining:
    dateTime.Should().HaveYear(2025).And.HaveMonth(7).And.HaveDay(4);

Closeness:
    dateTime.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
    dateTime.Should().NotBeCloseTo(DateTime.MinValue, TimeSpan.FromDays(1));

DateOnly (date without time):
    var date = new DateOnly(2025, 7, 4);
    date.Should().Be(new DateOnly(2025, 7, 4));

DateTimeOffset:
    var dto = new DateTimeOffset(2025, 7, 4, 12, 0, 0, TimeSpan.Zero);
    dto.Should().BeAfter(DateTimeOffset.MinValue);
    dto.Should().HaveOffset(TimeSpan.Zero);

================================================================================

TIMESPAN ASSERTIONS
====================

    var timeSpan = TimeSpan.FromMinutes(5);

    timeSpan.Should().BeGreaterThan(TimeSpan.FromMinutes(1));
    timeSpan.Should().BeLessThan(TimeSpan.FromMinutes(10));
    timeSpan.Should().BePositive();
    timeSpan.Should().BeCloseTo(TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(1));

TimeOnly:
    var time = new TimeOnly(14, 30, 0);
    time.Should().Be(new TimeOnly(14, 30, 0));

================================================================================

GUID ASSERTIONS
================

    var guid = Guid.NewGuid();

    guid.Should().NotBeEmpty();
    guid.Should().NotBe(Guid.Empty);

    Guid.Empty.Should().BeEmpty();

Nullable:
    Guid? nullGuid = null;
    nullGuid.Should().BeNull();
    nullGuid.Should().NotHaveValue();

================================================================================

ENUM ASSERTIONS
================

    var dayOfWeek = DayOfWeek.Monday;

    dayOfWeek.Should().Be(DayOfWeek.Monday);
    dayOfWeek.Should().NotBe(DayOfWeek.Friday);
    dayOfWeek.Should().HaveFlag(DayOfWeek.Monday);
    dayOfWeek.Should().BeDefined();

================================================================================

OBJECT ASSERTIONS
==================

    object obj = "hello";

    obj.Should().NotBeNull();
    obj.Should().BeOfType<string>();
    obj.Should().BeAssignableTo<IComparable>();
    obj.Should().NotBeOfType<int>();

    object obj2 = obj;
    obj.Should().BeSameAs(obj2);          // Reference equality
    obj.Should().NotBeSameAs(new object());

================================================================================

EXCEPTION ASSERTIONS
=====================

Synchronous exceptions:

    Action act = () => throw new InvalidOperationException("something went wrong");

    act.Should().Throw<InvalidOperationException>();
    act.Should().Throw<InvalidOperationException>()
        .WithMessage("something went wrong");
    act.Should().Throw<InvalidOperationException>()
        .WithMessage("*went wrong*");                  // Wildcard matching

Exact type matching (no subclasses):

    act.Should().ThrowExactly<InvalidOperationException>();

No exception expected:

    Action goodAct = () => { /* does nothing */ };
    goodAct.Should().NotThrow();
    goodAct.Should().NotThrow<InvalidOperationException>();

Async exceptions:

    Func<Task> asyncAct = async () =>
    {
        await Task.Delay(1);
        throw new InvalidOperationException("async failure");
    };

    await asyncAct.Should().ThrowAsync<InvalidOperationException>()
        .WithMessage("async failure");

    Func<Task> goodAsyncAct = async () => await Task.CompletedTask;
    await goodAsyncAct.Should().NotThrowAsync();

Inner exception assertions:

    act.Should().Throw<InvalidOperationException>()
        .WithInnerException<ArgumentNullException>();

    act.Should().Throw<InvalidOperationException>()
        .WithInnerException<ArgumentNullException>()
        .WithMessage("Value cannot be null*");

Function return + exception:

    Func<int> func = () => throw new InvalidOperationException();
    func.Should().Throw<InvalidOperationException>();

    Func<int> goodFunc = () => 42;
    goodFunc.Should().NotThrow();

================================================================================

OBJECT GRAPH COMPARISON (EQUIVALENCY)
=======================================

Deep comparison of object graphs by value, not by reference:

    var expected = new { Name = "Alice", Age = 30 };
    var actual = new { Name = "Alice", Age = 30 };

    actual.Should().BeEquivalentTo(expected);

With options:

    actual.Should().BeEquivalentTo(expected, options => options
        .Excluding(x => x.Age));                       // Exclude specific properties

    actual.Should().BeEquivalentTo(expected, options => options
        .Including(x => x.Name));                      // Include only specific properties

Nested objects:

    var expected = new
    {
        Name = "Alice",
        Address = new { City = "Seattle", State = "WA" }
    };

    actual.Should().BeEquivalentTo(expected);          // Deep comparison

Collections:

    var expectedList = new[] { new { Id = 1 }, new { Id = 2 } };
    var actualList = new[] { new { Id = 2 }, new { Id = 1 } };

    actualList.Should().BeEquivalentTo(expectedList);  // Order-independent by default

Records and tuples:

    // Records are compared by value
    // Tuples are compared element-by-element

Cyclic references:

    // Handled automatically without infinite loops

DataTable/DataRow/DataSet:

    // Full support for System.Data types
    dataTable.Should().BeEquivalentTo(expectedTable);

Enums in equivalency:

    // Enum comparison works with BeEquivalentTo

================================================================================

TYPE AND MEMBER ASSERTIONS
===========================

Type assertions:

    typeof(string).Should().Be(typeof(string));
    typeof(string).Should().BeAbstract();              // (would fail for string)
    typeof(string).Should().BeSealed();
    typeof(string).Should().BeAssignableTo<IComparable>();
    typeof(string).Should().BeDerivedFrom<object>();
    typeof(string).Should().Implement<IComparable>();
    typeof(string).Should().HaveProperty<int>("Length");
    typeof(string).Should().HaveMethod("Contains", new[] { typeof(string) });
    typeof(string).Should().HaveConstructor(new[] { typeof(char[]) });
    typeof(string).Should().HaveDefaultConstructor();  // (would fail for string)
    typeof(string).Should().HaveAccessModifier(CSharpAccessModifier.Public);
    typeof(string).Should().BeDecoratedWith<SerializableAttribute>();
    typeof(string).Should().BeDecoratedWithOrInherit<SerializableAttribute>();
    typeof(string).Should().HaveExplicitMethod(typeof(IConvertible), "ToInt32");
    typeof(string).Should().HaveImplicitConversionOperator<string, ReadOnlySpan<char>>();
    typeof(string).Should().HaveExplicitConversionOperator<string, char[]>();
    typeof(string).Should().HaveIndexer(typeof(int));  // string[int]
    typeof(string).Should().NotBeStatic();

Method assertions:

    var methodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });
    methodInfo.Should().BeVirtual();
    methodInfo.Should().NotBeVirtual();
    methodInfo.Should().ReturnVoidOrTaskVoid();         // (would fail)
    methodInfo.Should().Return<bool>();

Property assertions:

    var propertyInfo = typeof(string).GetProperty("Length");
    propertyInfo.Should().BeReadable();
    propertyInfo.Should().NotBeWritable();
    propertyInfo.Should().Return<int>();

Assembly assertions:

    typeof(string).Assembly.Should().NotReference(typeof(SilverAssertions).Assembly);

Type selector (batch assertions on multiple types):

    typeof(MyClass).Assembly.GetTypes()
        .ThatAreClasses()
        .ThatImplement<IDisposable>()
        .Should().BeSealed();

================================================================================

EVENT MONITORING
=================

    using SilverAssertions;

    var subject = new MyClass();

    using var monitor = subject.Monitor();

    subject.DoSomething(); // Triggers PropertyChanged event

    monitor.Should().Raise("PropertyChanged");
    monitor.Should().Raise("PropertyChanged")
        .WithSender(subject)
        .WithArgs<PropertyChangedEventArgs>(args => args.PropertyName == "Name");

    monitor.Should().NotRaise("SomeOtherEvent");

================================================================================

STREAM ASSERTIONS
==================

    using var stream = new MemoryStream(new byte[] { 1, 2, 3 });

    stream.Should().NotBeNull();
    stream.Should().BeReadable();
    stream.Should().BeSeekable();
    stream.Should().BeWritable();
    stream.Should().HaveLength(3);
    stream.Should().HavePosition(0);
    stream.Should().NotHaveLength(0);

Buffered streams:

    using var bufferedStream = new BufferedStream(stream);
    bufferedStream.Should().NotBeNull();

================================================================================

XML AND XDOCUMENT ASSERTIONS
==============================

XDocument:
    var doc = XDocument.Parse("<root><child>value</child></root>");
    doc.Should().HaveRoot("root");
    doc.Should().HaveElement("child");

XElement:
    var element = XElement.Parse("<item id='1'>content</item>");
    element.Should().HaveAttribute("id", "1");
    element.Should().HaveValue("content");
    element.Should().HaveElement("child");             // (would fail)

XAttribute:
    var attr = new XAttribute("name", "value");
    attr.Should().HaveValue("value");

XmlNode / XmlElement:
    // Traditional System.Xml types are also supported

================================================================================

HTTP RESPONSE ASSERTIONS
==========================

    var response = new HttpResponseMessage(HttpStatusCode.OK);

    response.Should().BeSuccessful();                  // 2xx
    response.Should().HaveStatusCode(HttpStatusCode.OK);
    response.Should().NotHaveStatusCode(HttpStatusCode.NotFound);

================================================================================

EXECUTION TIME ASSERTIONS
===========================

    Action act = () => Thread.Sleep(100);

    act.ExecutionTime().Should().BeLessThan(TimeSpan.FromSeconds(1));
    act.ExecutionTime().Should().BeGreaterThan(TimeSpan.FromMilliseconds(50));
    act.ExecutionTime().Should().BeCloseTo(TimeSpan.FromMilliseconds(100),
        TimeSpan.FromMilliseconds(50));

================================================================================

ASSERTION SCOPE
================

AssertionScope collects multiple assertion failures before reporting:

    using (new AssertionScope())
    {
        5.Should().Be(10);                             // Fails but continues
        "hello".Should().Be("world");                  // Also fails
        true.Should().BeFalse();                       // Also fails
    }
    // All three failures are reported together at scope disposal

This is useful when you want to check multiple properties at once and see
all failures, rather than stopping at the first one.

================================================================================

EXTENSIBILITY
==============

SilverAssertions can be extended with custom assertions by creating extension
methods on the appropriate assertion class.

================================================================================

COMPLETE EXAMPLES
=================

Example 1: Testing a Service Method
--------------------------------------
    using SilverAssertions;

    [Fact]
    public void GetUser_ReturnsExpectedUser()
    {
        // Arrange
        var service = new UserService();

        // Act
        var user = service.GetUser(1);

        // Assert
        user.Should().NotBeNull();
        user.Name.Should().Be("Alice");
        user.Age.Should().BeGreaterThan(0).And.BeLessThan(150);
        user.Email.Should().Contain("@").And.EndWith(".com");
        user.Roles.Should().NotBeEmpty().And.Contain("admin");
    }

Example 2: Testing Exception Behavior
---------------------------------------
    using SilverAssertions;

    [Fact]
    public void GetUser_WithInvalidId_ThrowsException()
    {
        var service = new UserService();

        Action act = () => service.GetUser(-1);

        act.Should().Throw<ArgumentException>()
            .WithMessage("*invalid*")
            .And.ParamName.Should().Be("id");
    }

Example 3: Testing Async Operations
--------------------------------------
    using SilverAssertions;

    [Fact]
    public async Task GetUsersAsync_ReturnsAllUsers()
    {
        var service = new UserService();

        var users = await service.GetUsersAsync();

        users.Should().HaveCountGreaterThan(0);
        users.Should().OnlyContain(u => u.IsActive);
        users.Should().BeInAscendingOrder(u => u.Name);
    }

Example 4: Object Graph Comparison
-------------------------------------
    using SilverAssertions;

    [Fact]
    public void CreateOrder_ReturnsCorrectOrder()
    {
        var service = new OrderService();

        var order = service.CreateOrder(userId: 1, productId: 42, quantity: 3);

        order.Should().BeEquivalentTo(new
        {
            UserId = 1,
            ProductId = 42,
            Quantity = 3,
            Status = OrderStatus.Pending
        }, options => options.Excluding(o => o.Id).Excluding(o => o.CreatedAt));
    }

Example 5: Collection Validation with AssertionScope
------------------------------------------------------
    using SilverAssertions;

    [Fact]
    public void ProcessItems_AllItemsAreValid()
    {
        var items = GetProcessedItems();

        using (new AssertionScope())
        {
            items.Should().NotBeEmpty();
            items.Should().OnlyHaveUniqueItems(i => i.Id);
            items.Should().AllSatisfy(item =>
            {
                item.Name.Should().NotBeNullOrWhiteSpace();
                item.Price.Should().BePositive();
                item.CreatedAt.Should().BeBefore(DateTime.Now);
            });
        }
    }

================================================================================

WHAT THIS LIBRARY DOES NOT DO
===============================

Do NOT attempt to use SilverAssertions for the following - it will not work:

  - Mocking or stubbing (use Moq, NSubstitute, or FakeItEasy instead)
  - Test discovery or test running (that's the test framework's job)
  - Code coverage measurement
  - Performance benchmarking (use BenchmarkDotNet instead)
  - Integration test infrastructure (use WebApplicationFactory, TestContainers)
  - Snapshot testing
  - Property-based testing (use FsCheck or Hedgehog instead)
  - UI testing or browser automation

This library IS for: writing readable, fluent assertion statements in unit
tests, regardless of which test framework you use.

================================================================================

MINIMUM VIABLE PROJECT TEMPLATE
=================================

To scaffold a new .NET 10 xUnit test project with SilverAssertions:

    dotnet new xunit -n MyProject.Tests --framework net10.0
    cd MyProject.Tests
    dotnet add package SilverAssertions.ApacheLicenseForever

Then in a test file:

    using SilverAssertions;

    public class MyTests
    {
        [Fact]
        public void Example_Test()
        {
            var result = 2 + 2;
            result.Should().Be(4);
        }
    }

Build and run tests:

    dotnet build
    dotnet test

================================================================================

PERFORMANCE TIPS FOR CODING AGENTS
====================================

1. USE AssertionScope: When testing multiple properties of an object, wrap
   assertions in an AssertionScope so all failures are reported at once rather
   than stopping at the first failure.

2. PREFER BeEquivalentTo FOR OBJECT COMPARISON: Instead of writing individual
   assertions for each property, use BeEquivalentTo with an anonymous object
   for cleaner and more maintainable tests.

3. CHAIN WITH .And: Use .And for related assertions on the same value rather
   than repeating .Should() calls. This is both more readable and efficient.

4. USE WILDCARDS IN WithMessage: Use "*" wildcards in exception message
   assertions to avoid brittle tests that break when message text changes
   slightly.

5. USE OnlyContain FOR BULK VALIDATION: When all elements in a collection
   must satisfy a condition, OnlyContain is cleaner than iterating manually.

6. PREFER SPECIFIC ASSERTIONS: Use specific methods like BePositive() instead
   of BeGreaterThan(0), and BeEmpty() instead of HaveCount(0). They produce
   better failure messages.

================================================================================

COMMON PITFALLS TO AVOID
=========================

1. DO NOT confuse the NuGet package name with the namespace.
   - Package: SilverAssertions.ApacheLicenseForever
   - Namespace: SilverAssertions (using SilverAssertions;)

2. DO NOT use FluentAssertions namespaces. Even though this is a fork,
   the namespace is SilverAssertions.

3. DO NOT forget the .Should() call. Writing "value.Be(42)" won't compile.
   Always write "value.Should().Be(42)".

4. DO NOT target .NET versions below 10.0. This library requires .NET 10+.

5. DO NOT use Throw<T> for async methods. Use ThrowAsync<T> instead.
   For async methods, the pattern is:
     Func<Task> act = async () => { ... };
     await act.Should().ThrowAsync<Exception>();

6. DO NOT forget that BeEquivalentTo is order-independent for collections
   by default. If order matters, use Equal() or ContainInOrder() instead.

7. DO NOT use Be() for floating-point equality. Use BeApproximately() or
   BeCloseTo() to account for floating-point precision.

================================================================================

DEEPER LEARNING: TEST FILE CROSS-REFERENCES
=============================================

The SilverAssertions source repository contains extensive test files that
demonstrate every assertion method. If the documentation above is not sufficient,
fetch and read the relevant test file from:

    https://github.com/ellisnet/SilverAssertions
    Path: tests/SilverAssertions.Tests/
    Path: tests/SilverAssertions.Equivalency.Tests/

Feature-to-test-file mapping:

  String assertions (Be, BeEmpty, Contain, StartWith, EndWith, Match,
  MatchRegex, HaveLength, BeUpperCased, BeLowerCased, BeOneOf, etc.):
    -> tests/SilverAssertions.Tests/Primitives/StringAssertionTests.*.cs
       (Multiple partial class files, one per method group)

  Numeric assertions (Be, BePositive, BeGreaterThan, BeInRange,
  BeApproximately, BeCloseTo, BeOneOf):
    -> tests/SilverAssertions.Tests/Numeric/NumericAssertionTests.cs

  Nullable numeric assertions:
    -> tests/SilverAssertions.Tests/Numeric/NullableNumericAssertionTests.cs

  Numeric difference assertions:
    -> tests/SilverAssertions.Tests/Numeric/NumericDifferenceAssertionsTests.cs

  Comparable assertions:
    -> tests/SilverAssertions.Tests/Numeric/ComparableTests.cs

  Boolean assertions:
    -> tests/SilverAssertions.Tests/Primitives/BooleanAssertionTests.cs
    -> tests/SilverAssertions.Tests/Primitives/NullableBooleanAssertionTests.cs

  DateTime, DateOnly, DateTimeOffset assertions:
    -> tests/SilverAssertions.Tests/Primitives/DateTimeAssertionTests.cs
    -> tests/SilverAssertions.Tests/Primitives/DateOnlyAssertionTests.cs
    -> tests/SilverAssertions.Tests/Primitives/DateTimeOffsetAssertionTests.cs

  TimeSpan and TimeOnly assertions:
    -> tests/SilverAssertions.Tests/Primitives/SimpleTimeSpanAssertionTests.cs
    -> tests/SilverAssertions.Tests/Primitives/NullableSimpleTimeSpanAssertionTests.cs
    -> tests/SilverAssertions.Tests/Primitives/TimeOnlyAssertionTests.cs

  Guid assertions:
    -> tests/SilverAssertions.Tests/Primitives/GuidAssertionTests.cs
    -> tests/SilverAssertions.Tests/Primitives/NullableGuidAssertionTests.cs

  Enum assertions:
    -> tests/SilverAssertions.Tests/Primitives/EnumAssertionTests.cs

  Object assertions:
    -> tests/SilverAssertions.Tests/Primitives/ObjectAssertionTests.cs
    -> tests/SilverAssertions.Tests/Primitives/ReferenceTypeAssertionsTests.cs

  HttpResponseMessage assertions:
    -> tests/SilverAssertions.Tests/Primitives/HttpResponseMessageAssertionTests.cs

  Collection assertions (HaveCount, Contain, BeInOrder, Equal, BeSubsetOf,
  OnlyContain, Satisfy, etc.):
    -> tests/SilverAssertions.Tests/Collections/CollectionAssertionTests.*.cs
       (Multiple partial class files, one per method group)
    -> tests/SilverAssertions.Tests/Collections/GenericCollectionAssertionOfStringTests.cs

  Dictionary assertions:
    -> tests/SilverAssertions.Tests/Collections/GenericDictionaryAssertionTests.cs

  Exception assertions (Throw, ThrowExactly, WithMessage, InnerException):
    -> tests/SilverAssertions.Tests/Exceptions/ExceptionAssertionTests.cs
    -> tests/SilverAssertions.Tests/Exceptions/FunctionExceptionAssertionTests.cs
    -> tests/SilverAssertions.Tests/Exceptions/AsyncFunctionExceptionAssertionTests.cs
    -> tests/SilverAssertions.Tests/Exceptions/NotThrowTests.cs
    -> tests/SilverAssertions.Tests/Exceptions/ThrowAssertionsTests.cs
    -> tests/SilverAssertions.Tests/Exceptions/InnerExceptionTests.cs
    -> tests/SilverAssertions.Tests/Exceptions/OuterExceptionTests.cs

  Object graph equivalency (BeEquivalentTo, options, nested, cyclic, etc.):
    -> tests/SilverAssertions.Equivalency.Tests/BasicTests.cs
    -> tests/SilverAssertions.Equivalency.Tests/CollectionTests.cs
    -> tests/SilverAssertions.Equivalency.Tests/NestedPropertiesTests.cs
    -> tests/SilverAssertions.Equivalency.Tests/CyclicReferencesTests.cs
    -> tests/SilverAssertions.Equivalency.Tests/DictionaryTests.cs
    -> tests/SilverAssertions.Equivalency.Tests/RecordTests.cs
    -> tests/SilverAssertions.Equivalency.Tests/TupleTests.cs
    -> tests/SilverAssertions.Equivalency.Tests/EnumTests.cs
    -> tests/SilverAssertions.Equivalency.Tests/MemberMatchingTests.cs
    -> tests/SilverAssertions.Equivalency.Tests/SelectionRulesTests.cs
    -> tests/SilverAssertions.Equivalency.Tests/MemberConversionTests.cs

  DataTable/DataRow/DataSet equivalency:
    -> tests/SilverAssertions.Equivalency.Tests/DataTableTests.cs
    -> tests/SilverAssertions.Equivalency.Tests/DataRowTests.cs
    -> tests/SilverAssertions.Equivalency.Tests/DataSetTests.cs
    -> tests/SilverAssertions.Equivalency.Tests/DataColumnTests.cs
    -> tests/SilverAssertions.Equivalency.Tests/DataRelationTests.cs
    -> tests/SilverAssertions.Equivalency.Tests/TypedDataSetTests.cs
    -> tests/SilverAssertions.Equivalency.Tests/TypedDataTableTests.cs

  XML equivalency:
    -> tests/SilverAssertions.Equivalency.Tests/XmlTests.cs

  Event monitoring:
    -> tests/SilverAssertions.Tests/Events/EventAssertionTests.cs

  Stream assertions:
    -> tests/SilverAssertions.Tests/Streams/StreamAssertionTests.cs
    -> tests/SilverAssertions.Tests/Streams/BufferedStreamAssertionTests.cs

  XML/XDocument/XElement assertions:
    -> tests/SilverAssertions.Tests/Xml/XDocumentAssertionTests.cs
    -> tests/SilverAssertions.Tests/Xml/XElementAssertionTests.cs
    -> tests/SilverAssertions.Tests/Xml/XAttributeAssertionTests.cs
    -> tests/SilverAssertions.Tests/Xml/XmlNodeAssertionTests.cs
    -> tests/SilverAssertions.Tests/Xml/XmlElementAssertionTests.cs

  Type and member assertions (Be, BeAbstract, BeSealed, HaveProperty, etc.):
    -> tests/SilverAssertions.Tests/Types/TypeAssertionTests.*.cs
       (Multiple partial class files)
    -> tests/SilverAssertions.Tests/Types/MethodInfoAssertionTests.cs
    -> tests/SilverAssertions.Tests/Types/PropertyInfoAssertionTests.cs
    -> tests/SilverAssertions.Tests/Types/MethodInfoSelectorTests.cs
    -> tests/SilverAssertions.Tests/Types/PropertyInfoSelectorTests.cs
    -> tests/SilverAssertions.Tests/Types/TypeSelectorTests.cs

  Execution time assertions:
    -> tests/SilverAssertions.Tests/Specialized/ExecutionTimeAssertionsTests.cs

  Task assertions:
    -> tests/SilverAssertions.Tests/Specialized/TaskAssertionTests.cs
    -> tests/SilverAssertions.Tests/Specialized/TaskOfTAssertionTests.cs
    -> tests/SilverAssertions.Tests/Specialized/TaskCompletionSourceAssertionTests.cs

  Delegate assertions:
    -> tests/SilverAssertions.Tests/Specialized/DelegateAssertionTests.cs

  Assembly assertions:
    -> tests/SilverAssertions.Tests/Specialized/AssemblyAssertionTests.cs

  AggregateException assertions:
    -> tests/SilverAssertions.Tests/Specialized/AggregateExceptionAssertionTests.cs

  AssertionScope (collecting multiple failures):
    -> tests/SilverAssertions.Tests/Execution/AssertionScopeTests.cs
    -> tests/SilverAssertions.Tests/Execution/AssertionScope.ChainingApiTests.cs
    -> tests/SilverAssertions.Tests/Execution/AssertionScope.ContextDataTests.cs

  Extensibility:
    -> tests/SilverAssertions.Tests/ExtensibilityTests.cs
    -> tests/SilverAssertions.Equivalency.Tests/ExtensibilityTests.cs

  Formatting:
    -> tests/SilverAssertions.Tests/Formatting/

  Test framework compatibility (xUnit, NUnit, MSTest, MSpec):
    -> tests/TestFrameworks/XUnit3.Tests/
    -> tests/TestFrameworks/NUnit4.Tests/
    -> tests/TestFrameworks/MSTestV4.Tests/
    -> tests/TestFrameworks/MSpec.Tests/

HOW TO USE: Fetch the raw file content from GitHub using a URL like:
    https://raw.githubusercontent.com/ellisnet/SilverAssertions/main/{path}
For example:
    https://raw.githubusercontent.com/ellisnet/SilverAssertions/main/tests/SilverAssertions.Tests/Numeric/NumericAssertionTests.cs

================================================================================

QUICK REFERENCE CARD
=====================

Install:          dotnet add package SilverAssertions.ApacheLicenseForever
Namespace:        using SilverAssertions;
Pattern:          value.Should().BeXxx();
Chain:            value.Should().BeXxx().And.BeYyy();
Because:          value.Should().BeTrue("because {reason}");

--- Strings ---
Be/NotBe          "abc".Should().Be("abc")
BeEquivalentTo    "ABC".Should().BeEquivalentTo("abc")   // case-insensitive
StartWith/EndWith "abc".Should().StartWith("a")
Contain           "abc".Should().Contain("b")
HaveLength        "abc".Should().HaveLength(3)
Match             "abc".Should().Match("a*c")
MatchRegex        "abc".Should().MatchRegex("^a.c$")
BeEmpty           "".Should().BeEmpty()
BeUpperCased      "ABC".Should().BeUpperCased()

--- Numbers ---
Be/NotBe          42.Should().Be(42)
BeGreaterThan     42.Should().BeGreaterThan(0)
BeLessThan        42.Should().BeLessThan(100)
BeInRange         42.Should().BeInRange(1, 100)
BePositive        42.Should().BePositive()
BeApproximately   3.14.Should().BeApproximately(3.1, 0.1)

--- Booleans ---
BeTrue/BeFalse    true.Should().BeTrue()

--- Collections ---
HaveCount         list.Should().HaveCount(5)
Contain           list.Should().Contain(3)
BeInAscendingOrder list.Should().BeInAscendingOrder()
BeEquivalentTo    list.Should().BeEquivalentTo(other)
OnlyContain       list.Should().OnlyContain(x => x > 0)
ContainSingle     list.Should().ContainSingle(x => x == 1)

--- Exceptions ---
Throw             act.Should().Throw<Exception>()
ThrowAsync        await act.Should().ThrowAsync<Exception>()
NotThrow          act.Should().NotThrow()
WithMessage       .WithMessage("*error*")

--- Objects ---
BeNull            obj.Should().BeNull()
BeOfType          obj.Should().BeOfType<string>()
BeEquivalentTo    obj.Should().BeEquivalentTo(expected)

--- DateTime ---
BeAfter/BeBefore  dt.Should().BeAfter(other)
HaveYear          dt.Should().HaveYear(2025)

--- Scope ---
AssertionScope    using (new AssertionScope()) { ... }

Target: .NET 10.0+
Frameworks: xUnit v3, NUnit v4, MSTest v4, MSpec

================================================================================
