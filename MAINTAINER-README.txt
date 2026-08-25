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
    SilverAssertions.slnx     Solution; also carries the Solution Items folder.

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
        TestFrameworks/XUnit4.Tests/        framework (and per xunit.v3 package
        TestFrameworks/NUnit4.Tests/        line), proving the right assertion
        TestFrameworks/MSTestV4.Tests/      exception type is thrown.
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
Directory.Build.props and no build script. There IS a global.json at the
repository root, and it is load-bearing - see TESTING below.

    dotnet restore SilverAssertions.slnx
    dotnet build SilverAssertions.slnx -c Release

IMPORTANT: the library project sets GeneratePackageOnBuild=true, so EVERY build
of src/SilverAssertions produces a .nupkg in its bin folder, with a version
derived from the clock at build time. That is expected; see PACKAGING.

TESTING
=======
Run BOTH of these from the repository root. Together they cover every test
project in the solution:

    dotnet build SilverAssertions.slnx && dotnet test --test-modules "**/bin/Debug/net10.0/*.Tests.dll"

    cd tests/TestFrameworks/MSpec.Tests && dotnet test

"dotnet test SilverAssertions.slnx" does NOT work; it fails before running a
single test. Why it takes two commands:

  - global.json at the repository root selects the Microsoft Testing Platform
    (MTP) runner. That is required, not optional: the xUnit v3 4.x projects
    cannot run under VSTest on the .NET 10 SDK at all.
  - The runner is chosen ONCE per "dotnet test" invocation and applies to every
    project in it. A solution-wide run therefore requires that EVERY project be
    MTP-capable.
  - Machine.Specifications has no MTP runner, so MSpec.Tests cannot be, and a
    solution-wide run is rejected outright.
  - MSpec.Tests therefore carries its own global.json pinning it to VSTest.
    global.json is resolved from the CURRENT DIRECTORY, not per project, so
    that file only takes effect when dotnet test is invoked from inside that
    folder - hence the "cd". Do not delete it.

The first command uses --test-modules (a glob over already-built assemblies)
instead of --solution, because that is the only form that runs several projects
in one invocation without triggering the solution-wide runner check. Two
consequences: it does not build anything, hence the "dotnet build &&" in front;
and it rejects -c/-f/--arch/--os/--runtime, so to test Release you build with
-c Release and change Debug to Release inside the glob.

IMPORTANT - THE FIRST COMMAND ALWAYS EXITS NON-ZERO, EVEN WHEN EVERY TEST
PASSES. The glob also matches MSpec.Tests.dll, which is not an MTP test app.
MTP launches it anyway, gets nothing back, and records:

    MSpec.Tests.dll Zero tests ran

That surfaces as "error: 1" in the summary and sets the exit code to 1. So a
completely green run looks like this:

    error: 1
    total: <n>
    failed: 0
    succeeded: <n>

Judge the run by the "failed:" count, NOT by the exit code. A run with genuine
test failures also exits non-zero, so the exit code cannot distinguish the two.

This is why neither command is currently wired into CI as a pass/fail gate.
Fixing it properly means keeping MSpec.Tests.dll out of the glob's reach - for
example by giving that project a different output path - rather than loosening
what the gate accepts.

MSpec's own two tests are covered by the second command, which exits normally.

No environment variables, no opt-in switches, no external services, no special
prep. The suites are pure in-process unit tests.

DEBUG VS RELEASE. Both configurations pass, but four tests are SKIPPED in an
optimized build:

    CallerIdentifierTests.When_namespace_is_prefixed_with_System_caller_should_be_known
    CallerIdentifierTests.When_there_are_several_statements_on_the_line_it_should_use_the_correct_statement
    CallerIdentifierTests.All_core_code_anywhere_in_the_stack_trace_is_ignored
    TaskOfTAssertionTests+CompleteWithinAsync.When_task_completes_and_async_result_is_not_expected_it_should_fail

CallerIdentifier recovers the name of the variable under assertion by walking
the stack to the caller's frame and reading that source line. With optimization
on, the JIT can inline the caller away; the frame is gone and the name degrades
to a generic noun - "Expected object to be <null>" instead of "Expected foo2 to
be <null>". The assertion still fails on the right thing; only the wording
changes. This is inherent to the technique and cannot be fixed from inside the
library, because the inlined frame belongs to the CONSUMER's assembly.

The four tests above assert on that recovered name, so they guard with
Assert.SkipWhen(JitOptimization.IsEnabled, ...) - see
tests/SilverAssertions.Tests/JitOptimization.cs. They run normally in Debug and
skip in Release, so BOTH configurations report zero failures and a real
regression cannot hide among expected ones.

    Debug:    6391 total, 0 failed, 0 skipped
    Release:  6391 total, 0 failed, 4 skipped

JitOptimization keys off DebuggableAttribute.IsJITOptimizerDisabled rather than
"#if DEBUG", so it follows the optimization setting itself: a Release build with
-p:Optimize=false runs all four. That is also how to verify the cause is
inlining and not missing PDBs - symbols are present in Release either way.

Notes:
  - SilverAssertions.Tests and SilverAssertions.Equivalency.Tests use xUnit v3
    with xunit.runner.visualstudio, and Microsoft.NET.Test.Sdk.
  - SilverAssertions.Tests additionally references System.Data.DataSetExtensions
    (typed DataSet specs), TestAssemblyA, TestAssemblyB and the vendored
    DennisDoomen.ChillBdd project (the GivenSubject/TestFor base classes used by
    AssertionOptionsTests).
  - The five projects under tests/TestFrameworks exist so that the adapters in
    src/SilverAssertions/Execution really are exercised. Keep all five
    building; a broken one means the corresponding adapter is untested.
  - XUnit3.Tests and XUnit4.Tests are NOT duplicates. Both drive the same
    XUnit3TestFramework adapter, which binds by assembly name
    (xunit.v3.assert), but they pin different package lines: XUnit3.Tests
    stays on xunit.v3 3.x, XUnit4.Tests tracks the current 4.x. Together they
    prove the adapter works against both. Each csproj carries a comment saying
    so; do not "consolidate" them, and do not bump XUnit3.Tests during a
    package sweep.
  - Note that xunit.v3 4.0.0 is not a typo. xUnit.net moved the product
    generation into the package NAME so the package VERSION could follow
    SemVer independently, so "xunit.v3 4.0.0" means version 4.0.0 of the
    xUnit.net v3 line. A future generation would be a package named xunit.v4.
  - Their runners differ, and the differences are deliberate. XUnit3.Tests and
    XUnit4.Tests are MTP-capable out of the box. MSTestV4.Tests sets
    EnableMSTestRunner, and NUnit4.Tests sets EnableNUnitRunner; both also need
    OutputType=Exe, because an MTP test project hosts its own runner and must
    be an executable. MSpec.Tests stays on VSTest - see TESTING.
  - Do not put an IncludeAssets filter on NUnit4.Tests' NUnit3TestAdapter
    reference. The usual "runtime; build; native; contentfiles; analyzers;
    buildtransitive" boilerplate omits "compile", and the MTP runner's
    generated entry point must compile against Microsoft.Testing.Platform,
    which that adapter supplies transitively. With the filter in place the
    project fails to build with CS0234.
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
