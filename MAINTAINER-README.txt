================================================================================
MAINTAINER-README: SilverAssertions
Notes for people and agents MAINTAINING this repository — not for package
consumers
================================================================================

If you are consuming the NuGet package, read AGENT-README.txt instead. This file
is about building, testing, packaging and maintaining the repository itself.

PURPOSE AND SCOPE
=================
The repository produces exactly one NuGet package:

    PackageId : SilverAssertions.ApacheLicenseForever
    Project   : src/SilverAssertions/SilverAssertions.csproj
    Assembly  : SilverAssertions.dll  (root namespace SilverAssertions)
    License   : Apache-2.0
    Consumer documentation: AGENT-README.txt (repo root; shipped in the nupkg)

Everything else in the repository is test code, vendored library source, or
documentation.

REPOSITORY LAYOUT
=================
    AGENT-README.txt          Consumer guide for the package (packed).
    MAINTAINER-README.txt     This file.
    EXTRAS-README.txt         Non-package content in the repository.
    README-INDEX.txt          Map of the README files.
    README.md                 Human-facing overview (GitHub and nuget.org).
    LICENSE                   Apache License 2.0.
    THIRD-PARTY-NOTICES.txt   Notices for FluentAssertions, Chill, Reflectify.
    icon-codebrix-128.png     Package icon.
    SilverAssertions.sln      Solution; also carries the Solution Items folder.

    src/SilverAssertions/     The library. Sub-folders map to namespaces:
        CallerIdentification/   Caller-name detection for "{context}".
        Collections/            GenericCollectionAssertions and friends,
                                MaximumMatching/ for the Satisfy algorithm.
        Common/                 Configuration, Services, clocks, guards,
                                CSharpAccessModifier.
        Data/                   System.Data assertions and options.
        Equivalency/            The BeEquivalentTo engine: Execution/,
                                Matching/, Ordering/, Selection/, Steps/,
                                Tracing/.
        Events/                 Event monitoring (EventMonitor, recorders).
        Execution/              AssertionScope, Execute, test-framework
                                adapters.
        Extensions/             Fluent date/time and occurrence-constraint
                                extensions.
        Formatting/             Formatter and the built-in value formatters.
        Numeric/                Numeric and comparable assertions.
        Primitives/             String, boolean, date/time, object, HTTP.
        Specialized/            Delegates, exceptions, tasks, execution time,
                                assemblies.
        Streams/                Stream assertions.
        Types/                  Type/member assertions and the selectors.
        Xml/                    LINQ-to-XML and System.Xml assertions,
                                Equivalency/ for the XML equivalency steps.
        InternalsVisibleTo.cs   Grants internals to the two test assemblies and
                                to DennisDoomen.Reflectify.Tests.

    tests/
        SilverAssertions.Tests/             Main suite (xUnit v3).
        SilverAssertions.Equivalency.Tests/ BeEquivalentTo suite (xUnit v3).
        TestAssemblyA/, TestAssemblyB/      Fixture assemblies used by the
                                            assembly and type-selector specs.
        TestFrameworks/XUnit3.Tests/        Smoke tests, one per supported
        TestFrameworks/NUnit4.Tests/        framework, proving the right
        TestFrameworks/MSTestV4.Tests/      assertion exception type is thrown.
        TestFrameworks/MSpec.Tests/

    libs/
        Reflectify/DennisDoomen.Reflectify/       Vendored source, COMPILED INTO
                                                  the library (see PROVENANCE).
        Reflectify/DennisDoomen.Reflectify.Tests/ Its own test project.
        ChillBdd/DennisDoomen.ChillBdd/           Vendored BDD test helper.
        ChillBdd/DennisDoomen.ChillBdd.Tests/     Its own test project.

BUILDING
========
Everything targets net10.0; a .NET 10 SDK is the only prerequisite. There is no
Directory.Build.props, no global.json and no build script - the solution is
self-contained.

    dotnet restore SilverAssertions.sln
    dotnet build SilverAssertions.sln -c Release

IMPORTANT: the library project sets GeneratePackageOnBuild=true, so EVERY build
of src/SilverAssertions produces a .nupkg in its bin folder, with a version
derived from the clock at build time. That is expected; see PACKAGING.

TESTING
=======
    dotnet test SilverAssertions.sln

No environment variables, no opt-in switches, no external services, no special
prep. The suites are pure in-process unit tests.

Notes:
  - SilverAssertions.Tests and SilverAssertions.Equivalency.Tests use xUnit v3
    with xunit.runner.visualstudio, Microsoft.NET.Test.Sdk and
    coverlet.collector.
  - SilverAssertions.Tests additionally references System.Data.DataSetExtensions
    (typed DataSet specs), TestAssemblyA, TestAssemblyB and the vendored
    DennisDoomen.ChillBdd project (the GivenSubject/TestFor base classes used by
    AssertionOptionsTests).
  - The four projects under tests/TestFrameworks each reference a different test
    framework so that the adapters in src/SilverAssertions/Execution really are
    exercised. Keep all four building; a broken one means the corresponding
    adapter is untested.
  - Test-class and file naming follows the upstream shape: one file per method
    group, e.g. StringAssertionTests.Contain.cs, all declaring
    "public partial class StringAssertionTests". Test method names are
    snake_case sentences ("When_all_types_are_sealed_it_succeeds") with
    // Arrange / // Act / // Assert comments in the body.
  - Internals are visible to SilverAssertions.Tests,
    SilverAssertions.Equivalency.Tests and DennisDoomen.Reflectify.Tests through
    src/SilverAssertions/InternalsVisibleTo.cs. Add a new test project there if
    it needs internals.

PACKAGING AND PUBLISHING
========================
Packing is done by the SDK on build (GeneratePackageOnBuild=true); there is no
separate pack driver:

    dotnet build src/SilverAssertions/SilverAssertions.csproj -c Release

Versioning is date-stamped and auto-incrementing, computed in the csproj from
System.DateTime.UtcNow:

    1.<x>.<y>.<z>
      1  major     always 1 for this library
      x  minor     whole years since _VersionBaseYear (2026 = 0)
      y  build     day of year, 1-based, UTC
      z  revision  minute of day, UTC, 0..1439

Consequences to keep in mind:
  - The value always increases over time, but it is NOT SemVer: major/minor say
    nothing about API compatibility.
  - Every build produces a new version, so the bin folder accumulates .nupkg
    files.
  - Two builds within the same UTC minute produce the SAME version - never
    publish two packages from within one minute.
  - Re-baseline by changing _VersionBaseYear in the csproj.

What ships in the nupkg, besides the assembly (all four are <None> items with
Pack="true" in the csproj):

    README.md                 (PackageReadmeFile)
    icon-codebrix-128.png     (PackageIcon)
    AGENT-README.txt          consumer documentation
    THIRD-PARTY-NOTICES.txt   required attribution

Package metadata also sets PackageLicenseExpression=Apache-2.0,
PackageRequireLicenseAcceptance=true, the ellisnet/SilverAssertions project and
repository URLs, and the tags "assertions;unit-testing;testing;fluent;tdd;bdd;
CodeBrix".

Runtime NuGet dependencies are JetBrains.Annotations and
System.Configuration.ConfigurationManager. Adding a dependency changes the
consumer contract - update INSTALLATION in AGENT-README.txt when you do.

Tagging and publishing to nuget.org are done by the repository owner. Do not run
git state-changing commands as part of maintenance work here.

PROVENANCE AND VENDORED SOURCES
===============================
1. FluentAssertions 7.1.0 (Apache-2.0), branch support-7.0, retrieved
   2025-01-27. The bulk of src/SilverAssertions is derived from it: copied,
   modified and adapted. Every namespace root was renamed from
   "FluentAssertions" to "SilverAssertions". Do not reintroduce the upstream
   namespace, and do not copy code from any FluentAssertions version whose
   license is newer than Apache-2.0 - that is the licensing premise this fork
   depends on, and it is stated in THIRD-PARTY-NOTICES.txt.

2. Reflectify (MIT, Dennis Doomen), libs/Reflectify. This is a source-only
   library. The library project does NOT reference it as a project or package;
   it compiles the single file directly:

       <Compile Include="..\..\libs\Reflectify\DennisDoomen.Reflectify\
                         Reflectify.cs"
                Link="Reflectify\Reflectify.cs" />

   Its types are internal, so they do not widen the public surface. The separate
   DennisDoomen.Reflectify project exists so the file can be compiled and tested
   on its own.

3. Chill (MIT), libs/ChillBdd, retrieved 2025-02-08. A BDD test-helper library
   used only by SilverAssertions.Tests. It never ships in the package.

When you edit vendored source, keep the upstream file structure recognisable and
leave the notices in THIRD-PARTY-NOTICES.txt in step with what is actually
vendored.

CODING CONVENTIONS
==================
These are what the code base actually does; follow them rather than reformatting
inherited code:

  - File-scoped namespaces ("namespace SilverAssertions.Primitives;").
  - Nullable reference types are NOT enabled; do not add "?" annotations to
    reference types or turn <Nullable> on.
  - Public types and members carry XML doc comments, including the boilerplate
    <param name="because"> / <param name="becauseArgs"> blocks. Keep them when
    adding an assertion.
  - Assertion methods end with "string because = "", params object[]
    becauseArgs" and return AndConstraint<TAssertions> (or
    AndWhichConstraint<...> when there is a matched element to drill into).
  - Assertion classes come in a self-typed generic pair - "class XAssertions :
    XAssertions<XAssertions>" plus "class XAssertions<TAssertions>" - so
    derived assertion classes keep their own return type. Preserve the pattern
    when adding one.
  - Failure logic goes through Execute.Assertion with ForCondition/BecauseOf/
    FailWith (or Given(...) when a later condition would dereference a possibly
    null subject).
  - #region blocks group related member pairs (BeXxx / NotBeXxx); keep pairs
    together.
  - Public API methods that a user could call in a chain should be marked
    [CustomAssertion] only when they wrap other assertions and would otherwise
    confuse caller identification.
  - Existing "// SMELL:" and "// TODO:" comments inherited from upstream are
    left in place deliberately - do not tidy them away.

NOTES
=====
  - ObjectAssertionsExtensions.BeBinarySerializable is intentionally
    non-functional: BinaryFormatter is gone from modern .NET, so the method
    throws NotImplementedException internally and reports an assertion failure.
    The commented-out original implementation is kept next to it. Do not
    "fix" it by silently passing; if it is ever re-implemented, update
    AGENT-README.txt (both the pitfall and the "does not do" entry).
  - The MSTest v3 and v4 adapters share an assembly name and exception type, so
    detection may pick v3 where v4 is loaded. That is documented in the source
    and has no practical effect.
  - The solution's "Solution Items" folder lists .gitignore, AGENT-README.txt,
    icon-codebrix-128.png, LICENSE, README.md and THIRD-PARTY-NOTICES.txt. If
    you want the new README files visible in the IDE, they would go there too.
  - The eight AI-agent pointer files (AGENTS.md, CLAUDE.md, .clinerules,
    .cursorrules, .cursor/rules/agent-readme.mdc, .windsurfrules,
    .github/copilot-instructions.md, .junie/guidelines.md) are stubs that point
    at README-INDEX.txt. They are maintained centrally across the CodeBrix
    family - do not hand-edit them here.

================================================================================
