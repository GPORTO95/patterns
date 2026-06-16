namespace SpecificationPattern;

public static class CustomerQueries
{
    public static IQueryable<Customer> EligibleForPromotion(
        this IQueryable<Customer> customers,
        DateTime cutOff)
    {
        return customers.Where(c => 
            c.IsActive &&
            c.DateRegistered <= cutOff &&
            c.TotalSpent >= 500);
    }
}
