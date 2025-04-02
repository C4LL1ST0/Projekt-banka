using Microsoft.EntityFrameworkCore;


abstract class Account : IComparable<Account>
{
    public IAccountEntity AccountEntity { get; set; }

    public Account(IAccountEntity accountEntity) { this.AccountEntity = accountEntity; }

    public abstract void Deposit(double amount);
    public abstract void Withdraw(double amount);
    public int CompareTo(Account? other)
    {
        if (other == null) throw new ArgumentException("Invalid account.");
        if (other.AccountEntity.Money == this.AccountEntity.Money) return 0;
        return this.AccountEntity.Money > other.AccountEntity.Money ? 1 : -1;
    }

    public bool CanPay(double amount)
    {
        return this.AccountEntity.Money - amount >= 0 ? true : false;
    }

    public abstract override string ToString();

}

class CommonAccount : Account
{
    public CommonAccount(CommonAccountEntity accountEntity) : base(accountEntity)
    {
        this.AccountEntity.Money = 0.0;
        this.AccountEntity.Owner = accountEntity.Owner;
        this.AccountEntity.CreatedAt = DateTime.Now;
    }

    public override void Deposit(double amount)
    {
        AccountEntity.Money += amount;
        using (var db = new ApplicationDbContext())
        {
            db.Entry(AccountEntity).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
    public override void Withdraw(double amount)
    {
        AccountEntity.Money -= amount;
        using (var db = new ApplicationDbContext())
        {
            db.Entry(AccountEntity).State = EntityState.Modified;
            db.SaveChanges();
        }
    }

    public override string ToString()
    {
        return $"Common account: {AccountEntity.Id}, money: {AccountEntity.Money}, created at {AccountEntity.CreatedAt}";
    }
}

class SavingsAccount : Account
{

    public double Interest { get; set; }
    public double ComputedBonus { get; set; }


    public SavingsAccount(SavingsAccountEntity accountEntity) : base(accountEntity)
    {
        this.AccountEntity.Money = 0.0;
        this.AccountEntity.Owner = accountEntity.Owner;
        this.AccountEntity.CreatedAt = DateTime.Now;
        this.Interest = Bank.savingsInterest;
    }

    public override void Deposit(double amount)
    {
        AccountEntity.Money += amount;
        using (var db = new ApplicationDbContext())
        {
            db.Entry(AccountEntity).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
    public override void Withdraw(double amount)
    {
        AccountEntity.Money -= amount;
        using (var db = new ApplicationDbContext())
        {
            db.Entry(AccountEntity).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
    public void HandleDailyInterest()
    {
        ComputedBonus += AccountEntity.Money * Interest;
        using (var db = new ApplicationDbContext())
        {
            db.Entry(AccountEntity).State = EntityState.Modified;
            db.SaveChanges();
        }
    }

    public void ApplyInterest()
    {
        AccountEntity.Money += ComputedBonus / 12;
        ComputedBonus = 0.0;
        using (var db = new ApplicationDbContext())
        {
            db.Entry(AccountEntity).State = EntityState.Modified;
            db.SaveChanges();
        }
    }

    public override string ToString()
    {
        return $"Savings account: {this.AccountEntity.Id}, money: {AccountEntity.Money}, created at {AccountEntity.CreatedAt}, interest: {Interest}";
    }
}

class CreditAccount : Account
{
    public double Ceiling { get; set; }
    public double Interest { get; set; }
    public TimeSpan? InterestFreePeriod { get; set; }

    public CreditAccount(CreditAccountEntity accountEntity, double insterest) : base(accountEntity)
    {
        this.Ceiling = Bank.creditAccountCeiling;
        this.AccountEntity.Money = 0.0;
        this.AccountEntity.Owner = accountEntity.Owner;
        this.AccountEntity.CreatedAt = DateTime.Now;
        this.Interest = insterest;
    }

    public override void Deposit(double amount)
    {
        AccountEntity.Money += amount;
        using (var db = new ApplicationDbContext())
        {
            db.Entry(AccountEntity).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
    public override void Withdraw(double amount)
    {
        AccountEntity.Money -= amount;
        using (var db = new ApplicationDbContext())
        {
            db.Entry(AccountEntity).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
    public new bool CanPay(double amount)
    {
        return this.AccountEntity.Money - amount > Ceiling ? true : false;
    }

    public override string ToString()
    {
        return $"Savings account: {this.AccountEntity.Id}, money: {AccountEntity.Money}, created at {AccountEntity.CreatedAt}, interest: {Interest}, ceiling: {Ceiling}";
    }
}
