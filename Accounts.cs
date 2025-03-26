using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

abstract class Account : IComparable<Account>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }
    public double Money { get; set; }

    List<double>? MoneyHistory { get; set; }

    [ForeignKey("User")]
    public Guid UserId { get; set; }

    public User Owner { get; set; }

    public abstract void Deposit(double amount);
    public abstract void Withdraw(double amount);
    public int CompareTo(Account? other)
    {
        if (other == null) throw new ArgumentException("Invalid account.");
        if (other.Money == this.Money) return 0;
        return this.Money > other.Money ? 1 : -1;
    }

    public bool CanPay(double amount)
    {
        return this.Money - amount >= 0 ? true : false;
    }
}

class CommonAccount : Account
{
    public CommonAccount() { }
    public CommonAccount(User owner) : base()
    {
        this.Money = 0.0;
        this.Owner = owner;
        this.CreatedAt = DateTime.Now;
    }

    public override void Deposit(double amount)
    {
        Money += amount;
        using (var db = new ApplicationDbContext())
        {
            db.Entry(this).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
    public override void Withdraw(double amount)
    {
        Money -= amount;
        using (var db = new ApplicationDbContext())
        {
            db.Entry(this).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}

class SavingsAccount : Account
{

    public double Interest { get; set; }
    public double ComputedBonus { get; set; }


    public SavingsAccount() { }
    public SavingsAccount(User owner) : base()
    {
        this.Money = 0.0;
        this.Owner = owner;
        this.CreatedAt = DateTime.Now;
        this.Interest = Bank.savingsInterest;
    }

    public override void Deposit(double amount)
    {
        Money += amount;
        using (var db = new ApplicationDbContext())
        {
            db.Entry(this).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
    public override void Withdraw(double amount)
    {
        Money -= amount;
        using (var db = new ApplicationDbContext())
        {
            db.Entry(this).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
    public void HandleDailyInterest()
    {
        ComputedBonus += Money * Interest;
        using (var db = new ApplicationDbContext())
        {
            db.Entry(this).State = EntityState.Modified;
            db.SaveChanges();
        }
    }

    public void ApplyInterest()
    {
        Money += ComputedBonus/12;
        ComputedBonus = 0.0;
        using (var db = new ApplicationDbContext())
        {
            db.Entry(this).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}

class CreditAccount : Account
{
    public double Ceiling { get; set; }
    public double Interest { get; set; }
    public TimeSpan? InterestFreePeriod { get; set; }

    public CreditAccount() { }
    public CreditAccount(User owner, double insterest) : base()
    {
        this.Ceiling = Bank.creditAccountCeiling;
        this.Money = 0.0;
        this.Owner = owner;
        this.CreatedAt = DateTime.Now;
        this.Interest = insterest;
    }

    public override void Deposit(double amount)
    {
        Money += amount;
        using (var db = new ApplicationDbContext())
        {
            db.Entry(this).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
    public override void Withdraw(double amount)
    {
        Money -= amount;
        using (var db = new ApplicationDbContext())
        {
            db.Entry(this).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
    public new bool CanPay(double amount)
    {
        return this.Money - amount > Ceiling ? true : false;
    }
}
