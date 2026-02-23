using System;
using System.Reflection;

namespace SilverAssertions.Tests;

public static class FindAssembly
{
    public static Assembly Containing<T>() => typeof(T).Assembly;

    public static Assembly Stub(string publicKey) => new AssemblyStub(publicKey);

    private sealed class AssemblyStub : Assembly
    {
        private readonly AssemblyName assemblyName = new();

        public override string FullName => nameof(AssemblyStub);

        public AssemblyStub(string publicKey)
        {
            assemblyName.SetPublicKey(FromHexString(publicKey));
        }

        public override AssemblyName GetName() => assemblyName;

        private static byte[] FromHexString(string chars)
            => chars is null
            ? null
            : Convert.FromHexString(chars);
    }
}
