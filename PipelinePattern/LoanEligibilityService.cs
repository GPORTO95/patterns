 namespace Loans.Processing;

public class LoanEligibilityService
{
    public LoanDecision Evaluete(LoanApplication application)
    {
        var warning = new List<string>();

        // Rule 1: Age check
        int age = DateTime.Today.Year - application.DateOfBirth.Year;
        if (application.DateOfBirth > DateTime.Today.AddYears(age)) 
            age--;

        if (age < 18)
        {
            return new LoanDecision
            {
                ApplicationId = application.Id,
                Status = LoanStatus.Rejected,
                Reasons = ["Application must be at least 18 years old"]
            };
        }

        if (age > 65)
        {
            warning.Add("Application age exceed standard policy limit");
        }

        // Rule 2: Minimum income
        if (application.AnnualIncome < 20_000)
        {
            return new LoanDecision
            {
                ApplicationId = application.Id,
                Status = LoanStatus.Rejected,
                Reasons = ["Annual income below minimum threshold ($20,000)"]
            };
        }

        // Rule 3: Debt-to-income ratio
        decimal monthyIncome = application.AnnualIncome / 12;
        decimal proposedPayment = application.RequestedAmount / application.TermMonths;
        decimal dtiRatio = (application.ExistingMonthlyDebt + proposedPayment) / monthyIncome;

        if (dtiRatio > 0.50m)
        {
            return new LoanDecision
            {
                ApplicationId = application.Id,
                Status = LoanStatus.Rejected,
                Reasons = [$"Debt-to-income ratio ({dtiRatio:P0}) exceeds manimum (50%)"]
            };
        }

        if (dtiRatio > 0.36m)
        {
            warning.Add($"Elevated debt-to-income ratio ({dtiRatio:P0})");
        }

        // Rule 4: Loan amount limits
        if (application.RequestedAmount < 1_000)
        {
            return new LoanDecision
            {
                ApplicationId = application.Id,
                Status = LoanStatus.Rejected,
                Reasons = ["Requested amount below minimum ($1,000)"]
            };
        }

        if (application.RequestedAmount > 100_000)
        {
            warning.Add("Requested amount exceed standard approval limit");
        }

        // Rule 5: Employment status
        if (application.EmploymentStatus == EmploymentStatus.Unemployed &&
            application.AnnualIncome < 30_000)
        {
            return new LoanDecision
            {
                ApplicationId = application.Id,
                Status = LoanStatus.Rejected,
                Reasons = ["Unemployed with insufficient income"]
            };
        }

        if (application.EmploymentStatus == EmploymentStatus.SelfEmployed
            && application.YearsEmployed < 2)
        {
            warning.Add("Self-employed for less than 2 years");
        }

        // Decision
        var status = warning.Count switch
        {
            0 => LoanStatus.Approved,
            <= 2 => LoanStatus.ApprovedWithConditions,
            _ => LoanStatus.ManualReview
        };

        return new LoanDecision
        {
            ApplicationId = application.Id,
            Status = status,
            Reasons = warning,
            ApprovedAmount = application.RequestedAmount
        };
    }
}
