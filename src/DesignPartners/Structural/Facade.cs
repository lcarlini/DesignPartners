namespace DesignPartners.Structural;

/// <summary>
/// Hides inventory reservation, payment capture, and notification behind a
/// single checkout facade so application code stays thin.
/// </summary>
public interface IInventoryService
{
    bool Reserve(string sku, int quantity);
}

public interface IPaymentService
{
    string Capture(string customerId, decimal amount);
}

public interface INotifier
{
    void Notify(string customerId, string message);
}

public sealed class CheckoutFacade(
    IInventoryService inventory,
    IPaymentService payments,
    INotifier notifier)
{
    public CheckoutConfirmation PlaceOrder(string customerId, string sku, int quantity, decimal unitPrice)
    {
        if (!inventory.Reserve(sku, quantity))
        {
            throw new InvalidOperationException($"Insufficient stock for {sku}.");
        }

        var total = unitPrice * quantity;
        var paymentId = payments.Capture(customerId, total);
        notifier.Notify(customerId, $"Order confirmed for {sku} x{quantity}.");

        return new CheckoutConfirmation(Guid.NewGuid().ToString("N")[..12], paymentId, total);
    }
}

public sealed record CheckoutConfirmation(string OrderId, string PaymentId, decimal Total);

public sealed class InMemoryInventoryService : IInventoryService
{
    private readonly Dictionary<string, int> _stock = new(StringComparer.OrdinalIgnoreCase)
    {
        ["ARCH-BOOK"] = 25,
        ["REVIEW-KIT"] = 10
    };

    public bool Reserve(string sku, int quantity)
    {
        if (!_stock.TryGetValue(sku, out var available) || available < quantity)
        {
            return false;
        }

        _stock[sku] = available - quantity;
        return true;
    }
}

public sealed class InMemoryPaymentService : IPaymentService
{
    public string Capture(string customerId, decimal amount) =>
        $"pay-{customerId}-{amount:0.00}";
}

public sealed class CollectingNotifier : INotifier
{
    private readonly List<string> _messages = [];

    public IReadOnlyList<string> Messages => _messages;

    public void Notify(string customerId, string message) =>
        _messages.Add($"{customerId}:{message}");
}
