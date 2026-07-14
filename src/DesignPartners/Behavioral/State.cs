namespace DesignPartners.Behavioral;

/// <summary>
/// Models an order lifecycle where each state owns the allowed transitions
/// and side-effect messaging.
/// </summary>
public sealed class FulfillmentOrder
{
    public FulfillmentOrder(string orderId)
    {
        OrderId = orderId;
        State = new PendingState(this);
    }

    public string OrderId { get; }
    public string Status => State.Name;
    internal OrderState State { get; set; }

    public void Pay() => State.Pay();
    public void Ship() => State.Ship();
    public void Cancel() => State.Cancel();
}

public abstract class OrderState(FulfillmentOrder order)
{
    protected FulfillmentOrder Order { get; } = order;
    public abstract string Name { get; }

    public virtual void Pay() => throw new InvalidOperationException($"Cannot pay from {Name}.");
    public virtual void Ship() => throw new InvalidOperationException($"Cannot ship from {Name}.");
    public virtual void Cancel() => throw new InvalidOperationException($"Cannot cancel from {Name}.");
}

public sealed class PendingState(FulfillmentOrder order) : OrderState(order)
{
    public override string Name => "Pending";

    public override void Pay() => Order.State = new PaidState(Order);

    public override void Cancel() => Order.State = new CancelledState(Order);
}

public sealed class PaidState(FulfillmentOrder order) : OrderState(order)
{
    public override string Name => "Paid";

    public override void Ship() => Order.State = new ShippedState(Order);

    public override void Cancel() => Order.State = new CancelledState(Order);
}

public sealed class ShippedState(FulfillmentOrder order) : OrderState(order)
{
    public override string Name => "Shipped";
}

public sealed class CancelledState(FulfillmentOrder order) : OrderState(order)
{
    public override string Name => "Cancelled";
}
