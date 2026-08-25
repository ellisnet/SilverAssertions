================================================================================
AGENT-README: SilverAssertions
A Guide for AI Coding Agents — CONSUMING the SilverAssertions.ApacheLicenseForever
NuGet package
================================================================================

OVERVIEW
========
SilverAssertions is a fluent assertion library for .NET unit tests. It lets a
test state its expected outcome as a readable, chainable expression built on the
".Should()" extension-method pattern:

    actual.Should().Be(expected);
    actual.Should().StartWith("Silver").And.HaveLength(16);

It works with any test framework (xUnit, NUnit, MSTest, MSpec); it does not
discover or run tests itself. It throws the failure exception that the detected
test framework understands, so a failing assertion is reported as a normal test
failure.

Target framework: .NET 10 or later.

Provenance: SilverAssertions is a fork of FluentAssertions 7.1.0, licensed under
the Apache License 2.0. The API surface is close to that of the upstream
project, but every namespace root is "SilverAssertions" instead of
"FluentAssertions". Do NOT write "using FluentAssertions;" and do NOT reference
both libraries in the same project - the ".Should()" extension methods would be
ambiguous and the code would not compile.

================================================================================

INSTALLATION
============
PackageId: SilverAssertions.ApacheLicenseForever

    dotnet add package SilverAssertions.ApacheLicenseForever

Or, in a .csproj (let NuGet resolve the latest compatible version):

    <PackageReference Include="SilverAssertions.ApacheLicenseForever" />

NuGet dependencies (restored automatically):
  - JetBrains.Annotations
  - System.Configuration.ConfigurationManager

License: Apache-2.0

Requirements and limits:
  - The consuming project must target net10.0 or later. There is no
    netstandard2.0 or net8.0 asset in the package.
  - No native libraries, no OS restrictions - it is a pure managed library.
  - The package name is NOT "SilverAssertions"; the ".ApacheLicenseForever"
    suffix is part of the id and records the license that the package will
    always carry.

================================================================================

KEY NAMESPACES / USINGS
=======================
For the overwhelming majority of assertions, exactly one using is needed:

    using SilverAssertions;

That namespace holds AssertionExtensions (all the ".Should()" overloads),
FluentActions, EventRaisingExtensions (WithSender / WithArgs),
TypeEnumerableExtensions, XmlAssertionExtensions, the System.Data ".Should()"
extensions, the occurrence-constraint factories (Exactly, AtLeast, AtMost,
MoreThan, LessThan), AndConstraint<T>, AndWhichConstraint<T,TMatched>,
CustomAssertionAttribute, AssertionOptions and AssertionExtensions.As<T>().

Additional namespaces are needed only when a type name is written out
explicitly:

    using SilverAssertions.Execution;    // AssertionScope, Execute,
                                         // IAssertionScope, FailReason,
                                         // AssertionFailedException
    using SilverAssertions.Common;       // CSharpAccessModifier, Configuration,
                                         // Services, ValueFormatterDetectionMode
    using SilverAssertions.Types;        // AllTypes, TypeSelector,
                                         // MethodInfoSelector,
                                         // PropertyInfoSelector
    using SilverAssertions.Events;       // IMonitor<T>, IEventRecording,
                                         // OccurredEvent, EventMetadata
    using SilverAssertions.Formatting;   // Formatter, IValueFormatter,
                                         // ValueFormatterAttribute,
                                         // FormattedObjectGraph,
                                         // FormattingOptions, FormattingContext
    using SilverAssertions.Equivalency;  // EquivalencyAssertionOptions<T>,
                                         // IMemberInfo, IObjectInfo, INode,
                                         // IEquivalencyStep, MemberVisibility
    using SilverAssertions.Extensions;   // 5.Seconds(), 1.January(2026), ...
    using SilverAssertions.Specialized;  // ExecutionTime, ExceptionAssertions<T>
    using SilverAssertions.Primitives;   // StringAssertions, ObjectAssertions...
    using SilverAssertions.Collections;  // GenericCollectionAssertions<T>...
    using SilverAssertions.Numeric;      // NumericAssertions<T>...
    using SilverAssertions.Streams;      // StreamAssertions
    using SilverAssertions.Xml;          // XDocumentAssertions...
    using SilverAssertions.Data;         // DataSetAssertions<T>, RowMatchMode...
    using SilverAssertions.Reflection;   // AssemblyAssertions

IMPORTANT: "new AssertionScope()" needs "using SilverAssertions.Execution;", and
CSharpAccessModifier needs "using SilverAssertions.Common;". Neither is in the
root namespace.

================================================================================

SUPPORTED TEST FRAMEWORKS
=========================
The failure exception is thrown through an adapter that is detected at the first
failure, in this order: an explicit app-setting, then dynamic scanning of the
loaded assemblies, then a fallback.

Adapters and the assembly each one looks for:

    mspec       Machine.Specifications              -> SpecificationException
    nunit       nunit.framework                     -> AssertionException
    mstestv2    Microsoft.VisualStudio.TestPlatform.TestFramework
    mstestv3    MSTest.TestFramework                -> AssertFailedException
    mstestv4    MSTest.TestFramework                -> AssertFailedException
    xunit2      xunit.assert                        -> XunitException
    xunit3      xunit.v3.assert                     -> XunitException

If none is found, SilverAssertions throws its own
SilverAssertions.Execution.AssertionFailedException, which most runners still
report as a failed test.

To force a specific adapter (useful when more than one framework is loaded), add
an app setting named "SilverAssertions.TestFramework" whose value is one of the
keys above, or set it in code before the first assertion:

    using SilverAssertions.Common;

    Configuration.Current.TestFrameworkName = "xunit3";

Naming an unsupported or unavailable framework throws InvalidOperationException
listing the valid keys.

================================================================================

CORE API REFERENCE
==================

THE .Should() PATTERN AND THE CONSTRAINT TYPES
----------------------------------------------
"Should()" is an extension method (on AssertionExtensions, plus a few sibling
extension classes) that wraps the subject in an assertion class. Which class you
get is decided by the compile-time type of the subject:

    string          -> StringAssertions
    int/long/...    -> NumericAssertions<T>          (T : struct, IComparable<T>)
    int?/long?/...  -> NullableNumericAssertions<T>
    bool / bool?    -> BooleanAssertions / NullableBooleanAssertions
    Guid / Guid?    -> GuidAssertions / NullableGuidAssertions
    TEnum / TEnum?  -> EnumAssertions<TEnum> / NullableEnumAssertions<TEnum>
    DateTime        -> DateTimeAssertions   (nullable: NullableDateTimeAssertions)
    DateTimeOffset  -> DateTimeOffsetAssertions / NullableDateTimeOffsetAssertions
    DateOnly        -> DateOnlyAssertions   / NullableDateOnlyAssertions
    TimeOnly        -> TimeOnlyAssertions   / NullableTimeOnlyAssertions
    TimeSpan        -> SimpleTimeSpanAssertions / NullableSimpleTimeSpanAssertions
    IComparable<T>  -> ComparableTypeAssertions<T>
    IEnumerable<T>  -> GenericCollectionAssertions<T>
    IEnumerable<string>            -> StringCollectionAssertions
    IDictionary<TKey,TValue>       -> GenericDictionaryAssertions<...>
    object / any reference type    -> ObjectAssertions
    HttpResponseMessage            -> HttpResponseMessageAssertions
    Stream / BufferedStream        -> StreamAssertions / BufferedStreamAssertions
    XDocument / XElement / XAttribute -> XDocumentAssertions / XElementAssertions
                                         / XAttributeAssertions
    XmlNode / XmlElement           -> XmlNodeAssertions / XmlElementAssertions
    Type                           -> TypeAssertions
    MethodInfo / ConstructorInfo   -> MethodInfoAssertions
                                      / ConstructorInfoAssertions
    PropertyInfo                   -> PropertyInfoAssertions
    TypeSelector / MethodInfoSelector / PropertyInfoSelector
                                   -> TypeSelectorAssertions
                                      / MethodInfoSelectorAssertions
                                      / PropertyInfoSelectorAssertions
    Assembly                       -> AssemblyAssertions
    Action                         -> ActionAssertions
    Func<T>                        -> FunctionAssertions<T>
    Func<Task>                     -> NonGenericAsyncFunctionAssertions
    Func<Task<T>>                  -> GenericAsyncFunctionAssertions<T>
    TaskCompletionSource /
    TaskCompletionSource<T>        -> TaskCompletionSourceAssertions
                                      / TaskCompletionSourceAssertions<T>
    ExecutionTime                  -> ExecutionTimeAssertions
    DataSet / DataTable / DataRow / DataColumn
                                   -> DataSetAssertions<T> /
                                      DataTableAssertions<T> /
                                      DataRowAssertions<T> / DataColumnAssertions

Most assertion classes come in a generic self-typed pair -
e.g. StringAssertions : StringAssertions<StringAssertions>,
NumericAssertions<T> : NumericAssertions<T, NumericAssertions<T>> - so that
derived assertion classes keep returning their own type from a chain. The
non-generic name is what you use; the generic one is what you derive from.

Return types, and how to keep chaining:

    AndConstraint<T>                      .And  -> back to the assertion class
    AndWhichConstraint<TParent,TMatched>  .And  -> back to the assertion class
                                          .Which (== .Subject) -> the matched
                                                    element, for further asserts
    WhoseValueConstraint<...>             .WhoseValue -> the dictionary value
    ExceptionAssertions<TException>       .And / .Which -> the exception itself

Examples:

    numbers.Should().HaveCount(3).And.OnlyHaveUniqueItems();

    // ContainSingle returns AndWhichConstraint<..., T>
    orders.Should().ContainSingle(o => o.Id == 42)
        .Which.Total.Should().Be(19.95m);

    // BeOfType<T> returns AndWhichConstraint<..., T>
    result.Should().BeOfType<HttpResponseMessage>()
        .Which.StatusCode.Should().Be(HttpStatusCode.OK);

    // ContainKey returns WhoseValueConstraint<...>
    ages.Should().ContainKey("Alice").WhoseValue.Should().Be(30);

    // Throw<T> returns ExceptionAssertions<T>; .And is the exception
    act.Should().Throw<ArgumentException>().And.ParamName.Should().Be("id");

The "because" phrase
--------------------
Almost every assertion ends with the same two optional parameters:

    (..., string because = "", params object[] becauseArgs)

"because" is appended to the failure message; if it does not already start with
the word "because" that word is prepended. "becauseArgs" are string.Format
placeholders for it:

    value.Should().BeTrue("the user {0} is authenticated", userName);
    count.Should().BeGreaterThan(0, "because counts must be positive");

Occurrence constraints
----------------------
Five static factories in the SilverAssertions namespace produce an
OccurrenceConstraint:

    Exactly.Once()   Exactly.Twice()   Exactly.Thrice()   Exactly.Times(n)
    AtLeast.Once()   AtLeast.Twice()   AtLeast.Thrice()   AtLeast.Times(n)
    AtMost.Once()    AtMost.Twice()    AtMost.Thrice()    AtMost.Times(n)
    MoreThan.Once()  MoreThan.Twice()  MoreThan.Thrice()  MoreThan.Times(n)
    LessThan.Twice() LessThan.Thrice() LessThan.Times(n)

    (LessThan has no Once() - "less than once" is not expressible.)

OccurrenceConstraintExtensions adds the int-first spellings:

    3.TimesExactly()    3.TimesOrLess()    3.TimesOrMore()

They are accepted by these members - and only these:

    StringAssertions.Contain(string, OccurrenceConstraint, ...)
    StringAssertions.ContainEquivalentOf(string, OccurrenceConstraint, ...)
    StringAssertions.MatchRegex(string|Regex, OccurrenceConstraint, ...)
    XDocumentAssertions.HaveElement(string|XName, OccurrenceConstraint, ...)
    XElementAssertions.HaveElement(string|XName, OccurrenceConstraint, ...)

Examples:

    "a-a-a".Should().Contain("a", Exactly.Times(3));
    "banana".Should().ContainEquivalentOf("AN", AtLeast.Twice());
    log.Should().MatchRegex(@"\d{4}", 2.TimesOrMore());
    doc.Should().HaveElement("item", AtLeast.Twice());

Note: event monitoring has no occurrence-constraint overload; count the
recording instead (see EVENT MONITORING).

STRING ASSERTIONS (StringAssertions)
------------------------------------
Signatures (all end with "string because = "", params object[] becauseArgs";
that tail is written as "..." below):

    AndConstraint<TAssertions> Be(string expected, ...)
    AndConstraint<TAssertions> NotBe(string unexpected, ...)
    AndConstraint<TAssertions> BeEquivalentTo(string expected, ...)      // case-
    AndConstraint<TAssertions> NotBeEquivalentTo(string unexpected, ...) // insens.
    AndConstraint<TAssertions> BeOneOf(params string[] validValues)
    AndConstraint<TAssertions> BeOneOf(IEnumerable<string> validValues, ...)
    AndConstraint<TAssertions> StartWith(string expected, ...)
    AndConstraint<TAssertions> NotStartWith(string unexpected, ...)
    AndConstraint<TAssertions> StartWithEquivalentOf(string expected, ...)
    AndConstraint<TAssertions> NotStartWithEquivalentOf(string unexpected, ...)
    AndConstraint<TAssertions> EndWith(string expected, ...)
    AndConstraint<TAssertions> NotEndWith(string unexpected, ...)
    AndConstraint<TAssertions> EndWithEquivalentOf(string expected, ...)
    AndConstraint<TAssertions> NotEndWithEquivalentOf(string unexpected, ...)
    AndConstraint<TAssertions> Contain(string expected, ...)
    AndConstraint<TAssertions> Contain(string expected,
                                       OccurrenceConstraint occurrence, ...)
    AndConstraint<TAssertions> ContainEquivalentOf(string expected, ...)
    AndConstraint<TAssertions> ContainEquivalentOf(string expected,
                                       OccurrenceConstraint occurrence, ...)
    AndConstraint<TAssertions> ContainAll(params string[] values)
    AndConstraint<TAssertions> ContainAll(IEnumerable<string> values, ...)
    AndConstraint<TAssertions> ContainAny(params string[] values)
    AndConstraint<TAssertions> ContainAny(IEnumerable<string> values, ...)
    AndConstraint<TAssertions> NotContain(string unexpected, ...)
    AndConstraint<TAssertions> NotContainAll(params string[] values)
    AndConstraint<TAssertions> NotContainAny(params string[] values)
    AndConstraint<TAssertions> NotContainEquivalentOf(string unexpected, ...)
    AndConstraint<TAssertions> Match(string wildcardPattern, ...)
    AndConstraint<TAssertions> NotMatch(string wildcardPattern, ...)
    AndConstraint<TAssertions> MatchEquivalentOf(string wildcardPattern, ...)
    AndConstraint<TAssertions> NotMatchEquivalentOf(string wildcardPattern, ...)
    AndConstraint<TAssertions> MatchRegex(string regularExpression, ...)
    AndConstraint<TAssertions> MatchRegex(Regex regularExpression, ...)
    AndConstraint<TAssertions> NotMatchRegex(string regularExpression, ...)
    AndConstraint<TAssertions> NotMatchRegex(Regex regularExpression, ...)
    AndConstraint<TAssertions> HaveLength(int expected, ...)
    AndConstraint<TAssertions> BeEmpty(...)          NotBeEmpty(...)
    AndConstraint<TAssertions> BeNullOrEmpty(...)    NotBeNullOrEmpty(...)
    AndConstraint<TAssertions> BeNullOrWhiteSpace(...) NotBeNullOrWhiteSpace(...)
    AndConstraint<TAssertions> BeUpperCased(...)     NotBeUpperCased(...)
    AndConstraint<TAssertions> BeLowerCased(...)     NotBeLowerCased(...)

Usage:

    string name = "SilverAssertions";

    name.Should().Be("SilverAssertions");
    name.Should().BeEquivalentTo("silverassertions");
    name.Should().StartWith("Silver").And.EndWith("Assertions")
        .And.HaveLength(16);
    name.Should().Match("Silver*");
    name.Should().MatchRegex(@"^Silver\w+$");
    name.Should().BeOneOf("SilverAssertions", "GoldAssertions");
    "".Should().BeEmpty();
    "   ".Should().BeNullOrWhiteSpace();
    "HELLO".Should().BeUpperCased();

Wildcard patterns use "*" (zero or more characters) and "?" (exactly one).

NUMERIC ASSERTIONS (NumericAssertions<T>, NullableNumericAssertions<T>)
-----------------------------------------------------------------------
Applies to sbyte, byte, short, ushort, int, uint, long, ulong, float, double,
decimal and their nullable forms (the class is constrained to
"where T : struct, IComparable<T>").

    AndConstraint<TAssertions> Be(T expected, ...) / Be(T? expected, ...)
    AndConstraint<TAssertions> NotBe(T unexpected, ...)
    AndConstraint<TAssertions> BePositive(...)   BeNegative(...)
    AndConstraint<TAssertions> BeLessThan(T expected, ...)
    AndConstraint<TAssertions> BeLessThanOrEqualTo(T expected, ...)
    AndConstraint<TAssertions> BeGreaterThan(T expected, ...)
    AndConstraint<TAssertions> BeGreaterThanOrEqualTo(T expected, ...)
    AndConstraint<TAssertions> BeInRange(T minimumValue, T maximumValue, ...)
    AndConstraint<TAssertions> NotBeInRange(T minimumValue, T maximumValue, ...)
    AndConstraint<TAssertions> BeOneOf(params T[] validValues)
    AndConstraint<TAssertions> BeOneOf(IEnumerable<T> validValues, ...)
    AndConstraint<TAssertions> BeOfType(Type expectedType, ...)
    AndConstraint<TAssertions> NotBeOfType(Type unexpectedType, ...)
    AndConstraint<TAssertions> Match(Expression<Func<T, bool>> predicate, ...)

    // NullableNumericAssertions<T> adds:
    AndConstraint<TAssertions> HaveValue(...)   NotHaveValue(...)
    AndConstraint<TAssertions> BeNull(...)      NotBeNull(...)

BeLessOrEqualTo / BeGreaterOrEqualTo exist as aliases of the "...ThanOrEqualTo"
members.

Tolerance-based comparison lives on NumericAssertionsExtensions, NOT on the
assertion class, and the two families are not interchangeable:

    // Integral types only - note the UNSIGNED delta parameter:
    AndConstraint<NumericAssertions<int>> BeCloseTo(
        this NumericAssertions<int> parent, int nearbyValue, uint delta, ...)
    // ... and the same for sbyte/byte/short/ushort/uint/long/ulong,
    //     plus NotBeCloseTo for each.

    // Floating-point and decimal only:
    AndConstraint<NumericAssertions<double>> BeApproximately(
        this NumericAssertions<double> parent, double expectedValue,
        double precision, ...)
    // ... and the same for float and decimal, for the Nullable variants,
    //     plus NotBeApproximately for each.

Usage:

    int value = 42;
    value.Should().Be(42);
    value.Should().BeGreaterThan(0).And.BeLessThan(100);
    value.Should().BeInRange(1, 100);
    value.Should().BePositive();
    value.Should().BeOneOf(40, 41, 42, 43);
    value.Should().BeCloseTo(45, 5u);          // delta is uint

    double result = 3.14159;
    result.Should().BeApproximately(3.14, 0.01);
    result.Should().NotBeApproximately(5.0, 0.01);

    int? maybe = null;
    maybe.Should().NotHaveValue();
    maybe.Should().BeNull();

ComparableTypeAssertions<T> (reached with "IComparable<T> x; x.Should()") adds
BeRankedEquallyTo / NotBeRankedEquallyTo alongside the same comparison members.

BOOLEAN ASSERTIONS (BooleanAssertions, NullableBooleanAssertions)
-----------------------------------------------------------------
    AndConstraint<TAssertions> BeTrue(...)      BeFalse(...)
    AndConstraint<TAssertions> Be(bool expected, ...)
    AndConstraint<TAssertions> NotBe(bool unexpected, ...)
    AndConstraint<TAssertions> Imply(bool consequent, ...)   // subject => cons.

    // NullableBooleanAssertions adds:
    HaveValue(...)  NotHaveValue(...)  BeNull(...)  NotBeNull(...)
    NotBeTrue(...)  NotBeFalse(...)    Be(bool? expected, ...)

Usage:

    isActive.Should().BeTrue();
    isDeleted.Should().BeFalse();
    isAdmin.Should().Imply(canDelete, "admins may always delete");

    bool? flag = null;
    flag.Should().NotHaveValue();

GUID ASSERTIONS (GuidAssertions, NullableGuidAssertions)
--------------------------------------------------------
    BeEmpty(...)  NotBeEmpty(...)
    Be(Guid expected, ...)      Be(string expected, ...)
    NotBe(Guid unexpected, ...) NotBe(string unexpected, ...)
    // nullable adds HaveValue / NotHaveValue / BeNull / NotBeNull / Be(Guid?)

    Guid.Empty.Should().BeEmpty();
    id.Should().NotBeEmpty();
    id.Should().Be("11111111-2222-3333-4444-555555555555");

ENUM ASSERTIONS (EnumAssertions<TEnum>, NullableEnumAssertions<TEnum>)
----------------------------------------------------------------------
Reached through EnumAssertionsExtensions.Should<TEnum>(this TEnum) where
TEnum : struct, Enum.

    Be(TEnum expected, ...)             NotBe(TEnum unexpected, ...)
    BeDefined(...)                      NotBeDefined(...)
    HaveValue(decimal expected, ...)    NotHaveValue(decimal unexpected, ...)
    HaveSameValueAs<T>(T expected, ...) NotHaveSameValueAs<T>(T unexpected, ...)
    HaveSameNameAs<T>(T expected, ...)  NotHaveSameNameAs<T>(T unexpected, ...)
    HaveFlag(TEnum expectedFlag, ...)   NotHaveFlag(TEnum unexpectedFlag, ...)
    Match(Expression<Func<TEnum?, bool>> predicate, ...)
    BeOneOf(params TEnum[] validValues) / BeOneOf(IEnumerable<TEnum>, ...)

Usage:

    [Flags]
    enum Access { None = 0, Read = 1, Write = 2, All = Read | Write }

    var access = Access.All;
    access.Should().HaveFlag(Access.Read).And.HaveFlag(Access.Write);
    access.Should().BeDefined();
    access.Should().HaveValue(3);
    DayOfWeek.Monday.Should().Be(DayOfWeek.Monday);
    ((DayOfWeek)99).Should().NotBeDefined();

DATE AND TIME ASSERTIONS
------------------------
DateTimeAssertions (and NullableDateTimeAssertions):

    Be(DateTime expected, ...)             NotBe(DateTime unexpected, ...)
    BeCloseTo(DateTime nearbyTime, TimeSpan precision, ...)
    NotBeCloseTo(DateTime distantTime, TimeSpan precision, ...)
    BeBefore / NotBeBefore / BeOnOrBefore / NotBeOnOrBefore(DateTime, ...)
    BeAfter  / NotBeAfter  / BeOnOrAfter  / NotBeOnOrAfter(DateTime, ...)
    HaveYear / HaveMonth / HaveDay / HaveHour / HaveMinute / HaveSecond(int, ...)
       (each with a NotHave... counterpart)
    BeSameDateAs(DateTime expected, ...)   NotBeSameDateAs(DateTime, ...)
    BeIn(DateTimeKind expectedKind, ...)
    BeOneOf(params DateTime[] validValues) / BeOneOf(IEnumerable<DateTime>, ...)

    // "distance from another moment" - these return DateTimeRangeAssertions<T>,
    // which is finished with .Before(target) or .After(target):
    DateTimeRangeAssertions<TAssertions> BeMoreThan(TimeSpan timeSpan)
    DateTimeRangeAssertions<TAssertions> BeAtLeast(TimeSpan timeSpan)
    DateTimeRangeAssertions<TAssertions> BeExactly(TimeSpan timeSpan)
    DateTimeRangeAssertions<TAssertions> BeWithin(TimeSpan timeSpan)
    DateTimeRangeAssertions<TAssertions> BeLessThan(TimeSpan timeSpan)

Usage:

    var dt = new DateTime(2026, 7, 4, 12, 0, 0, DateTimeKind.Utc);

    dt.Should().BeAfter(new DateTime(2026, 1, 1));
    dt.Should().HaveYear(2026).And.HaveMonth(7).And.HaveDay(4);
    dt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromDays(3650));
    dt.Should().BeIn(DateTimeKind.Utc);
    dt.Should().BeMoreThan(TimeSpan.FromHours(6))
        .After(new DateTime(2026, 7, 4, 0, 0, 0));

DateTimeOffsetAssertions adds BeExactly(DateTimeOffset, ...) and
HaveOffset(TimeSpan expected, ...) to the same shape;
DateTimeOffsetRangeAssertions<T> is its range type.

DateOnlyAssertions: Be / NotBe / BeBefore / BeOnOrBefore / BeAfter /
BeOnOrAfter / HaveYear / HaveMonth / HaveDay / BeOneOf (no time components).

TimeOnlyAssertions: Be / NotBe / BeCloseTo(TimeOnly, TimeSpan) / BeBefore /
BeOnOrBefore / BeAfter / BeOnOrAfter / HaveHours / HaveMinutes / HaveSeconds /
HaveMilliseconds / BeOneOf.

SimpleTimeSpanAssertions (and NullableSimpleTimeSpanAssertions):

    BePositive(...)   BeNegative(...)
    Be(TimeSpan expected, ...)          NotBe(TimeSpan unexpected, ...)
    BeLessThan / BeLessThanOrEqualTo / BeGreaterThan /
    BeGreaterThanOrEqualTo(TimeSpan expected, ...)
    BeCloseTo(TimeSpan nearbyTime, TimeSpan precision, ...)
    NotBeCloseTo(TimeSpan distantTime, TimeSpan precision, ...)

    TimeSpan.FromMinutes(5).Should().BePositive()
        .And.BeLessThan(TimeSpan.FromMinutes(10));

FLUENT DATE AND TIME CONSTRUCTION (SilverAssertions.Extensions)
----------------------------------------------------------------
FluentTimeSpanExtensions and FluentDateTimeExtensions build DateTime/TimeSpan
values readably. They are in the SilverAssertions.Extensions namespace, so they
need their own using.

    using SilverAssertions.Extensions;

    5.Seconds()        // TimeSpan; also Ticks/Nanoseconds/Microseconds/
    250.Milliseconds() // Milliseconds/Seconds/Minutes/Hours/Days,
    2.Hours()          // each for int, several also for long/double
    1.Hours().And(30.Minutes())

    4.July(2026)                   // DateTime (one method per month name)
    4.July(2026).At(12, 30)        // At(hours, minutes, seconds = 0, ...)
    4.July(2026).At(12, 30).AsUtc()
    5.Minutes().Before(deadline)   // DateTime
    5.Minutes().After(start)       // DateTime
    someDateTime.WithOffset(TimeSpan.Zero)   // DateTimeOffset
    someDateTime.Microsecond()  someDateTime.AddNanoseconds(500)

COLLECTION ASSERTIONS (GenericCollectionAssertions<T>)
-------------------------------------------------------
Works for arrays, List<T>, IEnumerable<T>, ICollection<T> and anything else that
is an IEnumerable<T>. Key signatures:

    Count:
      HaveCount(int expected, ...)
      HaveCount(Expression<Func<int, bool>> countPredicate, ...)
      NotHaveCount(int unexpected, ...)
      HaveCountGreaterThan / HaveCountGreaterThanOrEqualTo(int expected, ...)
      HaveCountLessThan / HaveCountLessThanOrEqualTo(int expected, ...)
      HaveSameCount<TExpectation>(IEnumerable<TExpectation> other, ...)
      NotHaveSameCount<TExpectation>(IEnumerable<TExpectation> other, ...)

    Emptiness / null:
      BeEmpty(...)  NotBeEmpty(...)  BeNullOrEmpty(...)  NotBeNullOrEmpty(...)
      NotContainNulls(...)  NotContainNulls<TKey>(Expression<Func<T,TKey>>, ...)

    Membership:
      AndWhichConstraint<TAssertions, T> Contain(T expected, ...)
      AndWhichConstraint<TAssertions, T> Contain(Expression<Func<T,bool>>, ...)
      AndConstraint<TAssertions>         Contain(IEnumerable<T> expected, ...)
      AndWhichConstraint<TAssertions, T> ContainSingle(...)
      AndWhichConstraint<TAssertions, T> ContainSingle(Expression<Func<T,bool>>)
      NotContain(T unexpected, ...) / NotContain(Expression<Func<T,bool>>, ...)
      ContainItemsAssignableTo<TExpectation>(...)
      NotContainItemsAssignableTo<TExpectation>(...)
      AllBeAssignableTo<TExpectation>(...)  AllBeOfType<TExpectation>(...)
      BeSubsetOf(IEnumerable<T> expectedSuperset, ...)   NotBeSubsetOf(...)
      IntersectWith(IEnumerable<T> other, ...)  NotIntersectWith(...)

    Order and position:
      Equal(params T[] elements) / Equal(IEnumerable<T> expected, ...)
      Equal<TExpectation>(IEnumerable<TExpectation> expectation,
                          Func<T, TExpectation, bool> equalityComparison, ...)
      NotEqual(IEnumerable<T> unexpected, ...)
      ContainInOrder(params T[] expected)             // subsequence
      ContainInConsecutiveOrder(params T[] expected)  // adjacent run
      NotContainInOrder(...)  NotContainInConsecutiveOrder(...)
      StartWith(T element, ...) / StartWith(IEnumerable<T> expectation, ...)
      EndWith(T element, ...)   / EndWith(IEnumerable<T> expectation, ...)
      HaveElementAt(int index, T element, ...)   // AndWhichConstraint
      HaveElementPreceding(T successor, T expectation, ...)
      HaveElementSucceeding(T predecessor, T expectation, ...)
      BeInAscendingOrder(...) / BeInAscendingOrder(IComparer<T> comparer, ...)
      BeInAscendingOrder<TSelector>(Expression<Func<T, TSelector>> property, ...)
      BeInAscendingOrder(Func<T, T, int> comparison, ...)
      BeInDescendingOrder(... same four overloads ...)
      NotBeInAscendingOrder / NotBeInDescendingOrder (same four overloads)

    Content rules:
      OnlyContain(Expression<Func<T, bool>> predicate, ...)
      OnlyHaveUniqueItems(...) / OnlyHaveUniqueItems<TKey>(Expression<...>, ...)
      AllSatisfy(Action<T> expected, ...)
      SatisfyRespectively(params Action<T>[] elementInspectors)
      Satisfy(params Expression<Func<T, bool>>[] predicates)

    Equivalency:
      BeEquivalentTo<TExpectation>(IEnumerable<TExpectation> expectation, ...)
      BeEquivalentTo<TExpectation>(IEnumerable<TExpectation> expectation,
          Func<EquivalencyAssertionOptions<TExpectation>,
               EquivalencyAssertionOptions<TExpectation>> config, ...)
      NotBeEquivalentTo(...)  AllBeEquivalentTo<TExpectation>(TExpectation, ...)
      ContainEquivalentOf<TExpectation>(TExpectation expectation, ...)
      NotContainEquivalentOf<TExpectation>(TExpectation unexpected, ...)

Ordering assertions return AndConstraint<SubsequentOrderingAssertions<T>>, whose
".And" exposes ThenBeInAscendingOrder / ThenBeInDescendingOrder (declared on
SubsequentOrderingGenericCollectionAssertions<TCollection, T, TAssertions>) for
secondary sort keys.

Usage:

    var numbers = new[] { 1, 2, 3, 4, 5 };

    numbers.Should().HaveCount(5);
    numbers.Should().Contain(3).And.NotContain(99);
    numbers.Should().Contain(new[] { 2, 4 });
    numbers.Should().ContainInConsecutiveOrder(2, 3, 4);
    numbers.Should().BeInAscendingOrder();
    numbers.Should().Equal(1, 2, 3, 4, 5);
    numbers.Should().BeEquivalentTo(new[] { 5, 3, 1, 4, 2 });  // order-free
    numbers.Should().OnlyContain(n => n > 0);
    numbers.Should().OnlyHaveUniqueItems();
    numbers.Should().AllSatisfy(n => n.Should().BePositive());
    numbers.Should().SatisfyRespectively(
        n => n.Should().Be(1),
        n => n.Should().Be(2),
        n => n.Should().Be(3),
        n => n.Should().Be(4),
        n => n.Should().Be(5));

    people.Should().BeInAscendingOrder(p => p.LastName)
        .And.ThenBeInAscendingOrder(p => p.FirstName);

STRING COLLECTION ASSERTIONS (StringCollectionAssertions)
----------------------------------------------------------
An IEnumerable<string> gets StringCollectionAssertions, which is
GenericCollectionAssertions<TCollection, string, TAssertions> plus:

    Equal(params string[] expected)  /  Equal(IEnumerable<string> expected)
    BeEquivalentTo(params string[] expectation)
    AllBe(string expectation, ...)
    AndWhichConstraint<TAssertions, string> ContainMatch(string wildcard, ...)
    NotContainMatch(string wildcardPattern, ...)

    names.Should().ContainMatch("A*").Which.Should().Be("Alice");
    names.Should().NotContainMatch("*test*");

DICTIONARY ASSERTIONS (GenericDictionaryAssertions<TCollection,TKey,TValue>)
-----------------------------------------------------------------------------
Derives from GenericCollectionAssertions over KeyValuePair<TKey,TValue>, so all
collection members are available too.

    WhoseValueConstraint<...> ContainKey(TKey expected, ...)
    AndConstraint<TAssertions> ContainKeys(params TKey[] expected)
    AndConstraint<TAssertions> NotContainKey(TKey unexpected, ...)
    AndConstraint<TAssertions> NotContainKeys(params TKey[] unexpected)
    AndWhichConstraint<TAssertions, TValue> ContainValue(TValue expected, ...)
    AndConstraint<TAssertions> ContainValues(params TValue[] expected)
    AndConstraint<TAssertions> NotContainValue(TValue unexpected, ...)
    AndConstraint<TAssertions> NotContainValues(params TValue[] unexpected)
    AndConstraint<TAssertions> Contain(TKey key, TValue value, ...)
    AndConstraint<TAssertions> Contain(params KeyValuePair<TKey,TValue>[] pairs)
    AndConstraint<TAssertions> NotContain(TKey key, TValue value, ...)
    AndConstraint<TAssertions> Equal<T>(T expected, ...)  NotEqual<T>(T, ...)
    AndConstraint<TAssertions> BeEquivalentTo<TExpectation>(TExpectation, ...)

Usage:

    var ages = new Dictionary<string, int>
    {
        ["Alice"] = 30, ["Bob"] = 25
    };

    ages.Should().ContainKey("Alice").WhoseValue.Should().Be(30);
    ages.Should().ContainKeys("Alice", "Bob");
    ages.Should().ContainValue(25);
    ages.Should().Contain("Bob", 25);
    ages.Should().NotContainKey("Dave");
    ages.Should().HaveCount(2);

OBJECT AND REFERENCE-TYPE ASSERTIONS
------------------------------------
ReferenceTypeAssertions<TSubject,TAssertions> is the base of nearly every
reference-type assertion class, so these members are available almost
everywhere:

    AndConstraint<TAssertions> BeNull(...)      NotBeNull(...)
    AndConstraint<TAssertions> BeSameAs(TSubject expected, ...)
    AndConstraint<TAssertions> NotBeSameAs(TSubject unexpected, ...)
    AndWhichConstraint<TAssertions, T> BeOfType<T>(...)
    AndConstraint<TAssertions> BeOfType(Type expectedType, ...)
    AndConstraint<TAssertions> NotBeOfType<T>(...)
    AndWhichConstraint<TAssertions, T> BeAssignableTo<T>(...)
    AndConstraint<TAssertions> NotBeAssignableTo<T>(...)
    AndConstraint<TAssertions> Match(Expression<Func<TSubject,bool>> pred., ...)
    TSubject Subject { get; }

ObjectAssertions<TSubject,TAssertions> adds:

    Be(TSubject expected, ...)
    Be(TSubject expected, IEqualityComparer<TSubject> comparer, ...)
    NotBe(TSubject unexpected, ...) / NotBe(..., IEqualityComparer<TSubject>, ...)
    BeOneOf(params TSubject[] validValues)
    BeOneOf(IEnumerable<TSubject> validValues,
            IEqualityComparer<TSubject> comparer, ...)
    BeEquivalentTo<TExpectation>(TExpectation expectation, ...)
    BeEquivalentTo<TExpectation>(TExpectation expectation,
        Func<EquivalencyAssertionOptions<TExpectation>,
             EquivalencyAssertionOptions<TExpectation>> config, ...)
    NotBeEquivalentTo<TExpectation>(TExpectation unexpected, ...)

ObjectAssertionsExtensions adds serialization round-trip checks:

    BeXmlSerializable(...)          BeDataContractSerializable(...)
    BeDataContractSerializable<T>(Func<EquivalencyAssertionOptions<T>,
                                       EquivalencyAssertionOptions<T>>, ...)
    BeBinarySerializable(...)   <- ALWAYS FAILS: see COMMON PITFALLS

Usage:

    object obj = "hello";
    obj.Should().NotBeNull();
    obj.Should().BeOfType<string>().Which.Should().Be("hello");
    obj.Should().BeAssignableTo<IComparable>();
    obj.Should().BeSameAs(obj);
    customer.Should().Match<Customer>(c => c.Orders.Count > 0);

    // AssertionExtensions.As<TTo>() casts inside a chain:
    result.As<Customer>().Name.Should().Be("Alice");

OBJECT GRAPH EQUIVALENCY (BeEquivalentTo + EquivalencyAssertionOptions<T>)
---------------------------------------------------------------------------
BeEquivalentTo performs a recursive, member-by-member structural comparison. The
EXPECTATION drives the comparison: every member of the expectation must have a
matching member on the subject with an equivalent value; members that exist only
on the subject are ignored. That is why an anonymous type makes a good partial
expectation.

    actual.Should().BeEquivalentTo(new { Name = "Alice", Age = 30 });

Collections are compared without regard to order by default; nested objects are
compared recursively; records and tuples are handled; cyclic references are
detected.

The optional second parameter configures EquivalencyAssertionOptions<TExpectation>
(namespace SilverAssertions.Equivalency). The lambda receives the options object
and must return it. Members declared on EquivalencyAssertionOptions<TExpectation>:

    Excluding(Expression<Func<TExpectation, object>> expression)
    Including(Expression<Func<TExpectation, object>> expression)
    For<TNext>(Expression<Func<TExpectation, IEnumerable<TNext>>> expression)
        -> NestedExclusionOptionBuilder<TExpectation,TNext>.Exclude(...)
    WithStrictOrderingFor(Expression<Func<TExpectation, object>> expression)
    WithoutStrictOrderingFor(Expression<Func<TExpectation, object>> expression)
    AsCollection()
    WithMapping<TSubject>(Expression<Func<TExpectation, object>> expectationPath,
                          Expression<Func<TSubject, object>> subjectPath)
    WithMapping(string expectationMemberPath, string subjectMemberPath)
    WithMapping<TNestedExpectation, TNestedSubject>(string, string)

Inherited from SelfReferenceEquivalencyAssertionOptions<TSelf> (all return the
options object, so they chain):

    Member selection
      IncludingAllDeclaredProperties()   IncludingAllRuntimeProperties()
      IncludingProperties()              ExcludingProperties()
      IncludingFields()                  ExcludingFields()
      IncludingInternalProperties()      IncludingInternalFields()
      ExcludingNonBrowsableMembers()     IgnoringNonBrowsableMembersOnSubject()
      Excluding(Expression<Func<IMemberInfo, bool>> predicate)
      Including(Expression<Func<IMemberInfo, bool>> predicate)
      ExcludingMissingMembers()          ThrowingOnMissingMembers()
      IncludingNestedObjects()           ExcludingNestedObjects()
    Typing
      RespectingRuntimeTypes()           RespectingDeclaredTypes()
      ComparingByMembers<T>() / ComparingByMembers(Type type)
      ComparingByValue<T>()   / ComparingByValue(Type type)
      ComparingEnumsByName()             ComparingEnumsByValue()
      ComparingRecordsByValue()          ComparingRecordsByMembers()
      WithAutoConversion()
      WithAutoConversionFor(Expression<Func<IObjectInfo, bool>> predicate)
      WithoutAutoConversionFor(Expression<Func<IObjectInfo, bool>> predicate)
    Ordering
      WithStrictOrdering()               WithoutStrictOrdering()
      WithStrictOrderingFor(Expression<Func<IObjectInfo, bool>> predicate)
      WithoutStrictOrderingFor(Expression<Func<IObjectInfo, bool>> predicate)
    Recursion
      IgnoringCyclicReferences()         AllowingInfiniteRecursion()
    Custom comparison
      Restriction<TProperty> Using<TProperty>(
          Action<IAssertionContext<TProperty>> action)
          -> .WhenTypeIs<TMemberType>()  or  .When(Expression<Func<IObjectInfo,
                                                                    bool>>)
      Using<T>(IEqualityComparer<T> comparer)
      Using<T, TEqualityComparer>()
      Using(IMemberSelectionRule) / Using(IMemberMatchingRule)
      Using(IOrderingRule)        / Using(IEquivalencyStep)
      WithoutSelectionRules()     WithoutMatchingRules()
    Diagnostics
      WithTracing(ITraceWriter writer = null)   // e.g. StringBuilderTraceWriter

IAssertionContext<TSubject> exposes SelectedNode (INode), Subject, Expectation,
Because and BecauseArgs. IMemberInfo exposes Name, Type, DeclaringType, Path,
GetterAccessibility and SetterAccessibility; IObjectInfo exposes Path,
ParentType, CompileTimeType and RuntimeType. SubjectInfoExtensions adds four
predicates over IMemberInfo for use inside Excluding/Including rules:

    WhichGetterHas(CSharpAccessModifier)  WhichGetterDoesNotHave(...)
    WhichSetterHas(CSharpAccessModifier)  WhichSetterDoesNotHave(...)

    actual.Should().BeEquivalentTo(expected, o => o
        .Excluding(m => m.WhichSetterHas(CSharpAccessModifier.Private)));

Usage:

    // exclude members of the EXPECTATION type
    actual.Should().BeEquivalentTo(expected, options => options
        .Excluding(o => o.Id)
        .Excluding(o => o.CreatedAt));

    // only compare one member
    actual.Should().BeEquivalentTo(expected, o => o.Including(x => x.Name));

    // exclude by rule, over the whole graph
    actual.Should().BeEquivalentTo(expected, o => o
        .Excluding(member => member.Name == "UpdatedAt"));

    // order matters for every collection in the graph
    actual.Should().BeEquivalentTo(expected, o => o.WithStrictOrdering());

    // tolerance for every double in the graph
    actual.Should().BeEquivalentTo(expected, o => o
        .Using<double>(ctx => ctx.Subject.Should()
            .BeApproximately(ctx.Expectation, 0.01))
        .WhenTypeIs<double>());

    // structs/classes that override Equals: force member-wise comparison
    actual.Should().BeEquivalentTo(expected, o => o.ComparingByMembers<Money>());

    // graphs that point back at themselves
    actual.Should().BeEquivalentTo(expected, o => o.IgnoringCyclicReferences());

    // subject member has a different name than the expectation member
    actual.Should().BeEquivalentTo(expected, o => o
        .WithMapping<Order>(e => e.CustomerName, s => s.BuyerName));

Global defaults for every BeEquivalentTo in the test run are set once, through
AssertionOptions (namespace SilverAssertions):

    AssertionOptions.AssertEquivalencyUsing(options => options
        .ComparingByValue<Money>()
        .ExcludingMissingMembers());

    AssertionOptions.CloneDefaults<T>()   // an EquivalencyAssertionOptions<T>
                                          // pre-loaded with the global defaults
    AssertionOptions.EquivalencyPlan      // the ordered IEquivalencyStep list
    AssertionOptions.FormattingOptions    // UseLineBreaks / MaxDepth / MaxLines

Reset it in test-class teardown if you change it:

    AssertionOptions.AssertEquivalencyUsing(_ => new EquivalencyAssertionOptions());

EXCEPTION AND DELEGATE ASSERTIONS
---------------------------------
Delegate subjects get DelegateAssertions<TDelegate,TAssertions> (base
DelegateAssertionsBase), specialised as ActionAssertions (Action) and
FunctionAssertions<T> (Func<T>):

    ExceptionAssertions<TException> Throw<TException>(...)
    ExceptionAssertions<TException> ThrowExactly<TException>(...)
    AndConstraint<TAssertions>      NotThrow(...)
    AndConstraint<TAssertions>      NotThrow<TException>(...)
    AndConstraint<TAssertions>      NotThrowAfter(TimeSpan waitTime,
                                                  TimeSpan pollInterval, ...)
    // FunctionAssertions<T> re-declares NotThrow / NotThrowAfter returning
    // AndWhichConstraint<FunctionAssertions<T>, T> so .Which is the result.

ExceptionAssertions<TException> (namespace SilverAssertions.Specialized) then
refines the caught exception:

    TException And { get; }        // the exception itself
    TException Which { get; }      // same as And
    ExceptionAssertions<TException> WithMessage(string expectedWildcardPattern,
                                                ...)
    ExceptionAssertions<TInner> WithInnerException<TInner>(...)
    ExceptionAssertions<Exception> WithInnerException(Type innerException, ...)
    ExceptionAssertions<TInner> WithInnerExceptionExactly<TInner>(...)
    ExceptionAssertions<Exception> WithInnerExceptionExactly(Type inner, ...)
    ExceptionAssertions<TException> Where(
        Expression<Func<TException, bool>> exceptionExpression, ...)

ExceptionAssertionsExtensions adds
WithParameterName(string paramName, ...) for TException : ArgumentException, and
awaitable overloads of WithMessage / Where / WithInnerException /
WithInnerExceptionExactly / WithParameterName that take a
Task<ExceptionAssertions<TException>> - which is what makes
"await act.Should().ThrowAsync<T>().WithMessage(...)" compile.

Getting a delegate out of an expression - FluentActions (static, in
SilverAssertions) and the equivalent instance-style extensions on
AssertionExtensions:

    FluentActions.Invoking(Action action)            -> Action
    FluentActions.Invoking<T>(Func<T> func)          -> Func<T>
    FluentActions.Awaiting(Func<Task> action)        -> Func<Task>
    FluentActions.Awaiting<T>(Func<Task<T>> func)    -> Func<Task<T>>
    FluentActions.Enumerating(Func<IEnumerable> e)   -> Action
    subject.Invoking(x => x.DoSomething())           -> Action
    subject.Invoking(x => x.Compute())               -> Func<TResult>
    subject.Awaiting(x => x.DoSomethingAsync())      -> Func<Task>
    subject.Awaiting(x => x.ComputeAsync())          -> Func<Task<TResult>>
    subject.Enumerating(x => x.GetItems())           -> Action

AggregateException handling: Throw<TException> and NotThrow unwrap an
AggregateException and match against the inner exceptions (the extractor is
AggregateExceptionExtractor, an IExtractExceptions). ThrowExactly<TException>
does NOT unwrap - it requires the thrown type to be exactly TException.

Usage:

    Action act = () => throw new InvalidOperationException("went wrong");

    act.Should().Throw<InvalidOperationException>()
        .WithMessage("*went wrong*");
    act.Should().ThrowExactly<InvalidOperationException>();

    Action bad = () => service.GetUser(-1);
    bad.Should().Throw<ArgumentException>()
        .WithParameterName("id")
        .And.Message.Should().Contain("invalid");

    (() => { }).Should().NotThrow();

    service.Invoking(s => s.Delete(0)).Should()
        .Throw<ArgumentOutOfRangeException>();

    repository.Enumerating(r => r.StreamAll()).Should()
        .Throw<InvalidOperationException>();   // deferred iterator throws

    Action wrapped = () => throw new AggregateException(
        new InvalidOperationException("inner"));
    wrapped.Should().Throw<InvalidOperationException>()
        .WithMessage("inner");                 // unwrapped

TASK AND ASYNC ASSERTIONS
-------------------------
"Func<Task>" gets NonGenericAsyncFunctionAssertions and "Func<Task<T>>" gets
GenericAsyncFunctionAssertions<T>; both derive from
AsyncFunctionAssertions<TTask,TAssertions>, which declares:

    Task<AndConstraint<TAssertions>> CompleteWithinAsync(TimeSpan timeSpan, ...)
    Task<AndConstraint<TAssertions>> NotCompleteWithinAsync(TimeSpan, ...)
    Task<ExceptionAssertions<TException>> ThrowAsync<TException>(...)
    Task<ExceptionAssertions<TException>> ThrowExactlyAsync<TException>(...)
    Task<ExceptionAssertions<TException>> ThrowWithinAsync<TException>(
        TimeSpan timeSpan, ...)
    Task<AndConstraint<TAssertions>> NotThrowAsync(...)
    Task<AndConstraint<TAssertions>> NotThrowAsync<TException>(...)
    Task<AndConstraint<TAssertions>> NotThrowAfterAsync(TimeSpan waitTime,
                                                        TimeSpan pollInterval,
                                                        ...)

GenericAsyncFunctionAssertions<TResult> re-declares CompleteWithinAsync,
NotThrowAsync and NotThrowAfterAsync as
Task<AndWhichConstraint<GenericAsyncFunctionAssertions<TResult>, TResult>>, so
".Which" is the awaited result. AsyncAssertionsExtensions.WithResult then lets
you assert the value in the same chain:

    Task<AndWhichConstraint<GenericAsyncFunctionAssertions<T>, T>> WithResult<T>(
        this Task<AndWhichConstraint<GenericAsyncFunctionAssertions<T>, T>> task,
        T expected, ...)

TaskCompletionSourceAssertions and TaskCompletionSourceAssertions<T> (base
TaskCompletionSourceAssertionsBase) cover a TaskCompletionSource:

    Task<AndConstraint<TaskCompletionSourceAssertions>> CompleteWithinAsync(
        TimeSpan timeSpan, ...)
    Task<AndConstraint<...>> NotCompleteWithinAsync(TimeSpan timeSpan, ...)
    // the generic form returns AndWhichConstraint<..., T> from
    // CompleteWithinAsync, and AsyncAssertionsExtensions.WithResult applies.

Usage:

    Func<Task> act = async () =>
    {
        await Task.Delay(1);
        throw new InvalidOperationException("async failure");
    };

    await act.Should().ThrowAsync<InvalidOperationException>()
        .WithMessage("async failure");

    Func<Task> ok = () => Task.CompletedTask;
    await ok.Should().NotThrowAsync();
    await ok.Should().CompleteWithinAsync(TimeSpan.FromSeconds(1));

    Func<Task<int>> compute = async () => { await Task.Yield(); return 42; };
    (await compute.Should().CompleteWithinAsync(TimeSpan.FromSeconds(1)))
        .Which.Should().Be(42);
    await compute.Should().CompleteWithinAsync(TimeSpan.FromSeconds(1))
        .WithResult(42);

    await service.Awaiting(s => s.LoadAsync(-1)).Should()
        .ThrowAsync<ArgumentException>();

    var tcs = new TaskCompletionSource<int>();
    tcs.SetResult(7);
    await tcs.Should().CompleteWithinAsync(TimeSpan.FromSeconds(1))
        .WithResult(7);

EXECUTION TIME ASSERTIONS
-------------------------
    ExecutionTime ExecutionTime(this Action action, StartTimer createTimer = null)
    ExecutionTime ExecutionTime(this Func<Task> action)
    MemberExecutionTime<T> ExecutionTimeOf<T>(this T subject,
        Expression<Action<T>> action, StartTimer createTimer = null)
    ExecutionTimeAssertions Should(this ExecutionTime executionTime)

ExecutionTimeAssertions:

    BeLessThan(TimeSpan maxDuration, ...)
    BeLessThanOrEqualTo(TimeSpan maxDuration, ...)
    BeGreaterThan(TimeSpan minDuration, ...)
    BeGreaterThanOrEqualTo(TimeSpan minDuration, ...)
    BeCloseTo(TimeSpan expectedDuration, TimeSpan precision, ...)

Usage:

    Action act = () => Thread.Sleep(100);

    act.ExecutionTime().Should().BeLessThan(TimeSpan.FromSeconds(1));
    act.ExecutionTime().Should()
        .BeGreaterThan(TimeSpan.FromMilliseconds(50));

    service.ExecutionTimeOf(s => s.Rebuild()).Should()
        .BeLessThan(TimeSpan.FromSeconds(2));

The delegate is executed when ExecutionTime()/ExecutionTimeOf() is called, not
when Should() is called.

TYPE AND MEMBER ASSERTIONS
--------------------------
TypeAssertions (Type subject):

    Be<TExpected>(...) / Be(Type expected, ...) / NotBe...
    BeAssignableTo<T>(...)      NotBeAssignableTo<T>(...)
    BeDerivedFrom<TBaseClass>(...) / BeDerivedFrom(Type baseType, ...)
    Implement<TInterface>(...)  / Implement(Type interfaceType, ...)
    BeSealed(...)  NotBeSealed(...)  BeAbstract(...)  NotBeAbstract(...)
    BeStatic(...)  NotBeStatic(...)
    AndWhichConstraint<TypeAssertions,TAttribute> BeDecoratedWith<TAttribute>(...)
    BeDecoratedWith<TAttribute>(
        Expression<Func<TAttribute,bool>> isMatchingAttributePredicate, ...)
    BeDecoratedWithOrInherit<TAttribute>(...)   NotBeDecoratedWith<TAttribute>...
    AndWhichConstraint<TypeAssertions,PropertyInfo> HaveProperty<TProperty>(
        string name, ...)
    HaveProperty(Type propertyType, string name, ...)  NotHaveProperty(string,...)
    HaveExplicitProperty<TInterface>(string name, ...)
    HaveExplicitMethod<TInterface>(string name,
        IEnumerable<Type> parameterTypes, ...)
    HaveExplicitMethod(Type interfaceType, string name,
        IEnumerable<Type> parameterTypes, ...)
    AndWhichConstraint<TypeAssertions,MethodInfo> HaveMethod(string name,
        IEnumerable<Type> parameterTypes, ...)
    NotHaveMethod(string name, IEnumerable<Type> parameterTypes, ...)
    AndWhichConstraint<TypeAssertions,PropertyInfo> HaveIndexer(
        Type indexerType, IEnumerable<Type> parameterTypes, ...)
    NotHaveIndexer(IEnumerable<Type> parameterTypes, ...)
    AndWhichConstraint<TypeAssertions,ConstructorInfo> HaveConstructor(
        IEnumerable<Type> parameterTypes, ...)
    HaveDefaultConstructor(...)   NotHaveDefaultConstructor(...)
    HaveAccessModifier(CSharpAccessModifier accessModifier, ...)
    NotHaveAccessModifier(CSharpAccessModifier accessModifier, ...)
    AndWhichConstraint<TypeAssertions,MethodInfo>
        HaveImplicitConversionOperator<TSource,TTarget>(...)
    HaveImplicitConversionOperator(Type sourceType, Type targetType, ...)
    HaveExplicitConversionOperator<TSource,TTarget>(...)
    HaveExplicitConversionOperator(Type sourceType, Type targetType, ...)
    NotHaveImplicitConversionOperator... / NotHaveExplicitConversionOperator...

CSharpAccessModifier (namespace SilverAssertions.Common) values: Public,
Private, Protected, Internal, ProtectedInternal, PrivateProtected,
InvalidForCSharp.

MethodInfoAssertions (base MethodBaseAssertions -> MemberInfoAssertions):

    BeVirtual(...)   NotBeVirtual(...)   BeAsync(...)   NotBeAsync(...)
    ReturnVoid(...)  NotReturnVoid(...)
    Return<TReturn>(...) / Return(Type returnType, ...)  NotReturn...
    HaveAccessModifier(CSharpAccessModifier, ...)   NotHaveAccessModifier(...)
    BeDecoratedWith<TAttribute>(...)   NotBeDecoratedWith<TAttribute>(...)

PropertyInfoAssertions:

    BeVirtual(...)    NotBeVirtual(...)
    BeReadable(...)   BeReadable(CSharpAccessModifier accessModifier, ...)
    NotBeReadable(...)
    BeWritable(...)   BeWritable(CSharpAccessModifier accessModifier, ...)
    NotBeWritable(...)
    Return<TReturn>(...) / Return(Type propertyType, ...)   NotReturn...
    BeDecoratedWith<TAttribute>(...)

ConstructorInfoAssertions has the MethodBaseAssertions members.

"Virtual" here means overridable, not the raw IL flag: a method that is virtual
AND final - which is how the compiler emits an implicit interface
implementation - satisfies NotBeVirtual. The same rule is applied to property
accessors.

AssemblyAssertions (namespace SilverAssertions.Reflection):

    Reference(Assembly assembly, ...)      NotReference(Assembly assembly, ...)
    AndWhichConstraint<AssemblyAssertions,Type> DefineType(string @namespace,
                                                           string name, ...)
    BeUnsigned(...)   BeSignedWithPublicKey(string publicKey, ...)

Usage (all of this compiles and passes against a type you control):

    using SilverAssertions;
    using SilverAssertions.Common;

    public interface IAudited
    {
        void Touch();
    }

    public sealed class Widget : IDisposable, IAudited
    {
        public Widget() { }
        public Widget(int size) => Size = size;
        public int Size { get; }
        public string this[int index] => index.ToString();
        public void Dispose() { }
        public bool Matches(string pattern) => true;
        void IAudited.Touch() { }
        public static implicit operator string(Widget w) => "widget";
    }

    typeof(Widget).Should().BeSealed();
    typeof(Widget).Should().NotBeAbstract().And.NotBeStatic();
    typeof(Widget).Should().Implement<IDisposable>();
    typeof(Widget).Should().BeDerivedFrom<object>();
    typeof(Widget).Should().HaveAccessModifier(CSharpAccessModifier.Public);
    typeof(Widget).Should().HaveDefaultConstructor();
    typeof(Widget).Should().HaveConstructor(new[] { typeof(int) });
    typeof(Widget).Should().HaveProperty<int>("Size")
        .Which.Should().NotBeWritable();
    typeof(Widget).Should().HaveMethod("Matches", new[] { typeof(string) })
        .Which.Should().Return<bool>();
    typeof(Widget).Should().HaveIndexer(typeof(string),
        new[] { typeof(int) });                       // string this[int]
    typeof(Widget).Should()
        .HaveImplicitConversionOperator<Widget, string>();
    typeof(Widget).Should().HaveExplicitMethod(typeof(IAudited), "Touch",
        Array.Empty<Type>());

    var method = typeof(Widget).GetMethod(nameof(Widget.Matches));
    method.Should().NotBeVirtual().And.NotBeAsync();
    method.Should().Return<bool>();

    var property = typeof(Widget).GetProperty(nameof(Widget.Size));
    property.Should().BeReadable().And.NotBeWritable();
    property.Should().Return<int>();

    // architecture rule: the domain assembly must not depend on the UI one
    typeof(Widget).Assembly.Should().NotReference(typeof(MainWindow).Assembly);

TYPE, METHOD AND PROPERTY SELECTORS
-----------------------------------
The selector types (namespace SilverAssertions.Types) turn an assembly or a set
of types into a filtered set that can be asserted as a whole.

    AllTypes.From(Assembly assembly)          -> TypeSelector
    new TypeSelector(Type type) / new TypeSelector(IEnumerable<Type> types)
    new MethodInfoSelector(Type type) / (IEnumerable<Type> types)
    new PropertyInfoSelector(Type type) / (IEnumerable<Type> types)

TypeExtensions (namespace SilverAssertions) is the fluent way in:

    TypeSelector         Types(this Assembly assembly)
    TypeSelector         Types(this Type type)
    TypeSelector         Types(this IEnumerable<Type> types)
    MethodInfoSelector   Methods(this Type type)
    MethodInfoSelector   Methods(this TypeSelector typeSelector)
    PropertyInfoSelector Properties(this Type type)
    PropertyInfoSelector Properties(this TypeSelector typeSelector)

    assembly.Types().ThatAreClasses().Should().BeSealed();
    typeof(Widget).Methods().ThatArePublicOrInternal.Should().NotBeVirtual();

TypeSelector filters (each returns TypeSelector, so they chain; ToArray() ends
the chain):

    ThatDeriveFrom<TBase>()          ThatDoNotDeriveFrom<TBase>()
    ThatImplement<TInterface>()      ThatDoNotImplement<TInterface>()
    ThatAreDecoratedWith<TAttribute>()   ThatAreNotDecoratedWith<TAttribute>()
    ThatAreDecoratedWithOrInherit<TAttribute>()
    ThatAreNotDecoratedWithOrInherit<TAttribute>()
    ThatAreInNamespace(string @namespace)     ThatAreNotInNamespace(string)
    ThatAreUnderNamespace(string @namespace)  ThatAreNotUnderNamespace(string)
    ThatAreClasses()      ThatAreNotClasses()
    ThatAreInterfaces()   ThatAreNotInterfaces()
    ThatAreValueTypes()   ThatAreNotValueTypes()
    ThatAreAbstract()     ThatAreNotAbstract()
    ThatAreSealed()       ThatAreNotSealed()
    ThatAreStatic()       ThatAreNotStatic()
    ThatSatisfy(Func<Type, bool> predicate)
    UnwrapTaskTypes()     UnwrapEnumerableTypes()

TypeSelectorAssertions (what "typeSelector.Should()" returns):

    BeSealed(...)  NotBeSealed(...)
    BeInNamespace(string @namespace, ...)   NotBeInNamespace(string, ...)
    BeUnderNamespace(string @namespace, ...) NotBeUnderNamespace(string, ...)
    BeDecoratedWith<TAttribute>(...)  BeDecoratedWithOrInherit<TAttribute>(...)
    NotBeDecoratedWith<TAttribute>(...)
    NotBeDecoratedWithOrInherit<TAttribute>(...)

MethodInfoSelector filters: ThatArePublicOrInternal, ThatReturnVoid,
ThatDoNotReturnVoid, ThatReturn<TReturn>(), ThatDoNotReturn<TReturn>(),
ThatAreDecoratedWith<TAttribute>(), ThatAreNotDecoratedWith<TAttribute>(),
ThatAreDecoratedWithOrInherit<TAttribute>(),
ThatAreNotDecoratedWithOrInherit<TAttribute>(), ThatAreAbstract(),
ThatAreNotAbstract(), ThatAreAsync(), ThatAreNotAsync(), ThatAreStatic(),
ThatAreNotStatic(), ThatAreVirtual(), ThatAreNotVirtual(), ReturnTypes()
(-> TypeSelector), ToArray().
MethodInfoSelectorAssertions: BeVirtual, NotBeVirtual, BeAsync, NotBeAsync,
BeDecoratedWith<TAttribute>, NotBeDecoratedWith<TAttribute>,
Be(CSharpAccessModifier, ...), NotBe(CSharpAccessModifier, ...).

PropertyInfoSelector filters: ThatArePublicOrInternal, ThatAreAbstract,
ThatAreNotAbstract, ThatAreStatic, ThatAreNotStatic, ThatAreVirtual,
ThatAreNotVirtual (these seven are properties, not methods),
ThatAreDecoratedWith<TAttribute>(), ThatAreNotDecoratedWith<TAttribute>(),
ThatAreDecoratedWithOrInherit<TAttribute>(),
ThatAreNotDecoratedWithOrInherit<TAttribute>(), OfType<TReturn>(),
NotOfType<TReturn>(), ReturnTypes(), ToArray().
PropertyInfoSelectorAssertions: BeVirtual, NotBeVirtual, BeWritable,
NotBeWritable, BeDecoratedWith<TAttribute>, NotBeDecoratedWith<TAttribute>.

TypeEnumerableExtensions (namespace SilverAssertions) offers the same filters
over a plain IEnumerable<Type> - ThatAreDecoratedWith<TAttribute>,
ThatAreDecoratedWithOrInherit<TAttribute>, ThatAreNotDecoratedWith<TAttribute>,
ThatAreNotDecoratedWithOrInherit<TAttribute>, ThatAreInNamespace,
ThatAreUnderNamespace, ThatDeriveFrom<T>, ThatImplement<T>, ThatAreClasses,
ThatAreNotClasses, ThatAreStatic, ThatAreNotStatic, ThatSatisfy,
UnwrapTaskTypes, UnwrapEnumerableTypes - but they return IEnumerable<Type>, and
an IEnumerable<Type> gets GenericCollectionAssertions<Type>, not
TypeSelectorAssertions.

    using SilverAssertions.Types;

    // CORRECT - start from a TypeSelector to get the type-level assertions:
    AllTypes.From(typeof(Widget).Assembly)
        .ThatAreClasses()
        .ThatImplement<IDisposable>()
        .Should().BeSealed();

    // ALSO CORRECT - wrap an existing sequence:
    new TypeSelector(myTypes).ThatAreClasses().Should().BeSealed();

    // WRONG - assembly.GetTypes().ThatAreClasses() is IEnumerable<Type>,
    // so .Should() gives a collection assertion with no BeSealed member.

    new MethodInfoSelector(typeof(Widget)).ThatArePublicOrInternal
        .ThatReturnVoid.Should().NotBeAsync();

    new PropertyInfoSelector(typeof(Widget)).ThatArePublicOrInternal
        .Should().NotBeWritable();

EVENT MONITORING
----------------
    IMonitor<T> Monitor<T>(this T eventSource, Func<DateTime> utcNow = null)

IMonitor<T> : IDisposable exposes:

    T Subject { get; }
    void Clear();
    EventAssertions<T> Should();
    IEventRecording GetRecordingFor(string eventName);
    EventMetadata[] MonitoredEvents { get; }     // EventName, HandlerType
    OccurredEvent[] OccurredEvents { get; }      // EventName, Parameters,
                                                 // TimestampUtc, Sequence

EventAssertions<T>:

    IEventRecording Raise(string eventName, ...)
    void            NotRaise(string eventName, ...)
    IEventRecording RaisePropertyChangeFor(
        Expression<Func<T, object>> propertyExpression, ...)
    void            NotRaisePropertyChangeFor(
        Expression<Func<T, object>> propertyExpression, ...)

IEventRecording : IEnumerable<OccurredEvent> exposes EventObject, EventName and
EventHandlerType. EventRaisingExtensions (namespace SilverAssertions) refines a
recording:

    IEventRecording WithSender(this IEventRecording r, object expectedSender)
    IEventRecording WithArgs<T>(this IEventRecording r,
                                Expression<Func<T, bool>> predicate)
    IEventRecording WithArgs<T>(this IEventRecording r,
                                params Expression<Func<T, bool>>[] predicates)

Usage:

    using System.ComponentModel;
    using SilverAssertions;

    var subject = new Person();

    using var monitor = subject.Monitor();

    subject.Name = "Alice";

    monitor.Should().Raise(nameof(INotifyPropertyChanged.PropertyChanged))
        .WithSender(subject)
        .WithArgs<PropertyChangedEventArgs>(a => a.PropertyName == "Name");

    monitor.Should().RaisePropertyChangeFor(p => p.Name);
    monitor.Should().NotRaise("Deleted");

    // count occurrences yourself - Raise has no occurrence-constraint overload
    monitor.GetRecordingFor("PropertyChanged").Should().HaveCount(1);

    monitor.Clear();                 // forget everything recorded so far
    monitor.OccurredEvents.Should().BeEmpty();

The monitor holds a weak reference to the subject and subscribes to every event
the subject declares; dispose it (or use "using") to unsubscribe.

STREAM ASSERTIONS (StreamAssertions, BufferedStreamAssertions)
---------------------------------------------------------------
    BeReadable(...)   NotBeReadable(...)
    BeWritable(...)   NotBeWritable(...)
    BeSeekable(...)   NotBeSeekable(...)
    BeReadOnly(...)   NotBeReadOnly(...)
    BeWriteOnly(...)  NotBeWriteOnly(...)
    HaveLength(long expected, ...)     NotHaveLength(long unexpected, ...)
    HavePosition(long expected, ...)   NotHavePosition(long unexpected, ...)
    // BufferedStreamAssertions adds:
    HaveBufferSize(int expected, ...)  NotHaveBufferSize(int unexpected, ...)

    using var stream = new MemoryStream(new byte[] { 1, 2, 3 });
    stream.Should().BeReadable().And.BeSeekable().And.HaveLength(3);
    stream.Should().HavePosition(0);

    using var buffered = new BufferedStream(stream, 1024);
    buffered.Should().HaveBufferSize(1024);

XML ASSERTIONS
--------------
XDocumentAssertions:

    Be(XDocument expected, ...)             NotBe(XDocument unexpected, ...)
    BeEquivalentTo(XDocument expected, ...) NotBeEquivalentTo(XDocument, ...)
    AndWhichConstraint<XDocumentAssertions,XElement> HaveRoot(string expected,..)
    HaveRoot(XName expected, ...)
    AndWhichConstraint<XDocumentAssertions,XElement> HaveElement(string, ...)
    HaveElement(XName expected, ...)
    HaveElement(string|XName expected, OccurrenceConstraint occurrence, ...)
        -> AndWhichConstraint<XDocumentAssertions, IEnumerable<XElement>>

XElementAssertions: Be / NotBe / BeEquivalentTo / NotBeEquivalentTo,
HaveValue(string expected, ...), HaveAttribute(string|XName expectedName,
string expectedValue, ...), HaveElement(string|XName, ...) and the
OccurrenceConstraint overloads.

XAttributeAssertions: Be / NotBe / HaveValue(string expected, ...).

XmlNodeAssertions (System.Xml): BeEquivalentTo(XmlNode expected, ...) /
NotBeEquivalentTo. XmlElementAssertions adds HaveInnerText(string expected, ...),
HaveAttribute(string expectedName, string expectedValue, ...),
HaveAttributeWithNamespace(string expectedName, string expectedNamespace,
string expectedValue, ...), HaveElement(string expectedName, ...) and
HaveElementWithNamespace(string expectedName, string expectedNamespace, ...).
The XmlNode/XmlElement ".Should()" overloads live on XmlAssertionExtensions.

    var doc = XDocument.Parse("<root><child>value</child></root>");
    doc.Should().HaveRoot("root")
        .Which.Should().HaveElement("child")
        .Which.Should().HaveValue("value");

    var element = XElement.Parse("<item id='1'>content</item>");
    element.Should().HaveAttribute("id", "1").And.HaveValue("content");

HTTP RESPONSE ASSERTIONS (HttpResponseMessageAssertions)
---------------------------------------------------------
    BeSuccessful(...)      // 2xx
    BeRedirection(...)     // 3xx
    HaveError(...)         // 4xx or 5xx
    HaveClientError(...)   // 4xx
    HaveServerError(...)   // 5xx
    HaveStatusCode(HttpStatusCode expected, ...)
    NotHaveStatusCode(HttpStatusCode unexpected, ...)

    response.Should().BeSuccessful();
    response.Should().HaveStatusCode(HttpStatusCode.OK);
    errorResponse.Should().HaveClientError();

SYSTEM.DATA ASSERTIONS (SilverAssertions.Data)
-----------------------------------------------
DataSetAssertions<TDataSet>: HaveTableCount(int expected, ...),
HaveTable(string expectedTableName, ...) -> AndWhichConstraint<..., DataTable>,
HaveTables(params string[] expectedTableNames), BeEquivalentTo(DataSet, ...).

DataTableAssertions<TDataTable>: HaveRowCount(int expected, ...),
HaveColumn(string expectedColumnName, ...) -> AndWhichConstraint<..., DataColumn>,
HaveColumns(params string[] expectedColumnNames), BeEquivalentTo(DataTable, ...).

DataRowAssertions<TDataRow>: HaveColumn / HaveColumns / BeEquivalentTo(DataRow).
DataColumnAssertions: BeEquivalentTo(DataColumn expectation, ...).

Each BeEquivalentTo has a second overload taking
Func<IDataEquivalencyAssertionOptions<T>, IDataEquivalencyAssertionOptions<T>>
with data-specific options: AllowingMismatchedTypes(),
IgnoringUnmatchedColumns(), UsingRowMatchMode(RowMatchMode rowMatchMode),
ExcludingOriginalData(), ExcludingTable(string), ExcludingTables(params
string[]), ExcludingColumn(DataColumn), ExcludingColumn(string tableName,
string columnName), ExcludingColumns(...), ExcludingColumnInAllTables(string),
ExcludingColumnsInAllTables(...), ExcludingRelated(...) for DataColumn,
DataRelation, DataRow, DataTable, Constraint, ForeignKeyConstraint and
UniqueConstraint, and Excluding(Expression<Func<IMemberInfo,bool>>).
RowMatchMode values: Index, PrimaryKey.

DataTableCollection, DataRowCollection and DataColumnCollection get
GenericCollectionAssertions over their element type, extended by
DataTableCollectionAssertionExtensions, DataRowCollectionAssertionExtensions and
DataColumnCollectionAssertionExtensions with BeSameAs / NotBeSameAs /
HaveSameCount / NotHaveSameCount overloads that accept the non-generic
collection type.

    dataSet.Should().HaveTableCount(2)
        .And.HaveTable("Orders")
        .Which.Should().HaveRowCount(10).And.HaveColumn("Total");

    actualTable.Should().BeEquivalentTo(expectedTable, options => options
        .UsingRowMatchMode(RowMatchMode.PrimaryKey)
        .ExcludingColumn("Orders", "UpdatedAt"));

ASSERTION SCOPE
---------------
An AssertionScope (namespace SilverAssertions.Execution) batches failures: every
assertion inside the scope runs, and all failures are reported together when the
scope is disposed.

    public AssertionScope()
    public AssertionScope(string context)
    public AssertionScope(Lazy<string> context)
    public AssertionScope(IAssertionStrategy assertionStrategy)

    static AssertionScope Current { get; }
    AssertionScope BecauseOf(string because, params object[] becauseArgs)
    AssertionScope ForCondition(bool condition)
    AssertionScope ForConstraint(OccurrenceConstraint constraint,
                                 int actualOccurrences)
    AssertionScope WithExpectation(string message, params object[] args)
    AssertionScope WithDefaultIdentifier(string identifier)
    AssertionScope UsingLineBreaks { get; }
    Continuation FailWith(string message, params object[] args)
    Continuation FailWith(Func<FailReason> failReasonFunc)
    Continuation ClearExpectation()
    GivenSelector<T> Given<T>(Func<T> selector)
    void AddReportable(string key, string value)
    void AddReportable(string key, Func<string> valueFunc)
    void AddNonReportable(string key, object value)
    T Get<T>(string key)
    bool HasFailures()
    string[] Discard()
    FormattingOptions FormattingOptions { get; }
    string CallerIdentity { get; }

Usage:

    using SilverAssertions;
    using SilverAssertions.Execution;

    using (new AssertionScope())
    {
        5.Should().Be(10);            // fails, but execution continues
        "hello".Should().Be("world"); // also fails
        true.Should().BeFalse();      // also fails
    }
    // one failure message listing all three

    using (new AssertionScope("the parsed invoice"))
    {
        invoice.Number.Should().Be("INV-1");
        invoice.Total.Should().BePositive();
    }
    // messages read "Expected the parsed invoice ..."

Scopes nest: an inner scope reports its failures to the outer one instead of
throwing. Discard() takes the collected failures and clears them (useful when
writing your own assertion that inspects failures).

FORMATTING (SilverAssertions.Formatting)
-----------------------------------------
Failure messages render values through Formatter:

    static string ToString(object value, FormattingOptions options = null)
    static void AddFormatter(IValueFormatter formatter)
    static void RemoveFormatter(IValueFormatter formatter)
    static IEnumerable<IValueFormatter> Formatters { get; }

FormattingOptions: UseLineBreaks (default false), MaxDepth (default 5),
MaxLines (default 100). The global instance is AssertionOptions.FormattingOptions;
each AssertionScope gets its own clone through scope.FormattingOptions.
Exceeding MaxLines throws MaxLinesExceededException internally, which the
formatter turns into a truncated message.

Built-in formatters (all public, all implementing IValueFormatter, so they can
be subclassed or removed): AggregateExceptionValueFormatter,
AttributeBasedFormatter, ByteValueFormatter, DateOnlyValueFormatter,
DateTimeOffsetValueFormatter, DecimalValueFormatter, DefaultValueFormatter,
DictionaryValueFormatter, DoubleValueFormatter, EnumValueFormatter,
EnumerableValueFormatter, ExceptionValueFormatter, ExpressionValueFormatter,
GuidValueFormatter, Int16ValueFormatter, Int32ValueFormatter,
Int64ValueFormatter, MultidimensionalArrayFormatter, NullValueFormatter,
PredicateLambdaExpressionValueFormatter, PropertyInfoFormatter,
SByteValueFormatter, SingleValueFormatter, StringValueFormatter, TaskFormatter,
TimeOnlyValueFormatter, TimeSpanValueFormatter, UInt16ValueFormatter,
UInt32ValueFormatter, UInt64ValueFormatter, XAttributeValueFormatter,
XDocumentValueFormatter, XElementValueFormatter, XmlNodeFormatter and
XmlReaderValueFormatter.

CONFIGURATION (SilverAssertions.Common)
----------------------------------------
Configuration.Current (a Configuration over an IConfigurationStore) exposes:

    string TestFrameworkName { get; set; }
        // app setting "SilverAssertions.TestFramework"
    ValueFormatterDetectionMode ValueFormatterDetectionMode { get; set; }
        // app setting "valueFormatters": Disabled (default), Specific, Scan
    string ValueFormatterAssembly { get; set; }
        // app setting "valueFormattersAssembly"; setting it implies Specific

Services exposes ConfigurationStore, Configuration, ThrowException, Reflector
and ResetToDefaults() for tests that need to swap the plumbing.

================================================================================

EXTENSIBILITY
=============
There are four extension points, in increasing order of depth.

1. AN EXTENSION METHOD ON AN EXISTING ASSERTION CLASS
-----------------------------------------------------
The cheapest option: add a method to StringAssertions, NumericAssertions<T>,
GenericCollectionAssertions<T> or any other assertion class, and return the
constraint so the chain continues.

    using SilverAssertions;
    using SilverAssertions.Execution;
    using SilverAssertions.Primitives;

    public static class StringAssertionsExtensions
    {
        [CustomAssertion]
        public static AndConstraint<StringAssertions> BeAValidEmailAddress(
            this StringAssertions assertions,
            string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(assertions.Subject is { Length: > 0 } s
                              && s.Contains('@'))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:string} to be a valid e-mail "
                          + "address{reason}, but found {0}.",
                          assertions.Subject);

            return new AndConstraint<StringAssertions>(assertions);
        }
    }

    // usage
    user.Email.Should().BeAValidEmailAddress().And.EndWith(".com");

2. A CUSTOM ASSERTION CLASS PLUS ITS OWN .Should()
--------------------------------------------------
Give a domain type its own assertion vocabulary. Derive from
ReferenceTypeAssertions<TSubject,TAssertions> to inherit BeNull, BeOfType,
BeSameAs, Match and the Subject property.

    using SilverAssertions;
    using SilverAssertions.Execution;
    using SilverAssertions.Primitives;

    public class Customer
    {
        public string Name { get; set; }
        public bool Active { get; set; }
    }

    public class CustomerAssertions
        : ReferenceTypeAssertions<Customer, CustomerAssertions>
    {
        public CustomerAssertions(Customer subject) : base(subject) { }

        protected override string Identifier => "customer";

        [CustomAssertion]
        public AndConstraint<CustomerAssertions> BeActive(
            string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected {context:customer} to be "
                                 + "active{reason}, ")
                .Given(() => Subject)
                .ForCondition(customer => customer is not null)
                .FailWith("but it was <null>.")
                .Then
                .ForCondition(customer => customer.Active)
                .FailWith("but {0} is dormant.", customer => customer.Name)
                .Then
                .ClearExpectation();

            return new AndConstraint<CustomerAssertions>(this);
        }
    }

    public static class CustomerExtensions
    {
        public static CustomerAssertions Should(this Customer customer)
            => new CustomerAssertions(customer);
    }

    // usage
    myCustomer.Should().BeActive("we do not work with dormant customers")
        .And.NotBeNull();

The Execute.Assertion chain (an AssertionScope):

    Execute.Assertion
        .BecauseOf(because, becauseArgs)      // records the reason
        .ForCondition(<bool>)                 // the test
        .FailWith("message {0}.", value);     // message when the test failed

Chaining several conditions needs care: the ForCondition ARGUMENT is evaluated
eagerly, even when an earlier condition already failed, so
".ForCondition(Subject is not null).FailWith(...).Then.ForCondition(Subject.X)"
throws NullReferenceException. Use Given<T>, whose conditions are lambdas and
are only evaluated while the chain is still succeeding:

    Execute.Assertion
        .BecauseOf(because, becauseArgs)
        .WithExpectation("Expected {context:collection} to be usable{reason}, ")
        .Given(() => Subject)
        .ForCondition(subject => subject is not null)
        .FailWith("but found <null>.")
        .Then
        .ForCondition(subject => subject.Any())
        .FailWith("but the collection was empty.")
        .Then
        .ClearExpectation();

WithExpectation sets a prefix that every following FailWith message is appended
to (which is why those messages start with "but "); ClearExpectation ends it.

Message placeholders: "{reason}" is replaced by the because phrase,
"{context:xxx}" by the caller-supplied identifier (or "xxx" as the fallback),
and "{0}", "{1}" ... by the formatted arguments. FailWith also accepts
Func<object>[] argument providers, and Func<FailReason> for a fully computed
reason (FailReason(string message, params object[] args)).

[CustomAssertion] (CustomAssertionAttribute, AttributeTargets.Method, namespace
SilverAssertions) marks a method so that CallerIdentifier skips it when working
out the subject name for "{context}". Without it, failure messages name your
helper instead of the variable under test.

3. A CUSTOM VALUE FORMATTER
---------------------------
Two ways. Implement IValueFormatter and register it:

    using SilverAssertions.Formatting;

    public class MoneyFormatter : IValueFormatter
    {
        public bool CanHandle(object value) => value is Money;

        public void Format(object value, FormattedObjectGraph formattedGraph,
            FormattingContext context, FormatChild formatChild)
        {
            var money = (Money)value;
            formattedGraph.AddFragment($"{money.Amount} {money.Currency}");
        }
    }

    Formatter.AddFormatter(new MoneyFormatter());     // once, e.g. in a fixture
    // Formatter.RemoveFormatter(instance) to undo

FormattedObjectGraph offers AddFragment, AddFragmentOnNewLine, AddLine,
WithIndentation() and LineCount; FormattingContext offers UseLineBreaks. Do not
call Formatter.ToString from inside Format - call the supplied FormatChild
delegate (childPath, value, formattedGraph) so cyclic references stay detected.

Or write a static method marked with [ValueFormatter]:

    using SilverAssertions.Formatting;

    public static class CustomFormatters
    {
        [ValueFormatter]
        public static void Format(Money value, FormattedObjectGraph output)
        {
            output.AddFragment($"{value.Amount} {value.Currency}");
        }
    }

Attribute-based formatters are found by AttributeBasedFormatter, which only
scans when detection is enabled - set the "valueFormatters" app setting to
"Scan", or set Configuration.Current.ValueFormatterDetectionMode =
ValueFormatterDetectionMode.Scan (or .Specific together with
ValueFormatterAssembly). The default is Disabled.

4. A CUSTOM EQUIVALENCY STEP OR RULE
------------------------------------
BeEquivalentTo runs an ordered list of IEquivalencyStep implementations:

    public interface IEquivalencyStep
    {
        EquivalencyResult Handle(Comparands comparands,
            IEquivalencyValidationContext context,
            IEquivalencyValidator nestedValidator);
    }

Derive from EquivalencyStep<T> to handle only expectations assignable to T:

    using SilverAssertions.Equivalency;

    public class MoneyEquivalencyStep : EquivalencyStep<Money>
    {
        protected override EquivalencyResult OnHandle(Comparands comparands,
            IEquivalencyValidationContext context,
            IEquivalencyValidator nestedValidator)
        {
            var subject = (Money)comparands.Subject;
            var expectation = (Money)comparands.Expectation;

            subject.Currency.Should().Be(expectation.Currency);
            subject.Amount.Should().BeApproximately(expectation.Amount, 0.001m);

            return EquivalencyResult.AssertionCompleted;
        }
    }

    // per assertion:
    actual.Should().BeEquivalentTo(expected,
        o => o.Using(new MoneyEquivalencyStep()));

    // or globally, through the plan:
    AssertionOptions.EquivalencyPlan.Add<MoneyEquivalencyStep>();

EquivalencyResult values: ContinueWithNext, AssertionCompleted. Comparands
exposes Subject, Expectation, CompileTimeType, RuntimeType and
GetExpectedType(IEquivalencyAssertionOptions). IEquivalencyValidationContext
exposes CurrentNode (INode), Reason, Tracer, Options, IsCyclicReference(object),
AsNestedMember(IMember), AsCollectionItem<TItem>(string index),
AsDictionaryItem<TKey,TExpectation>(TKey key) and Clone().

EquivalencyPlan (AssertionOptions.EquivalencyPlan) manages the step list:
Add<TStep>(), AddAfter<TPredecessor,TStep>(), Insert<TStep>(),
InsertBefore<TSuccessor,TStep>(), Remove<TStep>(), Clear(), Reset(). The
built-in steps you can position against include
RunAllUserStepsEquivalencyStep, AutoConversionStep,
ReferenceEqualityEquivalencyStep, GenericDictionaryEquivalencyStep,
DictionaryEquivalencyStep, GenericEnumerableEquivalencyStep,
EnumerableEquivalencyStep, StringEqualityEquivalencyStep,
SimpleEqualityEquivalencyStep, EnumEqualityStep,
StructuralEqualityEquivalencyStep, ValueTypeEquivalencyStep,
AssertionRuleEquivalencyStep, EqualityComparerEquivalencyStep,
ConstraintEquivalencyStep, ConstraintCollectionEquivalencyStep,
DataSetEquivalencyStep, DataTableEquivalencyStep, DataColumnEquivalencyStep,
DataRowEquivalencyStep, DataRowCollectionEquivalencyStep,
DataRelationEquivalencyStep, XDocumentEquivalencyStep, XElementEquivalencyStep,
XAttributeEquivalencyStep and MultiDimensionalArrayEquivalencyStep.

Finer-grained seams, all injected with options.Using(...):
IMemberSelectionRule (which members take part), IMemberMatchingRule (how a
subject member is found for an expectation member), IOrderingRule (where order
matters), plus MemberSelectionContext, MemberFactory, Node/INode, Field,
Property, MemberVisibility (None, Internal, Public, ExplicitlyImplemented,
DefaultInterfaceProperties), EqualityStrategy (Equals, Members, ForceEquals,
ForceMembers), EnumEquivalencyHandling (ByValue, ByName), CyclicReferenceHandling
(Ignore, ThrowException), OrderStrictness (Strict, NotStrict, Irrelevant) and
ConversionSelector. Tracing goes through ITraceWriter / StringBuilderTraceWriter
and the Tracer exposed on the validation context.

INFRASTRUCTURE TYPES YOU RARELY TOUCH
-------------------------------------
These are public because the extension points need them, not because a normal
test uses them:

    IClock / ITimer            the time source behind NotThrowAfter,
                               CompleteWithinAsync and ExecutionTime; most
                               delegate/task assertion classes have a
                               constructor overload taking an IClock so a test
                               can supply a deterministic clock
    Services / IReflector      the swappable plumbing (configuration store,
                               exception thrower, assembly scanner);
                               Services.ResetToDefaults() puts it back
    IConfigurationStore        where Configuration reads its app settings
    EquivalencyValidator /     the default IEquivalencyValidator and
    EquivalencyValidationContext   IEquivalencyValidationContext implementations
    OrderingRuleCollection     the IOrderingRule list held by the options
    ICloneable2                the clone contract used to copy options objects
    ICollectionWrapper<T>      exposes UnderlyingCollection for wrapped
                               System.Data collections
    ContinuationOfGiven<T> /   the values returned inside an Execute.Assertion
    Continuation /             chain; you get them from Then/FailWith rather
    ContinuedAssertionScope        than constructing them
    TimeSpanCondition          MoreThan, AtLeast, Exactly, Within, LessThan -
                               the enum behind the DateTime range assertions
    DateTimeExtensions         ToDateTimeOffset(this DateTime[, TimeSpan])
    DataSetAssertionExtensions, DataTableAssertionExtensions,
    DataRowAssertionExtensions the classes that carry the System.Data
                               ".Should()" overloads

================================================================================

COMPLETE EXAMPLES
=================

Example 1: Testing a service method
-----------------------------------
    using SilverAssertions;
    using Xunit;

    public class UserServiceTests
    {
        [Fact]
        public void GetUser_returns_the_expected_user()
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
    }

Example 2: Testing exception behaviour
--------------------------------------
    using System;
    using SilverAssertions;
    using Xunit;

    public class UserServiceExceptionTests
    {
        [Fact]
        public void GetUser_with_an_invalid_id_throws()
        {
            var service = new UserService();

            Action act = () => service.GetUser(-1);

            act.Should().Throw<ArgumentException>()
                .WithMessage("*invalid*")
                .WithParameterName("id")
                .And.ParamName.Should().Be("id");
        }
    }

Example 3: Async - throwing, completing and producing a result
--------------------------------------------------------------
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using SilverAssertions;
    using Xunit;

    public class UserServiceAsyncTests
    {
        [Fact]
        public async Task LoadAsync_rejects_a_negative_id()
        {
            var service = new UserService();

            Func<Task> act = () => service.LoadAsync(-1);

            await act.Should().ThrowAsync<ArgumentOutOfRangeException>()
                .WithParameterName("id");
        }

        [Fact]
        public async Task GetUsersAsync_completes_quickly_and_is_sorted()
        {
            var service = new UserService();

            Func<Task<IReadOnlyList<User>>> act = () => service.GetUsersAsync();

            var users = (await act.Should()
                .CompleteWithinAsync(TimeSpan.FromSeconds(2))).Which;

            users.Should().HaveCountGreaterThan(0);
            users.Should().OnlyContain(u => u.IsActive);
            users.Should().BeInAscendingOrder(u => u.Name);
        }

        [Fact]
        public async Task CountAsync_returns_three()
        {
            var service = new UserService();

            Func<Task<int>> act = () => service.CountAsync();

            await act.Should().CompleteWithinAsync(TimeSpan.FromSeconds(2))
                .WithResult(3);
        }
    }

Example 4: Object graph comparison with options
-----------------------------------------------
    using SilverAssertions;
    using Xunit;

    public class OrderServiceTests
    {
        [Fact]
        public void CreateOrder_returns_the_expected_order()
        {
            var service = new OrderService();

            var order = service.CreateOrder(userId: 1, productId: 42,
                                            quantity: 3);

            // The anonymous expectation names only the members that matter,
            // so Id and CreatedAt are simply not compared.
            order.Should().BeEquivalentTo(new
            {
                UserId = 1,
                ProductId = 42,
                Quantity = 3,
                Status = OrderStatus.Pending
            });
        }

        [Fact]
        public void CreateOrder_matches_the_prototype_except_for_identity()
        {
            var service = new OrderService();
            var expected = OrderPrototype.Pending(userId: 1, productId: 42);

            var order = service.CreateOrder(userId: 1, productId: 42,
                                            quantity: 3);

            // Here the expectation IS an Order, so Excluding can name its
            // members - and every other member is compared, including
            // nested Customer and the Lines collection.
            order.Should().BeEquivalentTo(expected, options => options
                .Excluding(o => o.Id)
                .Excluding(o => o.CreatedAt)
                .WithStrictOrderingFor(o => o.Lines)
                .ComparingByMembers<Money>());
        }
    }

Example 5: Event monitoring
---------------------------
    using System.ComponentModel;
    using SilverAssertions;
    using Xunit;

    public class PersonTests
    {
        [Fact]
        public void Setting_the_name_raises_PropertyChanged()
        {
            // Arrange
            var person = new Person { Name = "Bob" };

            using var monitor = person.Monitor();

            // Act
            person.Name = "Alice";

            // Assert
            monitor.Should().Raise(nameof(INotifyPropertyChanged.PropertyChanged))
                .WithSender(person)
                .WithArgs<PropertyChangedEventArgs>(
                    args => args.PropertyName == nameof(Person.Name));

            monitor.Should().NotRaise("Deleted");
            monitor.GetRecordingFor("PropertyChanged").Should().HaveCount(1);
        }
    }

Example 6: Collection validation inside an AssertionScope
---------------------------------------------------------
    using System;
    using SilverAssertions;
    using SilverAssertions.Execution;
    using Xunit;

    public class ProcessingTests
    {
        [Fact]
        public void All_processed_items_are_valid()
        {
            var items = Pipeline.GetProcessedItems();

            using (new AssertionScope("the processed items"))
            {
                items.Should().NotBeEmpty();
                items.Should().OnlyHaveUniqueItems(i => i.Id);
                items.Should().AllSatisfy(item =>
                {
                    item.Name.Should().NotBeNullOrWhiteSpace();
                    item.Price.Should().BePositive();
                    item.CreatedAt.Should().BeBefore(DateTime.UtcNow);
                });
            }
            // every violated rule is reported, not just the first
        }
    }

Example 7: A custom assertion for a domain type
-----------------------------------------------
    using SilverAssertions;
    using SilverAssertions.Execution;
    using SilverAssertions.Primitives;
    using Xunit;

    public class Invoice
    {
        public string Number { get; set; }
        public decimal Total { get; set; }
        public bool IsPaid { get; set; }
    }

    public class InvoiceAssertions
        : ReferenceTypeAssertions<Invoice, InvoiceAssertions>
    {
        public InvoiceAssertions(Invoice subject) : base(subject) { }

        protected override string Identifier => "invoice";

        [CustomAssertion]
        public AndConstraint<InvoiceAssertions> BeSettled(
            string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected {context:invoice} to be "
                                 + "settled{reason}, ")
                .Given(() => Subject)
                .ForCondition(invoice => invoice is not null)
                .FailWith("but it was <null>.")
                .Then
                .ForCondition(invoice => invoice.IsPaid)
                .FailWith("but {0} is still outstanding on {1}.",
                          invoice => invoice.Total,
                          invoice => invoice.Number)
                .Then
                .ClearExpectation();

            return new AndConstraint<InvoiceAssertions>(this);
        }
    }

    public static class InvoiceExtensions
    {
        public static InvoiceAssertions Should(this Invoice invoice)
            => new InvoiceAssertions(invoice);
    }

    public class InvoiceTests
    {
        [Fact]
        public void A_paid_invoice_is_settled()
        {
            var invoice = new Invoice
            {
                Number = "INV-1", Total = 0m, IsPaid = true
            };

            invoice.Should().BeSettled().And.NotBeNull();
        }
    }

================================================================================

MINIMUM VIABLE PROJECT
======================
A test project that uses SilverAssertions needs the test framework, its runner
and this package. With xUnit v3:

    dotnet new xunit3 -n MyProject.Tests
    cd MyProject.Tests
    dotnet add package SilverAssertions.ApacheLicenseForever

MyProject.Tests.csproj (the essentials):

    <Project Sdk="Microsoft.NET.Sdk">

      <PropertyGroup>
        <TargetFramework>net10.0</TargetFramework>
        <IsPackable>false</IsPackable>
      </PropertyGroup>

      <ItemGroup>
        <PackageReference Include="Microsoft.NET.Test.Sdk" />
        <PackageReference Include="xunit.v3" />
        <PackageReference Include="xunit.runner.visualstudio" />
        <PackageReference Include="SilverAssertions.ApacheLicenseForever" />
      </ItemGroup>

      <ItemGroup>
        <ProjectReference Include="..\MyProject\MyProject.csproj" />
      </ItemGroup>

    </Project>

CalculatorTests.cs:

    using SilverAssertions;
    using Xunit;

    namespace MyProject.Tests;

    public class CalculatorTests
    {
        [Fact]
        public void Add_returns_the_sum()
        {
            // Arrange
            var calculator = new Calculator();

            // Act
            int result = calculator.Add(2, 2);

            // Assert
            result.Should().Be(4);
        }
    }

Then:

    dotnet test

================================================================================

PERFORMANCE TIPS
================
1. BeEquivalentTo walks the whole object graph with reflection and is the most
   expensive assertion in the library. For a single value, "x.Should().Be(y)"
   is far cheaper. Keep BeEquivalentTo for graphs.

2. Narrow the graph rather than the assertion count: Including / Excluding /
   ExcludingNestedObjects cut the amount of reflection done per call.

3. Prefer an anonymous-type expectation over Excluding chains. It compares
   fewer members and needs no expression trees.

4. Do not call AllowingInfiniteRecursion routinely. It removes the depth guard;
   a self-referencing graph then costs whatever the graph costs.

5. Set global equivalency defaults once with
   AssertionOptions.AssertEquivalencyUsing instead of repeating the same
   options lambda in hundreds of tests - the lambda is invoked on every call.

6. AssertionScope is for grouping related checks so all failures surface at
   once. It allocates a scope and collects messages, so do not wrap a single
   assertion in one.

7. Formatting only happens on failure, but arguments passed to FailWith are
   evaluated eagerly. In custom assertions, use the Func<object> overloads or
   Given(...) when producing the value is expensive.

8. Prefer the specific assertion to a generic one: BePositive() over
   BeGreaterThan(0), BeEmpty() over HaveCount(0), ContainSingle(p) over
   Where(p).Should().HaveCount(1). Same cost, better failure message.

9. Collection assertions enumerate the subject. For a lazily-computed
   IEnumerable, materialise it once (ToList()) if several assertions run
   against it.

10. NotThrowAfter / NotThrowAfterAsync / NotCompleteWithinAsync spend real wall
    time. Keep waitTime small and the pollInterval sensible.

================================================================================

COMMON PITFALLS TO AVOID
========================
1. The package id is SilverAssertions.ApacheLicenseForever; the namespace is
   SilverAssertions. Do not use the package id as a using.

2. Do not add FluentAssertions to the same project, and never write
   "using FluentAssertions;". Two ".Should()" extension sets on the same type
   are ambiguous and the project will not compile.

3. AssertionScope is in SilverAssertions.Execution and CSharpAccessModifier is
   in SilverAssertions.Common. "using SilverAssertions;" alone is not enough for
   either. The same goes for AllTypes/TypeSelector (.Types), Formatter
   (.Formatting) and the 5.Seconds()-style helpers (.Extensions).

4. Forgetting to await an async assertion silently passes. ThrowAsync,
   NotThrowAsync, CompleteWithinAsync, NotCompleteWithinAsync,
   ThrowWithinAsync, ThrowExactlyAsync, NotThrowAfterAsync and the awaitable
   WithMessage/WithParameterName/WithResult all return a Task that MUST be
   awaited. Throw<T>/NotThrow exist only on the Action and Func<T> assertions;
   a Func<Task> subject offers ThrowAsync<T>/NotThrowAsync instead.

5. BeCloseTo and BeApproximately are not interchangeable. BeCloseTo exists only
   for the integral numeric types and takes an UNSIGNED delta
   (e.g. "42.Should().BeCloseTo(45, 5u)"). For float, double and decimal use
   BeApproximately(expectedValue, precision). Writing
   "3.14.Should().BeCloseTo(3.1, 0.1)" does not compile. (DateTime, TimeSpan,
   TimeOnly and DateTimeOffset do have their own BeCloseTo, taking a TimeSpan
   precision.)

6. In BeEquivalentTo, the options lambda is typed on the EXPECTATION. If the
   expectation is an anonymous object, "options.Excluding(o => o.Id)" does not
   compile unless the anonymous object itself has an Id. Exclude members only
   when the expectation is the real type; with an anonymous expectation just
   leave the member out.

7. BeEquivalentTo ignores collection order by default. Use WithStrictOrdering()
   (or Equal / ContainInOrder / ContainInConsecutiveOrder on the collection
   assertion) when order is part of the contract.

8. TypeEnumerableExtensions (ThatAreClasses, ThatImplement<T> ...) return
   IEnumerable<Type>, whose ".Should()" is a collection assertion. To reach
   BeSealed / BeInNamespace / BeDecoratedWith you need a TypeSelector - start
   from "AllTypes.From(assembly)" or "new TypeSelector(types)".

9. Reflection assertions need full signatures. HaveIndexer takes the indexer
   type AND the parameter types
   ("HaveIndexer(typeof(string), new[] { typeof(int) })"), and HaveExplicitMethod
   takes the interface, the name AND the parameter types. Conversion-operator
   assertions are generic over the two types, so a ref struct such as
   ReadOnlySpan<char> cannot be used as a type argument - use the
   "(Type sourceType, Type targetType)" overload for exotic types.

10. There is no ReturnVoidOrTaskVoid on MethodInfoAssertions. Use ReturnVoid(),
    Return<TReturn>() or BeAsync().

11. BeBinarySerializable always fails: BinaryFormatter is gone from modern
    .NET, so the check reports "serialization failed". Use
    BeDataContractSerializable or BeXmlSerializable instead.

12. Do not call ".Should()" on something that is already an assertion object -
    "value.Should().Should()" or "....And.Should()". AssertionExtensions carries
    void guard overloads marked [Obsolete(..., error: true)] for exactly that
    mistake, so it is a compile error with the message "You are asserting the
    'AndConstraint' itself".

13. Reflection assertions run against runtime metadata. [Serializable] is a
    metadata flag rather than a stored custom attribute, so
    "BeDecoratedWith<SerializableAttribute>()" fails even for types that carry
    it in source. Assert with attributes you control.

14. An "async void" method cannot be asserted as an Action. The delegate
    assertions detect the compiler-generated async state machine and throw
    InvalidOperationException ("Cannot use action assertions on an async void
    method..."). Assign it to a Func<Task> and use the async assertions.

15. ThrowExactly<T> does not unwrap AggregateException, while Throw<T> does. If
    a Task-based API wraps its failure, ThrowExactly will report
    AggregateException.

16. AssertionOptions and Formatter changes are global and persist for the whole
    test run. Reset them in teardown
    ("AssertionOptions.AssertEquivalencyUsing(_ => new
    EquivalencyAssertionOptions())", "Formatter.RemoveFormatter(instance)") or
    tests will influence each other, especially under parallel execution.

17. Mark helper methods that wrap assertions with [CustomAssertion]; otherwise
    the caller-identification logic names your helper's local variable in the
    failure message instead of the subject under test.

18. Dispose an event monitor (prefer "using var monitor = x.Monitor();"). It
    subscribes to every event on the subject until disposed.

================================================================================

WHAT THIS PACKAGE DOES NOT DO
=============================
  - It does not discover, run or parallelise tests. That is the test
    framework's job; SilverAssertions only throws the failure exception.
  - It is not a mocking or stubbing library (use Moq, NSubstitute, FakeItEasy).
  - It does not do binary serialization round-trips: BeBinarySerializable is
    present for source compatibility but always fails, because BinaryFormatter
    is not available on modern .NET.
  - It is not a benchmarking tool. ExecutionTime measures one run with a
    stopwatch; for statistically meaningful numbers use BenchmarkDotNet.
  - It does not do snapshot/approval testing, property-based testing,
    code-coverage measurement, UI or browser automation, or HTTP/service
    hosting for integration tests.
  - It does not assert on HttpResponseMessage content - only on status classes
    and codes. Read the content yourself and assert on the string or object.
  - It targets .NET 10 and later only; there is no asset for .NET Framework,
    .NET Standard or earlier .NET versions.

================================================================================

WORKING EXAMPLES ON GITHUB
==========================
Every assertion in this library has an executable example in the test suite. If
this document is not enough, read the test file for the feature. Base URL:

    https://github.com/ellisnet/SilverAssertions/tree/main/

Raw file content (for fetching a single file):

    https://raw.githubusercontent.com/ellisnet/SilverAssertions/main/<path>

Feature-to-test-file map (paths relative to the base URL above):

  Strings (Be, Contain, StartWith, EndWith, Match, MatchRegex, HaveLength,
  BeUpperCased, BeLowerCased, BeOneOf, ...)
    -> tests/SilverAssertions.Tests/Primitives/StringAssertionTests.*.cs
       (one partial-class file per method group)
    -> tests/SilverAssertions.Tests/Primitives/StringComparisonTests.cs

  Numerics, nullable numerics, comparables
    -> tests/SilverAssertions.Tests/Numeric/NumericAssertionTests.cs
    -> tests/SilverAssertions.Tests/Numeric/NullableNumericAssertionTests.cs
    -> tests/SilverAssertions.Tests/Numeric/NumericDifferenceAssertionsTests.cs
    -> tests/SilverAssertions.Tests/Numeric/ComparableTests.cs

  Booleans
    -> tests/SilverAssertions.Tests/Primitives/BooleanAssertionTests.cs
    -> tests/SilverAssertions.Tests/Primitives/NullableBooleanAssertionTests.cs

  DateTime, DateTimeOffset, DateOnly, TimeOnly, TimeSpan
    -> tests/SilverAssertions.Tests/Primitives/DateTimeAssertionTests.cs
    -> tests/SilverAssertions.Tests/Primitives/DateTimeOffsetAssertionTests.cs
    -> tests/SilverAssertions.Tests/Primitives/DateOnlyAssertionTests.cs
    -> tests/SilverAssertions.Tests/Primitives/TimeOnlyAssertionTests.cs
    -> tests/SilverAssertions.Tests/Primitives/SimpleTimeSpanAssertionTests.cs
    -> tests/SilverAssertions.Tests/Primitives/
       NullableSimpleTimeSpanAssertionTests.cs

  Fluent date/time construction (5.Seconds(), 4.July(2026), At, Before, After)
    -> tests/SilverAssertions.Tests/Extensions/FluentDateTimeTests.cs
    -> tests/SilverAssertions.Tests/Extensions/
       TimeSpanConversionExtensionTests.cs

  Guids and enums
    -> tests/SilverAssertions.Tests/Primitives/GuidAssertionTests.cs
    -> tests/SilverAssertions.Tests/Primitives/NullableGuidAssertionTests.cs
    -> tests/SilverAssertions.Tests/Primitives/EnumAssertionTests.cs

  Objects and reference types (BeOfType, BeAssignableTo, BeSameAs, Match, As<T>)
    -> tests/SilverAssertions.Tests/Primitives/ObjectAssertionTests.cs
    -> tests/SilverAssertions.Tests/Primitives/ReferenceTypeAssertionsTests.cs
    -> tests/SilverAssertions.Tests/Extensions/ObjectCastingTests.cs
    -> tests/SilverAssertions.Tests/Extensions/ObjectExtensionsTests.cs

  Collections (one partial-class file per method group)
    -> tests/SilverAssertions.Tests/Collections/CollectionAssertionTests.*.cs
    -> tests/SilverAssertions.Tests/Collections/
       GenericCollectionAssertionOfStringTests.cs
    -> tests/SilverAssertions.Tests/Collections/
       GenericDictionaryAssertionTests.cs

  Occurrence constraints and .Which chaining
    -> tests/SilverAssertions.Tests/OccurrenceConstraintTests.cs
    -> tests/SilverAssertions.Tests/AndWhichConstraintTests.cs

  Exceptions and delegates
    -> tests/SilverAssertions.Tests/Exceptions/ExceptionAssertionTests.cs
    -> tests/SilverAssertions.Tests/Exceptions/ThrowAssertionsTests.cs
    -> tests/SilverAssertions.Tests/Exceptions/NotThrowTests.cs
    -> tests/SilverAssertions.Tests/Exceptions/InnerExceptionTests.cs
    -> tests/SilverAssertions.Tests/Exceptions/OuterExceptionTests.cs
    -> tests/SilverAssertions.Tests/Exceptions/
       FunctionExceptionAssertionTests.cs
    -> tests/SilverAssertions.Tests/Exceptions/
       AsyncFunctionExceptionAssertionTests.cs
    -> tests/SilverAssertions.Tests/Exceptions/InvokingActionTests.cs
    -> tests/SilverAssertions.Tests/Exceptions/InvokingFunctionTests.cs
    -> tests/SilverAssertions.Tests/Specialized/DelegateAssertionTests.cs
    -> tests/SilverAssertions.Tests/Specialized/
       AggregateExceptionAssertionTests.cs
    -> tests/SilverAssertions.Tests/Extensions/FluentActionsTests.cs

  Tasks, async and TaskCompletionSource
    -> tests/SilverAssertions.Tests/Specialized/TaskAssertionTests.cs
    -> tests/SilverAssertions.Tests/Specialized/TaskOfTAssertionTests.cs
    -> tests/SilverAssertions.Tests/Specialized/
       TaskCompletionSourceAssertionTests.cs

  Execution time
    -> tests/SilverAssertions.Tests/Specialized/ExecutionTimeAssertionsTests.cs

  Object graph equivalency and its options
    -> tests/SilverAssertions.Equivalency.Tests/BasicTests.cs
    -> tests/SilverAssertions.Equivalency.Tests/CollectionTests.cs
    -> tests/SilverAssertions.Equivalency.Tests/NestedPropertiesTests.cs
    -> tests/SilverAssertions.Equivalency.Tests/CyclicReferencesTests.cs
    -> tests/SilverAssertions.Equivalency.Tests/DictionaryTests.cs
    -> tests/SilverAssertions.Equivalency.Tests/RecordTests.cs
    -> tests/SilverAssertions.Equivalency.Tests/TupleTests.cs
    -> tests/SilverAssertions.Equivalency.Tests/EnumTests.cs
    -> tests/SilverAssertions.Equivalency.Tests/SelectionRulesTests.cs
    -> tests/SilverAssertions.Equivalency.Tests/MemberMatchingTests.cs
    -> tests/SilverAssertions.Equivalency.Tests/MemberConversionTests.cs
    -> tests/SilverAssertions.Equivalency.Tests/MemberLessObjectsTests.cs
    -> tests/SilverAssertions.Equivalency.Tests/ObjectReferenceTests.cs
    -> tests/SilverAssertions.Equivalency.Tests/NonEquivalencyTests.cs
    -> tests/SilverAssertions.Equivalency.Tests/AssertionRuleTests.cs
    -> tests/SilverAssertions.Equivalency.Tests/DateTimePropertiesTests.cs
    -> tests/SilverAssertions.Tests/AssertionOptionsTests.cs

  System.Data equivalency and assertions
    -> tests/SilverAssertions.Equivalency.Tests/DataSetTests.cs
    -> tests/SilverAssertions.Equivalency.Tests/DataTableTests.cs
    -> tests/SilverAssertions.Equivalency.Tests/DataRowTests.cs
    -> tests/SilverAssertions.Equivalency.Tests/DataColumnTests.cs
    -> tests/SilverAssertions.Equivalency.Tests/DataRelationTests.cs
    -> tests/SilverAssertions.Equivalency.Tests/TypedDataSetTests.cs
    -> tests/SilverAssertions.Equivalency.Tests/TypedDataTableTests.cs
    -> tests/SilverAssertions.Tests/Collections/Data/
       DataTableCollectionAssertionTests.cs
    -> tests/SilverAssertions.Tests/Collections/Data/
       DataRowCollectionAssertionTests.cs
    -> tests/SilverAssertions.Tests/Collections/Data/
       DataColumnCollectionAssertionTests.cs

  Event monitoring
    -> tests/SilverAssertions.Tests/Events/EventAssertionTests.cs

  Streams
    -> tests/SilverAssertions.Tests/Streams/StreamAssertionTests.cs
    -> tests/SilverAssertions.Tests/Streams/BufferedStreamAssertionTests.cs

  XML (LINQ-to-XML and System.Xml)
    -> tests/SilverAssertions.Tests/Xml/XDocumentAssertionTests.cs
    -> tests/SilverAssertions.Tests/Xml/XElementAssertionTests.cs
    -> tests/SilverAssertions.Tests/Xml/XAttributeAssertionTests.cs
    -> tests/SilverAssertions.Tests/Xml/XmlNodeAssertionTests.cs
    -> tests/SilverAssertions.Tests/Xml/XmlElementAssertionTests.cs
    -> tests/SilverAssertions.Equivalency.Tests/XmlTests.cs

  HttpResponseMessage
    -> tests/SilverAssertions.Tests/Primitives/
       HttpResponseMessageAssertionTests.cs

  Types, members and selectors
    -> tests/SilverAssertions.Tests/Types/TypeAssertionTests.*.cs
    -> tests/SilverAssertions.Tests/Types/MethodInfoAssertionTests.cs
    -> tests/SilverAssertions.Tests/Types/MethodBaseAssertionTests.cs
    -> tests/SilverAssertions.Tests/Types/PropertyInfoAssertionTests.cs
    -> tests/SilverAssertions.Tests/Types/TypeSelectorTests.cs
    -> tests/SilverAssertions.Tests/Types/TypeSelectorAssertionTests.cs
    -> tests/SilverAssertions.Tests/Types/MethodInfoSelectorTests.cs
    -> tests/SilverAssertions.Tests/Types/MethodInfoSelectorAssertionTests.cs
    -> tests/SilverAssertions.Tests/Types/PropertyInfoSelectorTests.cs
    -> tests/SilverAssertions.Tests/Types/PropertyInfoSelectorAssertionTests.cs
    -> tests/SilverAssertions.Tests/TypeEnumerableExtensionsTests.cs
    -> tests/SilverAssertions.Tests/Specialized/AssemblyAssertionTests.cs

  AssertionScope, Execute.Assertion, Given/FailWith chains
    -> tests/SilverAssertions.Tests/Execution/AssertionScopeTests.cs
    -> tests/SilverAssertions.Tests/Execution/AssertionScope.ChainingApiTests.cs
    -> tests/SilverAssertions.Tests/Execution/AssertionScope.ContextDataTests.cs
    -> tests/SilverAssertions.Tests/Execution/
       AssertionScope.MessageFormatingTests.cs
    -> tests/SilverAssertions.Tests/Execution/GivenSelectorTests.cs
    -> tests/SilverAssertions.Tests/Execution/CallerIdentifierTests.cs
    -> tests/SilverAssertions.Tests/AssertionFailureTests.cs

  Extensibility (custom assertions, custom equivalency steps)
    -> tests/SilverAssertions.Tests/ExtensibilityTests.cs
    -> tests/SilverAssertions.Equivalency.Tests/ExtensibilityTests.cs

  Formatting (Formatter, IValueFormatter, [ValueFormatter], FormattingOptions)
    -> tests/SilverAssertions.Tests/Formatting/FormatterTests.cs
    -> tests/SilverAssertions.Tests/Formatting/
       MultidimensionalArrayFormatterTests.cs
    -> tests/SilverAssertions.Tests/Formatting/
       PredicateLambdaExpressionValueFormatterTests.cs
    -> tests/SilverAssertions.Tests/Formatting/TimeSpanFormatterTests.cs
    -> tests/SilverAssertions.Tests/Formatting/
       DateTimeOffsetValueFormatterTests.cs

  Configuration and test-framework detection
    -> tests/SilverAssertions.Tests/ConfigurationTests.cs
    -> tests/SilverAssertions.Tests/Execution/TestFrameworkProviderTests.cs
    -> tests/SilverAssertions.Tests/Execution/FallbackTestFrameworkTests.cs

  Per-framework smoke tests (xUnit v3, NUnit v4, MSTest v4, MSpec)
    -> tests/TestFrameworks/XUnit3.Tests/FrameworkTests.cs
    -> tests/TestFrameworks/NUnit4.Tests/FrameworkTests.cs
    -> tests/TestFrameworks/MSTestV4.Tests/FrameworkTests.cs
    -> tests/TestFrameworks/MSpec.Tests/FrameworkTests.cs

================================================================================

QUICK REFERENCE CARD
====================

Install:     dotnet add package SilverAssertions.ApacheLicenseForever
Using:       using SilverAssertions;
Target:      .NET 10 or later
License:     Apache-2.0
Pattern:     value.Should().BeXxx();
Chain:       value.Should().BeXxx().And.BeYyy();
Drill in:    coll.Should().ContainSingle(p).Which.Member.Should().Be(x);
Because:     value.Should().BeTrue("the user {0} is active", name);

--- Strings (StringAssertions) ---
Be                  "abc".Should().Be("abc")
BeEquivalentTo      "ABC".Should().BeEquivalentTo("abc")    // case-insensitive
StartWith/EndWith   "abc".Should().StartWith("a")
Contain             "abc".Should().Contain("b")
Contain + count     "aaa".Should().Contain("a", Exactly.Times(3))
Match / MatchRegex  "abc".Should().Match("a*c")
HaveLength          "abc".Should().HaveLength(3)
BeEmpty             "".Should().BeEmpty()
BeUpperCased        "ABC".Should().BeUpperCased()

--- Numbers (NumericAssertions<T>) ---
Be                  42.Should().Be(42)
BeGreaterThan       42.Should().BeGreaterThan(0)
BeInRange           42.Should().BeInRange(1, 100)
BePositive          42.Should().BePositive()
BeCloseTo (int)     42.Should().BeCloseTo(45, 5u)           // uint delta
BeApproximately     3.14.Should().BeApproximately(3.1, 0.1) // float/double/dec

--- Booleans / Guid / Enum ---
BeTrue/BeFalse      flag.Should().BeTrue()
BeEmpty             Guid.Empty.Should().BeEmpty()
HaveFlag            access.Should().HaveFlag(Access.Read)
BeDefined           value.Should().BeDefined()

--- Collections (GenericCollectionAssertions<T>) ---
HaveCount           list.Should().HaveCount(5)
Contain             list.Should().Contain(3)
ContainSingle       list.Should().ContainSingle(x => x.Id == 1).Which...
BeInAscendingOrder  list.Should().BeInAscendingOrder(x => x.Name)
Equal               list.Should().Equal(1, 2, 3)            // order matters
BeEquivalentTo      list.Should().BeEquivalentTo(other)     // order-free
OnlyContain         list.Should().OnlyContain(x => x > 0)
AllSatisfy          list.Should().AllSatisfy(x => x.Should().BePositive())

--- Dictionaries (GenericDictionaryAssertions) ---
ContainKey          map.Should().ContainKey("k").WhoseValue.Should().Be(1)
ContainValue        map.Should().ContainValue(1)
Contain             map.Should().Contain("k", 1)

--- Objects / graphs ---
BeNull/NotBeNull    obj.Should().NotBeNull()
BeOfType            obj.Should().BeOfType<Foo>().Which...
BeEquivalentTo      obj.Should().BeEquivalentTo(expected)
  with options      obj.Should().BeEquivalentTo(exp, o => o.Excluding(x => x.Id))

--- Exceptions (ExceptionAssertions<T>) ---
Throw               act.Should().Throw<Exception>()
ThrowExactly        act.Should().ThrowExactly<Exception>()  // no unwrapping
NotThrow            act.Should().NotThrow()
WithMessage         .WithMessage("*error*")
WithParameterName   .WithParameterName("id")
WithInnerException  .WithInnerException<IOException>()
Invoking            svc.Invoking(s => s.Do()).Should().Throw<T>()

--- Async ---
ThrowAsync          await act.Should().ThrowAsync<Exception>()
NotThrowAsync       await act.Should().NotThrowAsync()
CompleteWithinAsync await act.Should().CompleteWithinAsync(timeSpan)
WithResult          await f.Should().CompleteWithinAsync(t).WithResult(42)
Awaiting            await svc.Awaiting(s => s.DoAsync()).Should().ThrowAsync<T>()

--- Dates and times ---
BeAfter/BeBefore    dt.Should().BeAfter(other)
BeCloseTo           dt.Should().BeCloseTo(other, TimeSpan.FromSeconds(1))
HaveYear            dt.Should().HaveYear(2026)
Range               dt.Should().BeMoreThan(timeSpan).After(other)

--- Types (TypeAssertions / TypeSelector) ---
BeSealed            typeof(T).Should().BeSealed()
Implement           typeof(T).Should().Implement<IDisposable>()
HaveProperty        typeof(T).Should().HaveProperty<int>("Size")
HaveMethod          typeof(T).Should().HaveMethod("M", new[] { typeof(int) })
Selector            AllTypes.From(asm).ThatAreClasses().Should().BeSealed()

--- Events / streams / xml / http ---
Monitor             using var m = subject.Monitor();
Raise               m.Should().Raise("PropertyChanged").WithSender(subject)
Stream              stream.Should().BeReadable().And.HaveLength(3)
XDocument           doc.Should().HaveRoot("root").Which...
Http                response.Should().BeSuccessful()

--- Scope and extensibility ---
AssertionScope      using (new AssertionScope()) { ... }   // .Execution
Custom assertion    Execute.Assertion.ForCondition(c).BecauseOf(b, a)
                        .FailWith("...{reason}...", value)
Mark helpers        [CustomAssertion]
Custom formatter    Formatter.AddFormatter(new MyFormatter())
Global equivalency  AssertionOptions.AssertEquivalencyUsing(o => o...)

Test frameworks: xunit3, xunit2, nunit, mstestv4, mstestv3, mstestv2, mspec
Override with app setting "SilverAssertions.TestFramework".

================================================================================
