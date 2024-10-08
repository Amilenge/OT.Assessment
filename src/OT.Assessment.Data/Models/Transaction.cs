namespace OT.Assessment.Data.Models;

public partial class Transaction
{
    public Guid TransactionId { get; set; }

    public Guid WagerId { get; set; }

    public string? GameName { get; set; }

    public string? Provider { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? CreatedDateTime { get; set; }

    public virtual Wager Wager { get; set; } = null!;
}
