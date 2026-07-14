namespace DesignPartners.Creational;

/// <summary>
/// Creates families of related payment and tax collaborators without binding
/// callers to concrete regional implementations.
/// </summary>
public interface ICommerceFactory
{
    IPaymentGateway CreatePaymentGateway();
    ITaxCalculator CreateTaxCalculator();
}

public interface IPaymentGateway
{
    PaymentReceipt Charge(Money amount, string customerId);
}

public interface ITaxCalculator
{
    Money Calculate(Money subtotal);
}

public readonly record struct Money(decimal Amount, string Currency)
{
    public override string ToString() => $"{Currency} {Amount:0.00}";
}

public sealed record PaymentReceipt(string Provider, string TransactionId, Money Amount);

public sealed class DomesticCommerceFactory : ICommerceFactory
{
    public IPaymentGateway CreatePaymentGateway() => new CardPaymentGateway("DomesticPay");

    public ITaxCalculator CreateTaxCalculator() => new FlatRateTaxCalculator(0.08m, "USD");
}

public sealed class InternationalCommerceFactory : ICommerceFactory
{
    public IPaymentGateway CreatePaymentGateway() => new CardPaymentGateway("GlobalPay");

    public ITaxCalculator CreateTaxCalculator() => new FlatRateTaxCalculator(0.20m, "EUR");
}

file sealed class CardPaymentGateway(string provider) : IPaymentGateway
{
    public PaymentReceipt Charge(Money amount, string customerId) =>
        new(provider, $"{provider}-{customerId}-{Guid.NewGuid():N}"[..24], amount);
}

file sealed class FlatRateTaxCalculator(decimal rate, string currency) : ITaxCalculator
{
    public Money Calculate(Money subtotal) =>
        new(decimal.Round(subtotal.Amount * rate, 2), currency);
}

public sealed class CheckoutService(ICommerceFactory factory)
{
    public CheckoutResult Checkout(string customerId, Money subtotal)
    {
        var tax = factory.CreateTaxCalculator().Calculate(subtotal);
        var total = new Money(subtotal.Amount + tax.Amount, tax.Currency);
        var receipt = factory.CreatePaymentGateway().Charge(total, customerId);
        return new CheckoutResult(subtotal, tax, total, receipt);
    }
}

public sealed record CheckoutResult(
    Money Subtotal,
    Money Tax,
    Money Total,
    PaymentReceipt Receipt);
