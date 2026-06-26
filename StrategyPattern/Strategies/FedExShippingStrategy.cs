namespace Orders.Processing.Strategies;

public class FedExShippingStrategy : IShippingStrategy
{
    public string ProviderName => "FedEx";

    public decimal CalculateCost(Order order)
    {
        return order.TotalWeight * 1.5m + 5.00m;
    }
}
