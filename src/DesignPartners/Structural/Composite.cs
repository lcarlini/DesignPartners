namespace DesignPartners.Structural;

/// <summary>
/// Treats individual products and nested bundles uniformly when calculating
/// catalog totals and descriptions.
/// </summary>
public abstract class CatalogComponent
{
    public abstract string Name { get; }
    public abstract decimal Price { get; }
    public abstract string Describe(int indent = 0);
}

public sealed class CatalogItem(string name, decimal price) : CatalogComponent
{
    public override string Name { get; } = name;
    public override decimal Price { get; } = price;

    public override string Describe(int indent = 0) =>
        $"{new string(' ', indent * 2)}- {Name}: {Price:0.00}";
}

public sealed class CatalogBundle(string name) : CatalogComponent
{
    private readonly List<CatalogComponent> _children = [];

    public override string Name { get; } = name;
    public override decimal Price => _children.Sum(child => child.Price);

    public CatalogBundle Add(CatalogComponent component)
    {
        _children.Add(component);
        return this;
    }

    public override string Describe(int indent = 0)
    {
        var lines = new List<string>
        {
            $"{new string(' ', indent * 2)}+ {Name}: {Price:0.00}"
        };

        lines.AddRange(_children.Select(child => child.Describe(indent + 1)));
        return string.Join(Environment.NewLine, lines);
    }
}
