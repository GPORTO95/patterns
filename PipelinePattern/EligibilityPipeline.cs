using static System.Net.Mime.MediaTypeNames;

namespace Loans.Processing;

public sealed class EligibilityPipeline
{
    private readonly IEnumerable<IEligibilityRule> _rules;

    public EligibilityPipeline(IEnumerable<IEligibilityRule> rules)
    {
        _rules = rules.OrderBy(r => r.Order);
    }

    public LoanDecision Run(LoanApplication loanApplication)
    {
        var context = new EligibilityContext
        {
            LoanApplication = loanApplication
        };

        foreach (var eligibilityRule in _rules)
        {
            eligibilityRule.Evaluate(context);

            if (context.IsRejected)
            {
                return new LoanDecision
                {
                    ApplicationId = loanApplication.Id,
                    Status = LoanStatus.Rejected,
                    Reasons = [context.RejectedReason ?? "N/A"] 
                };
            }
        }

        var status = context.Warnings.Count switch
        {
            0 => LoanStatus.Approved,
            <= 2 => LoanStatus.ApprovedWithConditions,
            _ => LoanStatus.ManualReview
        };

        return new LoanDecision
        {
            ApplicationId = loanApplication.Id,
            Status = status,
            Reasons = context.Warnings,
            ApprovedAmount = loanApplication.RequestedAmount
        };
    }
}
