using Microsoft.EntityFrameworkCore;

class ApplicationDbContext : DbContext
{
    public DbSet<User>? Users { get; set; }

    public DbSet<CommonAccountEntity> CommonAccounts { get; set; }
    public DbSet<SavingsAccountEntity> SavingsAccounts { get; set; }
    public DbSet<CreditAccountEntity> CreditAccounts { get; set; }
    public DbSet<Transaction>? Transactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=bank.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Ignore the Account class and its derived classes
        modelBuilder.Ignore<Account>();
        modelBuilder.Ignore<CommonAccount>();
        modelBuilder.Ignore<SavingsAccount>();
        modelBuilder.Ignore<CreditAccount>();

        // Configure the AccountEntity classes
        modelBuilder.Entity<CommonAccountEntity>().ToTable("CommonAccounts");
        modelBuilder.Entity<SavingsAccountEntity>().ToTable("SavingsAccounts");
        modelBuilder.Entity<CreditAccountEntity>().ToTable("CreditAccounts");

        modelBuilder.Entity<User>()
            .HasOne(u => u.CommonAccountEntity)
            .WithOne(cae => cae.Owner)
            .HasForeignKey<CommonAccountEntity>(cae => cae.UserId);

        modelBuilder.Entity<User>()
            .HasOne(u => u.SavingsAccountEntity)
            .WithOne(sae => sae.Owner)
            .HasForeignKey<SavingsAccountEntity>(sae => sae.UserId);

        modelBuilder.Entity<User>()
            .HasOne(u => u.CreditAccountEntity)
            .WithOne(crae => crae.Owner)
            .HasForeignKey<CreditAccountEntity>(cae => cae.UserId);

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
