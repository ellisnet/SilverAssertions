using System;
using SilverAssertions.Execution;

namespace SilverAssertions.Common;

/// <summary>
/// Maintains the framework-specific services.
/// </summary>
public static class Services
{
    private static readonly object Lockable = new();
    private static Configuration configuration;

    static Services()
    {
        ResetToDefaults();
    }

    public static IConfigurationStore ConfigurationStore { get; set; }

    public static Configuration Configuration
    {
        get
        {
            lock (Lockable)
            {
                return configuration ??= new Configuration(ConfigurationStore);
            }
        }
    }

    public static Action<string> ThrowException { get; set; }

    public static IReflector Reflector { get; set; }

    public static void ResetToDefaults()
    {
        Reflector = new FullFrameworkReflector();
        ConfigurationStore = new ConfigurationStoreExceptionInterceptor(new AppSettingsConfigurationStore());
        ThrowException = new TestFrameworkProvider(Configuration).Throw;
    }
}
