// See https://aka.ms/new-console-template for more information
public class OrderBuilder
{
    private int _number;
    private DateTime _createdOn;
    private AddressBuilder _shippingAddress = AddressBuilder.Empty();

    private OrderBuilder()
    { }

    public static OrderBuilder Empty() => new();

    public OrderBuilder WithNumber(int number)
    {
        _number = number;
        return this;
    }

    public OrderBuilder WithCreatedOn(DateTime createdOn)
    {
        _createdOn = createdOn;
        return this;
    }

    public OrderBuilder ShippedTo(Action<AddressBuilder> action)
    {
        action(_shippingAddress);

        return this;
    }

    public Order Build()
    {
        return new Order
        {
            Number = _number,
            CreatedOn = _createdOn,
            ShippingAddress = _shippingAddress.Build(),
        };
    }
}
