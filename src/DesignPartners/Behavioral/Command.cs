namespace DesignPartners.Behavioral;

/// <summary>
/// Encapsulates inventory mutations as undoable commands so operators can
/// reverse accidental adjustments safely.
/// </summary>
public interface IInventoryCommand
{
    string Name { get; }
    void Execute();
    void Undo();
}

public sealed class WarehouseStock
{
    private readonly Dictionary<string, int> _levels = new(StringComparer.OrdinalIgnoreCase);

    public int Get(string sku) => _levels.TryGetValue(sku, out var qty) ? qty : 0;

    public void Set(string sku, int quantity)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(quantity);
        _levels[sku] = quantity;
    }
}

public sealed class AdjustStockCommand(WarehouseStock stock, string sku, int delta) : IInventoryCommand
{
    private int _previous;

    public string Name => $"adjust:{sku}:{delta:+#;-#;0}";

    public void Execute()
    {
        _previous = stock.Get(sku);
        var next = _previous + delta;
        if (next < 0)
        {
            throw new InvalidOperationException($"Stock for {sku} cannot go below zero.");
        }

        stock.Set(sku, next);
    }

    public void Undo() => stock.Set(sku, _previous);
}

public sealed class CommandInvoker
{
    private readonly Stack<IInventoryCommand> _history = new();

    public void Run(IInventoryCommand command)
    {
        command.Execute();
        _history.Push(command);
    }

    public bool UndoLast()
    {
        if (_history.Count == 0)
        {
            return false;
        }

        _history.Pop().Undo();
        return true;
    }
}
