================================================================================
EXTRAS-README: SilverAssertions
Samples, tools and other content in this repository that is not part of a NuGet
package
================================================================================

This repository ships no sample applications, no demo projects, no tools and no
optional test-data sets. Apart from the library in src/SilverAssertions - which
is the NuGet package and is documented in AGENT-README.txt - everything here is
either a test project or vendored library source.

The test projects double as the worked examples for the package: the
"WORKING EXAMPLES ON GITHUB" section of AGENT-README.txt maps each feature to
the test file that demonstrates it.

TEST PROJECTS
=============
    tests/SilverAssertions.Tests
        The main suite (xUnit v3). One partial-class file per assertion method
        group, e.g. Primitives/StringAssertionTests.Contain.cs. Run with
        "dotnet test".

    tests/SilverAssertions.Equivalency.Tests
        The BeEquivalentTo suite (xUnit v3): object graphs, collections,
        dictionaries, records, tuples, enums, cyclic references, member
        matching and selection rules, System.Data and XML equivalency.

    tests/TestAssemblyA and tests/TestAssemblyB
        Two tiny fixture assemblies with no tests of their own. They exist so
        the assembly-level and type-selector assertions have real, separate
        assemblies to reason about (TestAssemblyA references TestAssemblyB).

    tests/TestFrameworks/XUnit3.Tests
    tests/TestFrameworks/NUnit4.Tests
    tests/TestFrameworks/MSTestV4.Tests
    tests/TestFrameworks/MSpec.Tests
        Four one-test projects, each referencing a different test framework.
        Each asserts that a failing SilverAssertions assertion throws that
        framework's own assertion-exception type, which is what proves the
        framework adapters work. They are part of the solution and run with the
        normal "dotnet test".

VENDORED LIBRARY SOURCE
=======================
    libs/Reflectify/DennisDoomen.Reflectify
        Source-only reflection helpers (MIT, Dennis Doomen). Not a package
        reference: the library compiles Reflectify.cs directly, as internal
        types. The project here exists so the file can be compiled and tested
        standalone.

    libs/Reflectify/DennisDoomen.Reflectify.Tests
        Tests for the vendored Reflectify source.

    libs/ChillBdd/DennisDoomen.ChillBdd
        A vendored BDD test-helper library (MIT, from the Chill project). Used
        only by tests/SilverAssertions.Tests, which builds a few specs on its
        GivenSubject/TestFor base classes. It has one external dependency,
        Autofac, and never ships in the NuGet package.

    libs/ChillBdd/DennisDoomen.ChillBdd.Tests
        Tests for the vendored Chill source.

Licensing and provenance for all vendored source is recorded in
THIRD-PARTY-NOTICES.txt; maintenance notes are in MAINTAINER-README.txt.

================================================================================
