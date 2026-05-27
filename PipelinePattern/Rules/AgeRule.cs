using static System.Net.Mime.MediaTypeNames;

namespace Loans.Processing.Rules;

public sealed class AgeRule : IEligibilityRule
{
    public int Order => 10;

    public void Evaluate(EligibilityContext context)
    {
        int age = DateTime.Today.Year - context.LoanApplication.DateOfBirth.Year;
        if (context.LoanApplication.DateOfBirth > DateTime.Today.AddYears(age))
            age--;

        if (age < 18)
        {
            context.Reject("Application must be at least 18 years old");
            return;
        }

        if (age > 65)
        {
            context.Warn("Application age exceed standard policy limit");
        }
    }
}
