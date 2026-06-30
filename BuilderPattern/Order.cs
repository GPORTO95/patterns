// See https://aka.ms/new-console-template for more information
public class Order
{
    public int Number { get; set; }
    public DateTime CreatedOn { get; set; }

    public Address ShippingAddress { get; set; }
}
