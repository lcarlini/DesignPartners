namespace DesignPartners.Structural;

/// <summary>
/// Adapts a legacy freight API into the modern shipping rate contract used by
/// the checkout pipeline.
/// </summary>
public interface IShippingRateProvider
{
    ShippingQuote Quote(string origin, string destination, decimal weightKg);
}

public sealed record ShippingQuote(string Carrier, decimal Cost, TimeSpan Estimate);

/// <summary>Simulates a third-party API that cannot be changed.</summary>
public sealed class LegacyFreightClient
{
    public string GetRateCsv(string fromZip, string toZip, double pounds) =>
        $"LEGACY,{fromZip}->{toZip},{pounds * 1.35:0.00},48";
}

public sealed class LegacyFreightAdapter(LegacyFreightClient client) : IShippingRateProvider
{
    public ShippingQuote Quote(string origin, string destination, decimal weightKg)
    {
        var pounds = (double)(weightKg * 2.20462m);
        var parts = client.GetRateCsv(origin, destination, pounds).Split(',');

        return new ShippingQuote(
            Carrier: parts[0],
            Cost: decimal.Parse(parts[2]),
            Estimate: TimeSpan.FromHours(int.Parse(parts[3])));
    }
}
