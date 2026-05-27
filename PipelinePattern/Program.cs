using Loans.Processing;
using Loans.Processing.Rules;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder();

builder.Services.AddTransient<IEligibilityRule, AgeRule>();
builder.Services.AddTransient<IEligibilityRule, MinimumIncomeRule>();
builder.Services.AddTransient<IEligibilityRule, DebtToIncomeRule>();
builder.Services.AddTransient<IEligibilityRule, LoanAmountRule>();
builder.Services.AddTransient<IEligibilityRule, EmploymentRule>();
builder.Services.AddTransient<IEligibilityRule, SanctionedCountryRule>();

builder.Services.AddTransient<EligibilityPipeline>();

var app = builder.Build();

var applications = new List<LoanApplication>
{
    new()
    {
        Id = 1,
        ApplicationName = "Alice Smith",
        DateOfBirth = new DateTime(1985, 5, 15),
        AnnualIncome = 85_000,
        ExistingMonthlyDebt = 500,
        RequestedAmount = 25_000,
        TermMonths = 60,
        EmploymentStatus = EmploymentStatus.Employed,
        YearsEmployed = 5,
        CountryCode = "US"
    },
    new()
    {
        Id = 2,
        ApplicationName = "Bob Jones",
        DateOfBirth = new DateTime(2010, 5, 15), //under 18
        AnnualIncome = 50_000,
        ExistingMonthlyDebt = 200,
        RequestedAmount = 10_000,
        TermMonths = 36,
        EmploymentStatus = EmploymentStatus.Employed,
        YearsEmployed = 5,
        CountryCode = "US"
    },
    new()
    {
        Id = 3,
        ApplicationName = "Carol White",
        DateOfBirth = new DateTime(1985, 5, 15),
        AnnualIncome = 60_000,
        ExistingMonthlyDebt = 2_000,
        RequestedAmount = 150_000, // Exceeds standard limit + high DT
        TermMonths = 60,
        EmploymentStatus = EmploymentStatus.SelfEmployed,
        YearsEmployed = 1,
        CountryCode = "US"
    },
    new()
    {
        Id = 4,
        ApplicationName = "Dave Park",
        DateOfBirth = new DateTime(1975, 5, 15),
        AnnualIncome = 15_000, // Bellow minimum
        ExistingMonthlyDebt = 100,
        RequestedAmount = 5_000,
        TermMonths = 60,
        EmploymentStatus = EmploymentStatus.Unemployed,
        YearsEmployed = 0,
        CountryCode = "US"
    },
    new()
    {
        Id = 5,
        ApplicationName = "Eva Chen",
        DateOfBirth = new DateTime(1992, 5, 15), 
        AnnualIncome = 100_000,
        ExistingMonthlyDebt = 300,
        RequestedAmount = 20_000,
        TermMonths = 48,
        EmploymentStatus = EmploymentStatus.Employed,
        YearsEmployed = 8,
        CountryCode = "NK" // Sanctioned country
    },
    new()
    {
        Id = 1,
        ApplicationName = "John Kenedy",
        DateOfBirth = new DateTime(1950, 5, 15), // much old
        AnnualIncome = 90_000,
        ExistingMonthlyDebt = 500,
        RequestedAmount = 25_000,
        TermMonths = 60,
        EmploymentStatus = EmploymentStatus.Employed,
        YearsEmployed = 5,
        CountryCode = "US"
    },
};


// BEFORE: Monolithic If/Else Service ----------------------------------------------
PrintHeader("BEFORE - Monolithic If/Else Service");

var legacyService = new LoanEligibilityService();

foreach (var application in applications)
{
    var decision = legacyService.Evaluete(application);
    PrintDecision(application, decision);
}

Console.WriteLine();

// AFTER: Pipeline Pattern ----------------------------------------------
PrintHeader("AFTER - Pipeline Pattern");

var pipeline = app.Services.GetRequiredService<EligibilityPipeline>();

foreach (var application in applications)
{
    var decision = pipeline.Run(application);
    PrintDecision(application, decision);
}

Console.WriteLine();

static void PrintHeader(string title)
{
    Console.WriteLine();
    Console.WriteLine($"|-- {title} {new string('-', Math.Max(0, 60 - title.Length))} --|");
    Console.WriteLine();
}

static void PrintDecision(LoanApplication application, LoanDecision decision)
{
    Console.WriteLine($"    [{decision.Status}] {application.ApplicationName} (#{application.Id})");

    if (decision.ApprovedAmount.HasValue)
        Console.WriteLine($"    Approved amount: ${decision.ApprovedAmount:N0}");

    foreach (var reason in decision.Reasons)
        Console.WriteLine($"    -> {reason}");

    Console.WriteLine();
}
