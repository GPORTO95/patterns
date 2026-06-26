namespace Orders.Processing;

public interface IShippingStrategy
{
    string ProviderName { get; }
    decimal CalculateCost(Order order);
}
