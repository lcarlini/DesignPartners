namespace DesignPartners.Creational;

/// <summary>
/// Clones configured product blueprints so variants can be created cheaply
/// without replaying expensive setup steps.
/// </summary>
public interface IPrototype<out T>
{
    T Clone();
}

public sealed class ProductBlueprint : IPrototype<ProductBlueprint>
{
    public required string Sku { get; init; }
    public required string Name { get; init; }
    public required decimal BasePrice { get; init; }
    public required Dictionary<string, string> Attributes { get; init; }

    public ProductBlueprint Clone() => new()
    {
        Sku = Sku,
        Name = Name,
        BasePrice = BasePrice,
        Attributes = new Dictionary<string, string>(Attributes)
    };

    public ProductBlueprint WithAttribute(string key, string value)
    {
        var clone = Clone();
        clone.Attributes[key] = value;
        return clone;
    }
}
