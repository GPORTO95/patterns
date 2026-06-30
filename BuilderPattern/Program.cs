// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var order = OrderBuilder.Empty()
    .WithNumber(10)
    .WithCreatedOn(DateTime.Now)
    .ShippedTo(b => b
        .Street("Rua Teste")
        .City("Osasco")
        .Zip("06456100")
        .Country("BR"))
    .Build();

Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(order));
