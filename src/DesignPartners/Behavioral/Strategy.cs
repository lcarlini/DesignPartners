namespace DesignPartners.Behavioral;

/// <summary>
/// Encapsulates interchangeable pricing algorithms so the quote engine can
/// switch discount policy without conditional sprawl.
/// </summary>
public interface IDiscountStrategy
{
    string Name { get; }
    decimal Apply(decimal subtotal);
}

public sealed class NoDiscountStrategy : IDiscountStrategy
{
    public string Name => "none";

    public decimal Apply(decimal subtotal) => subtotal;
}

public sealed class PercentageDiscountStrategy(decimal percent) : IDiscountStrategy
{
    public string Name => $"percent-{percent:0.##}";

    public decimal Apply(decimal subtotal) =>
        decimal.Round(subtotal * (1m - (percent / 100m)), 2);
}

public sealed class ThresholdDiscountStrategy(decimal threshold, decimal amountOff) : IDiscountStrategy
{
    public string Name => $"threshold-{threshold:0.##}";

    public decimal Apply(decimal subtotal) =>
        subtotal >= threshold
            ? decimal.Round(Math.Max(0m, subtotal - amountOff), 2)
            : subtotal;
}

public sealed class PricingEngine(IDiscountStrategy strategy)
{
    public QuotePrice Quote(decimal subtotal) =>
        new(subtotal, strategy.Apply(subtotal), strategy.Name);
}

public sealed record QuotePrice(decimal Subtotal, decimal Total, string Strategy);
