using Microsoft.EntityFrameworkCore;

class ApplicationDbContext : DbContext
{
    public DbSet<User>? Users { get; set; }

    public DbSet<CommonAccount>? CommonAccounts { get; set; }
    public DbSet<SavingsAccount>? SavingsAccounts { get; set; }
    public DbSet<CreditAccount>? CreditAccounts { get; set; }
    public DbSet<Transaction>? Transactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=bank.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasOne(u => u.CommonAccount)
            .WithOne(ca => ca.Owner)
            .HasForeignKey<CommonAccount>(ca => ca.UserId);

        modelBuilder.Entity<User>()
            .HasOne(u => u.SavingsAccount)
            .WithOne(sa => sa.Owner)
            .HasForeignKey<SavingsAccount>(sa => sa.UserId);

        modelBuilder.Entity<User>()
            .HasOne(u => u.CreditAccount)
            .WithOne(cra => cra.Owner)
            .HasForeignKey<CreditAccount>(ca => ca.UserId);

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
