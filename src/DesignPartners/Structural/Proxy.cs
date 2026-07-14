namespace DesignPartners.Structural;

/// <summary>
/// Adds caching in front of an expensive catalog lookup without changing the
/// callers that depend on <see cref="IProductCatalog"/>.
/// </summary>
public interface IProductCatalog
{
    Product? Find(string sku);
    int LookupCount { get; }
}

public sealed record Product(string Sku, string Name, decimal Price);

public sealed class RemoteProductCatalog : IProductCatalog
{
    private readonly Dictionary<string, Product> _products = new(StringComparer.OrdinalIgnoreCase)
    {
        ["ARCH-BOOK"] = new("ARCH-BOOK", "Architecture Notebook", 49.90m),
        ["REVIEW-KIT"] = new("REVIEW-KIT", "Code Review Kit", 29.50m)
    };

    public int LookupCount { get; private set; }

    public Product? Find(string sku)
    {
        LookupCount++;
        return _products.TryGetValue(sku, out var product) ? product : null;
    }
}

public sealed class CachingProductCatalogProxy(IProductCatalog inner) : IProductCatalog
{
    private readonly Dictionary<string, Product?> _cache = new(StringComparer.OrdinalIgnoreCase);

    public int LookupCount => inner.LookupCount;

    public Product? Find(string sku)
    {
        if (_cache.TryGetValue(sku, out var cached))
        {
            return cached;
        }

        var product = inner.Find(sku);
        _cache[sku] = product;
        return product;
    }
}
