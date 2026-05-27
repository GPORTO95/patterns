using static System.Net.Mime.MediaTypeNames;

namespace Loans.Processing.Rules;

public sealed class EmploymentRule : IEligibilityRule
{
    public int Order => 50;

    public void Evaluate(EligibilityContext context)
    {
        if (context.LoanApplication.EmploymentStatus == EmploymentStatus.Unemployed &&
            context.LoanApplication.AnnualIncome < 30_000)
        {
            context.Reject("Unemployed with insufficient income");
            return;
        }

        if (context.LoanApplication.EmploymentStatus == EmploymentStatus.SelfEmployed
            && context.LoanApplication.YearsEmployed < 2)
        {
            context.Warn("Self-employed for less than 2 years");
        }
    }
}
