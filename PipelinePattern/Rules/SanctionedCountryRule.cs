namespace Loans.Processing.Rules;

public sealed class SanctionedCountryRule : IEligibilityRule
{
    private static readonly HashSet<string> SanctionedCountries = ["NK", "CU"];
    public int Order => 0;

    public void Evaluate(EligibilityContext context)
    {
        if (SanctionedCountries.Contains(context.LoanApplication.CountryCode))
            context.Reject("Application from sanctioned countries are not accepted");
    }
}
