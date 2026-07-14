namespace DesignPartners.Behavioral;

/// <summary>
/// Notifies subscribers when order lifecycle events occur without coupling the
/// publisher to concrete side effects.
/// </summary>
public sealed class OrderEvents
{
    private readonly List<Action<OrderChanged>> _subscribers = [];

    public void Subscribe(Action<OrderChanged> handler) => _subscribers.Add(handler);

    public void Publish(OrderChanged change)
    {
        foreach (var subscriber in _subscribers.ToArray())
        {
            subscriber(change);
        }
    }
}

public sealed record OrderChanged(string OrderId, string Status, DateTimeOffset OccurredAt);

public sealed class OrderWorkflow(OrderEvents events)
{
    public void Advance(string orderId, string status)
    {
        events.Publish(new OrderChanged(orderId, status, DateTimeOffset.UtcNow));
    }
}
