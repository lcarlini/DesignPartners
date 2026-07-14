using DesignPartners.Behavioral;
using DesignPartners.Creational;
using DesignPartners.Structural;

Console.WriteLine("DesignPartners — Gang of Four as architecture partners");
Console.WriteLine(new string('=', 58));
Console.WriteLine($"Catalog: {AppSettings.Instance.CatalogName}");
Console.WriteLine($"Currency: {AppSettings.Instance.DefaultCurrency}");
Console.WriteLine();

RunCreational();
RunStructural();
RunBehavioral();

Console.WriteLine("Done. Explore the partners under src/DesignPartners.");

static void RunCreational()
{
    Console.WriteLine("Creational partners");
    Console.WriteLine(new string('-', 58));

    var domestic = new CheckoutService(new DomesticCommerceFactory())
        .Checkout("cust-42", new Money(100m, "USD"));
    Console.WriteLine($"Abstract Factory  → {domestic.Receipt.Provider} charged {domestic.Total}");

    var order = new SalesOrderBuilder()
        .WithOrderId("ORD-1001")
        .ForCustomer("cust-42")
        .ShipTo("Seattle, WA")
        .AddLine("ARCH-BOOK", 2, 49.90m)
        .WithCoupon("WELCOME10")
        .Expedite()
        .Build();
    Console.WriteLine($"Builder           → {order.OrderId} subt={order.Subtotal:0.00} expedited={order.Expedited}");

    var email = new EmailNotificationCreator().Notify("dev@example.com", "Order paid");
    Console.WriteLine($"Factory Method    → {email}");

    var baseBlueprint = new ProductBlueprint
    {
        Sku = "ARCH-BOOK",
        Name = "Architecture Notebook",
        BasePrice = 49.90m,
        Attributes = new Dictionary<string, string> { ["cover"] = "soft" }
    };
    var hardcover = baseBlueprint.WithAttribute("cover", "hardcover");
    Console.WriteLine($"Prototype         → clone cover={hardcover.Attributes["cover"]}");
    Console.WriteLine();
}

static void RunStructural()
{
    Console.WriteLine("Structural partners");
    Console.WriteLine(new string('-', 58));

    IShippingRateProvider shipping = new LegacyFreightAdapter(new LegacyFreightClient());
    var quote = shipping.Quote("98101", "10001", 1.5m);
    Console.WriteLine($"Adapter           → {quote.Carrier} ${quote.Cost:0.00} ~{quote.Estimate.TotalHours:0}h");

    var notifier = new CollectingNotifier();
    var confirmation = new CheckoutFacade(
            new InMemoryInventoryService(),
            new InMemoryPaymentService(),
            notifier)
        .PlaceOrder("cust-42", "ARCH-BOOK", 1, 49.90m);
    Console.WriteLine($"Facade            → order={confirmation.OrderId} pay={confirmation.PaymentId}");

    IProductCatalog catalog = new CachingProductCatalogProxy(new RemoteProductCatalog());
    _ = catalog.Find("ARCH-BOOK");
    _ = catalog.Find("ARCH-BOOK");
    Console.WriteLine($"Proxy             → lookups={catalog.LookupCount} (second call cached)");

    var starter = new CatalogBundle("Starter Pack")
        .Add(new CatalogItem("Architecture Notebook", 49.90m))
        .Add(new CatalogItem("Code Review Kit", 29.50m));
    Console.WriteLine($"Composite         → {starter.Name} total={starter.Price:0.00}");
    Console.WriteLine();
}

static void RunBehavioral()
{
    Console.WriteLine("Behavioral partners");
    Console.WriteLine(new string('-', 58));

    var priced = new PricingEngine(new PercentageDiscountStrategy(10m)).Quote(100m);
    Console.WriteLine($"Strategy          → {priced.Strategy} {priced.Subtotal:0.00} → {priced.Total:0.00}");

    var events = new OrderEvents();
    var trail = new List<string>();
    events.Subscribe(change => trail.Add($"{change.OrderId}:{change.Status}"));
    new OrderWorkflow(events).Advance("ORD-1001", "Paid");
    Console.WriteLine($"Observer          → {trail[0]}");

    var warehouse = new WarehouseStock();
    warehouse.Set("ARCH-BOOK", 10);
    var invoker = new CommandInvoker();
    invoker.Run(new AdjustStockCommand(warehouse, "ARCH-BOOK", -3));
    invoker.UndoLast();
    Console.WriteLine($"Command           → stock after undo={warehouse.Get("ARCH-BOOK")}");

    var fulfillment = new FulfillmentOrder("ORD-1001");
    fulfillment.Pay();
    fulfillment.Ship();
    Console.WriteLine($"State             → {fulfillment.OrderId} is {fulfillment.Status}");

    var report = new OperationsFulfillmentReport().Generate("ORD-1001");
    Console.WriteLine($"Template Method   → {report.Split(Environment.NewLine)[1]}");
}
