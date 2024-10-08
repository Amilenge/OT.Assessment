namespace OT.Assessment.Data.Models;

public partial class Account
{
    public Guid AccountId { get; set; }

    public string? Username { get; set; }

    public decimal? TotalAmountSpend { get; set; }

    public virtual ICollection<Wager> Wagers { get; set; } = new List<Wager>();
}
