namespace Loans.Processing;

public sealed class EligibilityContext
{
    public required LoanApplication LoanApplication { get; init; }
    public List<string> Warnings { get; set; } = [];
    public bool IsRejected { get; set; }
    public string? RejectedReason { get; set; }

    public void Reject(string reason)
    {
        IsRejected = true;
        RejectedReason = reason;
    }

    public void Warn(string reason) => Warnings.Add(reason);
}