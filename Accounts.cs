using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

interface IAccount
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User Owner { get; set; }
    public DateTime CreatedAt { get; set; }
    public double Money { get; set; }

    public abstract void Deposit(double amount);
    public abstract void Withdraw(double amount);
    public string ToString();
    public bool CanPay(double amount);
}
abstract class Account : IComparable<Account>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public double Money { get; set; }

    public void Deposit(double amount)
    {
        Money += amount;
        using (var db = new ApplicationDbContext())
        {
            db.Entry(this).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
    public void Withdraw(double amount)
    {
        Money -= amount;
        using (var db = new ApplicationDbContext())
        {
            db.Entry(this).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
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

class CommonAccount : Account, IAccount
{
    [ForeignKey("User")]
    public Guid UserId { get; set; }
    public User Owner { get; set; }
    public CommonAccount() { }
    public CommonAccount(User owner) : base()
    {
        this.Money = 0.0;
        this.Owner = owner;
        this.CreatedAt = DateTime.Now;
    }

    public override string ToString()
    {
        return $"Common account: {this.Id}, money: {Money}, created at {CreatedAt}";
    }
}

class SavingsAccount : Account, IAccount
{
    [ForeignKey("User")]
    public Guid UserId { get; set; }
    public User Owner { get; set; }

    public double Interest { get; set; }
    public double ComputedBonus { get; set; }

    public bool IsStudent{get; set;}


    public SavingsAccount() { }
    public SavingsAccount(User owner) : base()
    {
        this.Money = 0.0;
        this.Owner = owner;
        this.CreatedAt = DateTime.Now;
        this.Interest = Bank.savingsInterest;
        this.IsStudent = owner.IsStudent;
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
        Money += ComputedBonus / 12;
        ComputedBonus = 0.0;
        using (var db = new ApplicationDbContext())
        {
            db.Entry(this).State = EntityState.Modified;
            db.SaveChanges();
        }
    }

    public override string ToString()
    {
        return $"Savings account: {this.Id}, money: {Money}, created at {CreatedAt}, interest: {Interest}";
    }
}

class CreditAccount : Account, IAccount
{
    [ForeignKey("User")]
    public Guid UserId { get; set; }
    public User Owner { get; set; }

    public double ComputedMinus{get; set;}

    public double Ceiling { get; set; }
    public double Interest { get; set; }
    
    public CreditAccount() { }
    public CreditAccount(User owner, double insterest) : base()
    {
        this.Ceiling = Bank.creditAccountCeiling;
        this.Money = 0.0;
        this.Owner = owner;
        this.CreatedAt = DateTime.Now;
        this.Interest = insterest;
    }

    public new bool CanPay(double amount)
    {
        return this.Money - amount > Ceiling ? true : false;
    }

    public override string ToString()
    {
        return $"Savings account: {this.Id}, money: {Money}, created at {CreatedAt}, interest: {Interest}, ceiling: {Ceiling}";
    }

    public void HandleDailyInterest()
    {
        ComputedMinus -= Math.Abs(Money * Interest);
        using (var db = new ApplicationDbContext())
        {
            db.Entry(this).State = EntityState.Modified;
            db.SaveChanges();
        }
    }

    public void ApplyInterest()
    {
        Money += ComputedMinus / 12;
        ComputedMinus = 0.0;
        using (var db = new ApplicationDbContext())
        {
            db.Entry(this).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}
