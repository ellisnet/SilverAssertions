// ReSharper disable InconsistentNaming

namespace SilverAssertions.Execution;

internal class MSTestFrameworkV2 : LateBoundTestFramework
{
    protected override string ExceptionFullName => "Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException";

    protected internal override string AssemblyName => "Microsoft.VisualStudio.TestPlatform.TestFramework";
}

//Note that because MSTestFrameworkV3 and MSTestFrameworkV4 have the same ExceptionFullName and AssemblyName;
//  the wrong one might be detected - i.e. MSTestFrameworkV3 might be detected instead of MSTestFrameworkV4.
//  From testing so far, that doesn't seem to cause any problems.

internal class MSTestFrameworkV3 : LateBoundTestFramework
{
    protected override string ExceptionFullName => "Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException";

    protected internal override string AssemblyName => "MSTest.TestFramework";
}

internal class MSTestFrameworkV4 : LateBoundTestFramework
{
    protected override string ExceptionFullName => "Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException";

    protected internal override string AssemblyName => "MSTest.TestFramework";
}
