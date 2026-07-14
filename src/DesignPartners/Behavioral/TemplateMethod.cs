namespace DesignPartners.Behavioral;

/// <summary>
/// Defines the skeleton of a fulfillment report while letting subclasses
/// customize data gathering and formatting steps.
/// </summary>
public abstract class FulfillmentReport
{
    public string Generate(string orderId)
    {
        var header = BuildHeader(orderId);
        var body = BuildBody(orderId);
        var footer = BuildFooter();
        return string.Join(Environment.NewLine, header, body, footer);
    }

    protected virtual string BuildHeader(string orderId) => $"# Fulfillment Report {orderId}";

    protected abstract string BuildBody(string orderId);

    protected virtual string BuildFooter() => $"Generated at {DateTimeOffset.UtcNow:u}";
}

public sealed class OperationsFulfillmentReport : FulfillmentReport
{
    protected override string BuildBody(string orderId) =>
        $"Operations checklist for {orderId}: pack, label, handoff.";
}

public sealed class FinanceFulfillmentReport : FulfillmentReport
{
    protected override string BuildBody(string orderId) =>
        $"Finance summary for {orderId}: captured payment, pending settlement.";
}
