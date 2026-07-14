namespace DesignPartners.Creational;

/// <summary>
/// Provides a single shared configuration source with thread-safe lazy
/// initialization — useful when many collaborators need the same settings.
/// </summary>
public sealed class AppSettings
{
    private static readonly Lazy<AppSettings> InstanceFactory =
        new(() => new AppSettings(), LazyThreadSafetyMode.ExecutionAndPublication);

    public static AppSettings Instance => InstanceFactory.Value;

    private AppSettings()
    {
        CatalogName = "DesignPartners Commerce";
        DefaultCurrency = "USD";
        FeatureFlags = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase)
        {
            ["expedited-shipping"] = true,
            ["loyalty-discounts"] = true
        };
    }

    public string CatalogName { get; }
    public string DefaultCurrency { get; }
    public IReadOnlyDictionary<string, bool> FeatureFlags { get; }

    public bool IsEnabled(string feature) =>
        FeatureFlags.TryGetValue(feature, out var enabled) && enabled;
}
