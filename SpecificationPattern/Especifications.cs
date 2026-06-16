using System.Linq.Expressions;

namespace SpecificationPattern;

public class ActiveCustomerSpecification : Specification<Customer>
{
    public override Expression<Func<Customer, bool>> ToExpression()
    {
        return customer => customer.IsActive;
    }
}

public class RegisteredBeforeSpecification : Specification<Customer>
{
    private readonly DateTime _cutOff;

    public RegisteredBeforeSpecification(DateTime cutOff) => _cutOff = cutOff;

    public override Expression<Func<Customer, bool>> ToExpression()
    {
        return customer => customer.DateRegistered <= _cutOff;
    }
}

public class MinimumSpendSpecification : Specification<Customer>
{
    private readonly decimal _minimum;

    public MinimumSpendSpecification(decimal minimum) => _minimum = minimum;

    public override Expression<Func<Customer, bool>> ToExpression()
    {
        return customer => customer.TotalSpent >= _minimum;
    }
}

public class AllowedCountrySpecification : Specification<Customer>
{
    private readonly string[] _countries;

    public AllowedCountrySpecification(params string[] countries) => _countries = countries;

    public override Expression<Func<Customer, bool>> ToExpression()
    {
        return customer => _countries.Contains(customer.Country);
    }
}