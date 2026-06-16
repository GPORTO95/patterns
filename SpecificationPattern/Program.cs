// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpecificationPattern;

var builder = Host.CreateApplicationBuilder(args);

var app = builder.Build();

await using var scope = app.Services.CreateAsyncScope();
var db = scope.ServiceProvider.GetRequiredService<ShopContext>();

// specification
var cutoff = DateTime.Now.AddDays(-30);

var promotionSpecification = new ActiveCustomerSpecification()
    .And(new RegisteredBeforeSpecification(cutoff))
    .And(new MinimumSpendSpecification(500))
    .And(new AllowedCountrySpecification("US", "CA", "UK"));

var eligible = db.Customers
    .Where(promotionSpecification)
    .ToList();

// querie
var eligible2 = db.Customers
    .EligibleForPromotion(cutoff)
    .ToList();