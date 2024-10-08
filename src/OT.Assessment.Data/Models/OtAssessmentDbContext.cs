using Microsoft.EntityFrameworkCore;

namespace OT.Assessment.Data.Models;

public partial class OtAssessmentDbContext : DbContext
{
    public OtAssessmentDbContext()
    {
    }

    public OtAssessmentDbContext(DbContextOptions<OtAssessmentDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<Wager> Wagers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__accounts__F267251E5F05B41D");

            entity.ToTable("accounts");

            entity.HasIndex(e => e.Username, "UQ__accounts__F3DBC5728A0A025F").IsUnique();

            entity.HasIndex(e => e.Username, "idx_accounts_username");

            entity.Property(e => e.AccountId)
                .ValueGeneratedNever()
                .HasColumnName("accountId");
            entity.Property(e => e.TotalAmountSpend)
                .HasColumnType("decimal(18, 6)")
                .HasColumnName("totalAmountSpend");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__transact__9B57CF72DE8BF79A");

            entity.ToTable("transactions");

            entity.HasIndex(e => e.WagerId, "idx_transactions_wagerId");

            entity.Property(e => e.TransactionId)
                .ValueGeneratedNever()
                .HasColumnName("transactionId");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(18, 6)")
                .HasColumnName("amount");
            entity.Property(e => e.CreatedDateTime)
                .HasColumnType("datetime")
                .HasColumnName("createdDateTime");
            entity.Property(e => e.GameName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("gameName");
            entity.Property(e => e.Provider)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("provider");
            entity.Property(e => e.WagerId).HasColumnName("wagerId");

            entity.HasOne(d => d.Wager).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.WagerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__transacti__wager__29572725");
        });

        modelBuilder.Entity<Wager>(entity =>
        {
            entity.HasKey(e => e.WagerId).HasName("PK__wagers__8CE78343A96896BA");

            entity.ToTable("wagers");

            entity.HasIndex(e => e.AccountId, "idx_wagers_accountId");

            entity.Property(e => e.WagerId)
                .ValueGeneratedNever()
                .HasColumnName("wagerId");
            entity.Property(e => e.AccountId).HasColumnName("accountId");
            entity.Property(e => e.BrandId).HasColumnName("brandId");
            entity.Property(e => e.CountryCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("countryCode");
            entity.Property(e => e.CreatedDateTime)
                .HasColumnType("datetime")
                .HasColumnName("createdDateTime");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.ExternalReferenceId).HasColumnName("externalReferenceId");
            entity.Property(e => e.NumberOfBets).HasColumnName("numberOfBets");
            entity.Property(e => e.SessionData)
                .HasColumnType("text")
                .HasColumnName("sessionData");
            entity.Property(e => e.Theme)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("theme");
            entity.Property(e => e.TransactionId).HasColumnName("transactionId");
            entity.Property(e => e.TransactionTypeId).HasColumnName("transactionTypeId");

            entity.HasOne(d => d.Account).WithMany(p => p.Wagers)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__wagers__accountI__267ABA7A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
