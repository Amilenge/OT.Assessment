namespace OT.Assessment.Data.Models;

public partial class Wager
{
    public Guid WagerId { get; set; }

    public string? Theme { get; set; }

    public Guid TransactionId { get; set; }

    public Guid? BrandId { get; set; }

    public Guid AccountId { get; set; }

    public Guid? ExternalReferenceId { get; set; }

    public Guid? TransactionTypeId { get; set; }

    public DateTime? CreatedDateTime { get; set; }

    public int? NumberOfBets { get; set; }

    public string? CountryCode { get; set; }

    public string? SessionData { get; set; }

    public long? Duration { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
