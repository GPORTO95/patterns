namespace Loans.Processing;

public class LoanDecision
{
    public int ApplicationId { get; set; }
    public decimal? ApprovedAmount { get; set; }
    public LoanStatus Status { get; set; }
    public List<string> Reasons { get; set; } = [];
}

public enum LoanStatus
{
    Rejected,
    Approved,
    ApprovedWithConditions,
    ManualReview
}