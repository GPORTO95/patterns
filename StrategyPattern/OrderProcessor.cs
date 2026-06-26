namespace Orders.Processing;

public class OrderProcessorBefore
{
    public decimal CalculateShippingCost(Order order, string shippingProvider)
    {
        switch (shippingProvider)
        {
            case "FedEx":
                return CalculateFedExCost(order);
            case "UPS":
                return CalculateUpsCost(order);
            case "USPS":
                return CalculateUspsCost(order);
            case "DHL":
                return CalculateDhlCost(order);
            default:
                throw new ArgumentException("Unknown shipping provider", nameof(shippingProvider));
        }
    }

    private decimal CalculateFedExCost(Order order)
    {
        return order.TotalWeight * 1.5m + 5.00m;
    }

    private decimal CalculateUpsCost(Order order)
    {
        return order.TotalWeight * 1.2m + 7.50m;
    }

    private decimal CalculateUspsCost(Order order)
    {
        return 8.99m;
    }

    private decimal CalculateDhlCost(Order order)
    {
        return order.TotalWeight * 2.5m + 15.00m;
    }
}

public class OrderProcessorNew
{
    private readonly Dictionary<string, IShippingStrategy> _shippingStrategies;

    public OrderProcessorNew(IEnumerable<IShippingStrategy> shippingStrategies)
    {
        _shippingStrategies = shippingStrategies.ToDictionary(s => s.ProviderName, s => s);
    }

    public decimal CalculateShippingCost(Order order, string shippingProvider)
    {
        if (!_shippingStrategies.TryGetValue(shippingProvider, out var shippingStrategy))
        {
            throw new ArgumentException("Unknown shipping provider", nameof(shippingProvider));
        }

        return shippingStrategy.CalculateCost(order);
    }
}
