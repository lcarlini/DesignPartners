using DesignPartners.Structural;

namespace DesignPartners.Tests;

public sealed class StructuralPatternTests
{
    [Fact]
    public void Adapter_NormalizesLegacyFreightResponse()
    {
        IShippingRateProvider provider = new LegacyFreightAdapter(new LegacyFreightClient());

        var quote = provider.Quote("98101", "10001", 1m);

        Assert.Equal("LEGACY", quote.Carrier);
        Assert.True(quote.Cost > 0);
        Assert.Equal(TimeSpan.FromHours(48), quote.Estimate);
    }

    [Fact]
    public void Facade_CoordinatesCheckoutCollaborators()
    {
        var notifier = new CollectingNotifier();
        var facade = new CheckoutFacade(
            new InMemoryInventoryService(),
            new InMemoryPaymentService(),
            notifier);

        var confirmation = facade.PlaceOrder("cust-1", "ARCH-BOOK", 2, 10m);

        Assert.Equal(20m, confirmation.Total);
        Assert.Single(notifier.Messages);
    }

    [Fact]
    public void Proxy_CachesCatalogLookups()
    {
        var remote = new RemoteProductCatalog();
        IProductCatalog catalog = new CachingProductCatalogProxy(remote);

        _ = catalog.Find("ARCH-BOOK");
        _ = catalog.Find("ARCH-BOOK");

        Assert.Equal(1, catalog.LookupCount);
    }

    [Fact]
    public void Composite_SumsNestedBundlePrices()
    {
        var bundle = new CatalogBundle("Pack")
            .Add(new CatalogItem("A", 10m))
            .Add(new CatalogBundle("Nested").Add(new CatalogItem("B", 5m)));

        Assert.Equal(15m, bundle.Price);
        Assert.Contains("Nested", bundle.Describe());
    }
}
