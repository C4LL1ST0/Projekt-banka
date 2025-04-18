using Microsoft.EntityFrameworkCore;

class ApplicationDbContext : DbContext
{
    public DbSet<User>? Users { get; set; }

    public DbSet<CommonAccount>? CommonAccounts { get; set; }
    public DbSet<SavingsAccount>? SavingsAccounts { get; set; }
    public DbSet<CreditAccount>? CreditAccounts { get; set; }
    public DbSet<Transaction>? Transactions { get; set; }
    public DbSet<LogMsg>? Logs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=bank.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CommonAccount>().ToTable("CommonAccounts");
        modelBuilder.Entity<SavingsAccount>().ToTable("SavingsAccounts");
        modelBuilder.Entity<CreditAccount>().ToTable("CreditAccounts");

        modelBuilder.Entity<LogMsg>().ToTable("Logs");

        modelBuilder.Entity<CommonAccount>()
            .HasOne(ca => ca.Owner)
            .WithOne(u => u.CommonAccount)
            .HasForeignKey<CommonAccount>(ca => ca.UserId);

        modelBuilder.Entity<SavingsAccount>()
            .HasOne(sa => sa.Owner)
            .WithOne(u => u.SavingsAccount)
            .HasForeignKey<SavingsAccount>(sa => sa.UserId);

        modelBuilder.Entity<CreditAccount>()
            .HasOne(cra => cra.Owner)
            .WithOne(u => u.CreditAccount)
            .HasForeignKey<CreditAccount>(cra => cra.UserId);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.PayerAccount)
            .WithMany()
            .HasForeignKey(t => t.PayerAccountId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.DestinationAccount)
            .WithMany()
            .HasForeignKey(t => t.DestinationAccountId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
