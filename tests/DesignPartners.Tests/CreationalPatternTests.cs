using DesignPartners.Creational;

namespace DesignPartners.Tests;

public sealed class CreationalPatternTests
{
    [Fact]
    public void AbstractFactory_CreatesConsistentDomesticFamily()
    {
        var result = new CheckoutService(new DomesticCommerceFactory())
            .Checkout("cust-1", new Money(100m, "USD"));

        Assert.Equal("DomesticPay", result.Receipt.Provider);
        Assert.Equal(8.00m, result.Tax.Amount);
        Assert.Equal(108.00m, result.Total.Amount);
    }

    [Fact]
    public void Builder_RequiresLinesAndBuildsImmutableOrder()
    {
        var order = new SalesOrderBuilder()
            .WithOrderId("ORD-1")
            .ForCustomer("cust-1")
            .ShipTo("Austin, TX")
            .AddLine("ARCH-BOOK", 2, 50m)
            .Build();

        Assert.Equal(100m, order.Subtotal);
        Assert.Throws<InvalidOperationException>(() => new SalesOrderBuilder().Build());
    }

    [Fact]
    public void FactoryMethod_RoutesThroughChosenChannel()
    {
        var email = new EmailNotificationCreator().Notify("a@b.com", "hi");
        var sms = new SmsNotificationCreator().Notify("+15551212", "hi");

        Assert.StartsWith("email:", email);
        Assert.StartsWith("sms:", sms);
    }

    [Fact]
    public void Prototype_ClonesIndependently()
    {
        var original = new ProductBlueprint
        {
            Sku = "SKU-1",
            Name = "Notebook",
            BasePrice = 10m,
            Attributes = new Dictionary<string, string> { ["color"] = "blue" }
        };

        var clone = original.WithAttribute("color", "red");

        Assert.Equal("blue", original.Attributes["color"]);
        Assert.Equal("red", clone.Attributes["color"]);
    }

    [Fact]
    public void Singleton_ReturnsSameInstance()
    {
        Assert.Same(AppSettings.Instance, AppSettings.Instance);
        Assert.True(AppSettings.Instance.IsEnabled("expedited-shipping"));
    }
}
