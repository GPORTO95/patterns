namespace Orders.Processing.Strategies;

public class DhlShippingStrategy : IShippingStrategy
{
    public string ProviderName => "DHL";

    public decimal CalculateCost(Order order)
    {
        return order.TotalWeight * 2.5m + 15.00m;
    }
}