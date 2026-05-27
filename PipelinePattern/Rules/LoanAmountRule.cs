using static System.Net.Mime.MediaTypeNames;

namespace Loans.Processing.Rules;

public sealed class LoanAmountRule : IEligibilityRule
{
    public int Order => 40;

    public void Evaluate(EligibilityContext context)
    {
        if (context.LoanApplication.RequestedAmount < 1_000)
        {
            context.Reject("Requested amount below minimum ($1,000)");
            return;
        }

        if (context.LoanApplication.RequestedAmount > 100_000)
        {
            context.Warn("Requested amount exceed standard approval limit");
        }
    }
}
