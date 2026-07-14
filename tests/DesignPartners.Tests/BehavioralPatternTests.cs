using DesignPartners.Behavioral;

namespace DesignPartners.Tests;

public sealed class BehavioralPatternTests
{
    [Fact]
    public void Strategy_AppliesSelectedDiscountPolicy()
    {
        var percent = new PricingEngine(new PercentageDiscountStrategy(10m)).Quote(200m);
        var threshold = new PricingEngine(new ThresholdDiscountStrategy(100m, 15m)).Quote(200m);

        Assert.Equal(180m, percent.Total);
        Assert.Equal(185m, threshold.Total);
    }

    [Fact]
    public void Observer_NotifiesSubscribers()
    {
        var events = new OrderEvents();
        OrderChanged? observed = null;
        events.Subscribe(change => observed = change);

        new OrderWorkflow(events).Advance("ORD-9", "Paid");

        Assert.NotNull(observed);
        Assert.Equal("Paid", observed!.Status);
    }

    [Fact]
    public void Command_SupportsUndo()
    {
        var stock = new WarehouseStock();
        stock.Set("SKU", 5);
        var invoker = new CommandInvoker();

        invoker.Run(new AdjustStockCommand(stock, "SKU", -2));
        Assert.Equal(3, stock.Get("SKU"));

        Assert.True(invoker.UndoLast());
        Assert.Equal(5, stock.Get("SKU"));
    }

    [Fact]
    public void State_EnforcesValidTransitions()
    {
        var order = new FulfillmentOrder("ORD-1");
        order.Pay();
        order.Ship();

        Assert.Equal("Shipped", order.Status);
        Assert.Throws<InvalidOperationException>(() => order.Cancel());
    }

    [Fact]
    public void TemplateMethod_KeepsSharedStructure()
    {
        var report = new FinanceFulfillmentReport().Generate("ORD-7");

        Assert.Contains("# Fulfillment Report ORD-7", report);
        Assert.Contains("Finance summary", report);
        Assert.Contains("Generated at", report);
    }
}
