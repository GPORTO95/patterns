namespace Orders.Processing.Strategies;

public interface IUspsApi
{
    decimal Fee();
}

public class UspsShippingStrategy() : IShippingStrategy
{
    public string ProviderName => "USPS";

    public decimal CalculateCost(Order order)
    {
        return 8.99m;// + api.Fee();
    }
}
