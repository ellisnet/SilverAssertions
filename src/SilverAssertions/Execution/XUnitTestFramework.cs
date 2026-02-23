using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace SilverAssertions.Execution;

/// <summary>
/// Implements the XUnit (version 2) test framework adapter.
/// </summary>
internal class XUnit2TestFramework : ITestFramework
{
    private Assembly assembly;

    public bool IsAvailable
    {
        get
        {
            try
            {
                // For netfx the assembly is not in AppDomain by default, so we can't just scan AppDomain.CurrentDomain
                assembly = Assembly.Load(new AssemblyName("xunit.assert"));

                return assembly is not null;
            }
            catch
            {
                return false;
            }
        }
    }

    [DoesNotReturn]
    public void Throw(string message)
    {
        Type exceptionType = assembly.GetType("Xunit.Sdk.XunitException")
            ?? throw new NotSupportedException("Failed to create the XUnit assertion type");

        throw (Exception)Activator.CreateInstance(exceptionType, message);
    }
}

/// <summary>
/// Implements the XUnit (version 3) test framework adapter.
/// </summary>
internal class XUnit3TestFramework : ITestFramework
{
    private Assembly assembly;

    public bool IsAvailable
    {
        get
        {
            try
            {
                // xUnit v3 uses a different assembly name than v2
                assembly = Assembly.Load(new AssemblyName("xunit.v3.assert"));

                return assembly is not null;
            }
            catch
            {
                return false;
            }
        }
    }

    [DoesNotReturn]
    public void Throw(string message)
    {
        Type exceptionType = assembly.GetType("Xunit.Sdk.XunitException")
                             ?? throw new NotSupportedException("Failed to create the XUnit assertion type");

        throw (Exception)Activator.CreateInstance(exceptionType, message);
    }
}
