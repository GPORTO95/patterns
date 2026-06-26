using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orders.Processing;
using Orders.Processing.Strategies;

var builder = Host.CreateApplicationBuilder();

builder.Services.AddTransient<IShippingStrategy, FedExShippingStrategy>();
builder.Services.AddTransient<IShippingStrategy, UpsShippingStrategy>();
builder.Services.AddTransient<IShippingStrategy, UspsShippingStrategy>();
builder.Services.AddTransient<IShippingStrategy, DhlShippingStrategy>();

builder.Services.AddTransient<OrderProcessorNew>();

var app = builder.Build();

PrintHeader("BEFORE - Switch Statement");

var processorBefore = new OrderProcessorBefore();

var lightOrder = new Order { Id = 1, CustomerName = "Alice Smith", TotalWeight = 2m };
var heavyOrder = new Order { Id = 2, CustomerName = "Bob Jones", TotalWeight = 20m };

Console.WriteLine($"Light order ({lightOrder.TotalWeight} lbs:)");
Console.WriteLine($"  FedEx : ${processorBefore.CalculateShippingCost(lightOrder, "FedEx"):F2}");
Console.WriteLine($"  UPS   : ${processorBefore.CalculateShippingCost(lightOrder, "UPS"):F2}");
Console.WriteLine($"  USPS  : ${processorBefore.CalculateShippingCost(lightOrder, "USPS"):F2}");
Console.WriteLine($"  DHL   : ${processorBefore.CalculateShippingCost(lightOrder, "DHL"):F2}");

Console.WriteLine();

PrintHeader("AFTER - Strategy Pattern");

var processor = app.Services.GetRequiredService<OrderProcessorNew>();

Console.WriteLine($"Light order {lightOrder.TotalWeight} lbs:");

foreach (var provider in new[] { "FedEx", "UPS", "USPS", "DHL" })
{
    Console.WriteLine($"   {provider, -6}: ${processor.CalculateShippingCost(lightOrder, provider):F2}");
}

PrintHeader("Error Handling - Unknowm Provider");

try
{
    processorBefore.CalculateShippingCost(lightOrder, "Amazon");
}
catch(ArgumentException ex)
{
    Console.WriteLine($"     Caught: {ex.Message}");
}


static void PrintHeader(string title)
{
    Console.WriteLine();
    Console.WriteLine($"|-- {title} {new string('-', Math.Max(0, 60 - title.Length))} --|");
    Console.WriteLine();
}
