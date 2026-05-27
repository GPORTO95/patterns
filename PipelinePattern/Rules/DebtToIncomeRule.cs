using System.Data;
using static System.Net.Mime.MediaTypeNames;

namespace Loans.Processing.Rules;

public sealed class DebtToIncomeRule : IEligibilityRule
{
    public int Order => 30;

    public void Evaluate(EligibilityContext context)
    {
        decimal monthyIncome = context.LoanApplication.AnnualIncome / 12;
        decimal proposedPayment = context.LoanApplication.RequestedAmount / context.LoanApplication.TermMonths;
        decimal dtiRatio = (context.LoanApplication.ExistingMonthlyDebt + proposedPayment) / monthyIncome;

        if (dtiRatio > 0.50m)
        {
            context.Reject($"Debt-to-income ratio ({dtiRatio:P0}) exceeds manimum (50%)");
            return;
        }

        if (dtiRatio > 0.36m)
        {
            context.Warn($"Elevated debt-to-income ratio ({dtiRatio:P0})");
        }
    }
}
