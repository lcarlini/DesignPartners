namespace DesignPartners.Creational;

/// <summary>
/// Assembles a complex sales order through a fluent builder so construction
/// steps stay readable while the final snapshot remains immutable.
/// </summary>
public sealed class SalesOrder
{
    public required string OrderId { get; init; }
    public required string CustomerId { get; init; }
    public required IReadOnlyList<OrderLine> Lines { get; init; }
    public required string ShippingAddress { get; init; }
    public string? CouponCode { get; init; }
    public bool Expedited { get; init; }

    public decimal Subtotal => Lines.Sum(line => line.UnitPrice * line.Quantity);
}

public sealed record OrderLine(string Sku, int Quantity, decimal UnitPrice);

public sealed class SalesOrderBuilder
{
    private string? _orderId;
    private string? _customerId;
    private string? _shippingAddress;
    private string? _couponCode;
    private bool _expedited;
    private readonly List<OrderLine> _lines = [];

    public SalesOrderBuilder WithOrderId(string orderId)
    {
        _orderId = orderId;
        return this;
    }

    public SalesOrderBuilder ForCustomer(string customerId)
    {
        _customerId = customerId;
        return this;
    }

    public SalesOrderBuilder ShipTo(string shippingAddress)
    {
        _shippingAddress = shippingAddress;
        return this;
    }

    public SalesOrderBuilder AddLine(string sku, int quantity, decimal unitPrice)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        ArgumentOutOfRangeException.ThrowIfNegative(unitPrice);
        _lines.Add(new OrderLine(sku, quantity, unitPrice));
        return this;
    }

    public SalesOrderBuilder WithCoupon(string couponCode)
    {
        _couponCode = couponCode;
        return this;
    }

    public SalesOrderBuilder Expedite()
    {
        _expedited = true;
        return this;
    }

    public SalesOrder Build()
    {
        if (string.IsNullOrWhiteSpace(_orderId))
        {
            throw new InvalidOperationException("OrderId is required.");
        }

        if (string.IsNullOrWhiteSpace(_customerId))
        {
            throw new InvalidOperationException("CustomerId is required.");
        }

        if (string.IsNullOrWhiteSpace(_shippingAddress))
        {
            throw new InvalidOperationException("ShippingAddress is required.");
        }

        if (_lines.Count == 0)
        {
            throw new InvalidOperationException("At least one order line is required.");
        }

        return new SalesOrder
        {
            OrderId = _orderId,
            CustomerId = _customerId,
            ShippingAddress = _shippingAddress,
            CouponCode = _couponCode,
            Expedited = _expedited,
            Lines = _lines.ToArray()
        };
    }
}
