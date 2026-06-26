namespace Orders.Processing.Strategies;

public class UpsShippingStrategy : IShippingStrategy
{
    public string ProviderName => "UPS";

    public decimal CalculateCost(Order order)
    {
        return order.TotalWeight * 1.2m + 7.50m;
    }
}
