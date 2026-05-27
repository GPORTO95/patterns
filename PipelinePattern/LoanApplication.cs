namespace Loans.Processing;

public class LoanApplication
{
    public int Id { get; set; }
    public int YearsEmployed { get; set; }
    public string ApplicationName { get; set; }
    public string CountryCode { get; set; }
    public decimal AnnualIncome { get; set; }
    public decimal RequestedAmount { get; set; }
    public decimal TermMonths { get; set; }
    public decimal ExistingMonthlyDebt { get; set; }
    public DateTime DateOfBirth { get; set; }
    public EmploymentStatus EmploymentStatus { get; set; }
}

public enum EmploymentStatus 
{
    Unemployed,
    SelfEmployed,
    Employed
}