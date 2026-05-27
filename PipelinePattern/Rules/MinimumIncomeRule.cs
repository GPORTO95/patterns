using static System.Net.Mime.MediaTypeNames;

namespace Loans.Processing.Rules;

public sealed class MinimumIncomeRule : IEligibilityRule
{
    public int Order => 20;

    public void Evaluate(EligibilityContext context)
    {
        if (context.LoanApplication.AnnualIncome < 20_000)
        {
            //return new LoanDecision
            //{
            //    ApplicationId = application.Id,
            //    Status = LoanStatus.Rejected,
            //    Reasons = ["Annual income below minimum threshold ($20,000)"]
            //};
            context.Reject("Annual income below minimum threshold ($20,000)");
            return;
        }
    }
}
