using System.Diagnostics;
using System.Reflection;

namespace SilverAssertions.Tests;

/// <summary>
/// Reports whether this test assembly was compiled with JIT optimizations enabled.
/// </summary>
/// <remarks>
/// <see cref="CallerIdentifier"/> recovers the name of the variable under assertion by
/// walking the stack to the caller's frame and reading that source line. In an optimized
/// build the JIT can inline the caller away, the frame disappears, and the name degrades
/// to a generic noun ("object", "function", "root"). The assertion still fails correctly;
/// only the wording of the message changes.
/// <para>
/// A handful of tests assert on that recovered name and therefore cannot pass in an
/// optimized build. They call <c>Assert.SkipWhen(JitOptimization.IsEnabled, ...)</c> so
/// they run normally in Debug and are skipped - not failed - in Release.
/// </para>
/// <para>
/// This keys off <see cref="DebuggableAttribute.IsJITOptimizerDisabled"/> rather than a
/// <c>#if DEBUG</c>, so it tracks the optimization setting itself. A Release build with
/// <c>-p:Optimize=false</c> is correctly treated as unoptimized, and those tests run.
/// </para>
/// </remarks>
internal static class JitOptimization
{
    /// <summary>
    /// <c>true</c> when the JIT optimizer is enabled for this assembly (a normal Release
    /// build); <c>false</c> when optimizations are disabled (a normal Debug build).
    /// </summary>
    public static bool IsEnabled { get; } =
        typeof(JitOptimization).Assembly.GetCustomAttribute<DebuggableAttribute>()
            is not { IsJITOptimizerDisabled: true };
}
