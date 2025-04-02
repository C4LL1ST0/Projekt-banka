using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

enum Access
{
    CLIENT,
    BANKER,
    ADMIN,
}
delegate Task updateUser(User user);

class User
{
    public event updateUser onUserUpdate;

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string Username { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public int Age { get; set; }
    public int Phone { get; set; }
    public Access Access { get; set; }
    public string Password { get; set; }


    public CommonAccountEntity? CommonAccountEntity { get; set; }
    public SavingsAccountEntity? SavingsAccountEntity { get; set; }
    public CreditAccountEntity? CreditAccountEntity { get; set; }

    [NotMapped]
    public CommonAccount? CommonAccount { get; set; }
    [NotMapped]
    public SavingsAccount? SavingsAccount { get; set; }
    [NotMapped]
    public CreditAccount? CreditAccount { get; set; }

    public User() { }

    public User(string username, string name, string surname, int age, int phone, Access access, string password)
    {
        this.Username = username;
        this.Name = name;
        this.Surname = surname;
        this.Age = age;
        this.Phone = phone;
        this.Access = access;
        this.Password = password;

        InitiateAccounts().Wait();
    }

    private async Task InitiateAccounts()
    {
        using (var db = new ApplicationDbContext())
        {
            db.Users.Attach(this);
            UserService.AssignAccountsToUser(this);
        }
    }

    public void CreateCommonAccount()
    {
        if (CommonAccount != null) throw new ArgumentException("Common account already exists");
        CommonAccount = AccountService.CreateCommonAccount(this);
        using (var db = new ApplicationDbContext())
        {
            db.Users.Attach(this);
            db.CommonAccounts.Add((CommonAccountEntity)CommonAccount.AccountEntity);
            db.SaveChanges();
        }
        onUserUpdate?.Invoke(this);
    }

    public void CreateSavingsAccount()
    {
        if (SavingsAccount != null) throw new ArgumentException("Savings account already exists");
        SavingsAccount = AccountService.CreateSavingsAccount(this);
        using (var db = new ApplicationDbContext())
        {
            db.Users.Attach(this);
            db.SavingsAccounts.Add((SavingsAccountEntity)SavingsAccount.AccountEntity);
            db.SaveChanges();
        }
        onUserUpdate?.Invoke(this);
    }

    public void CreateCreditAccount()
    {
        if (CreditAccount != null) throw new ArgumentException("Credit account already exists");
        CreditAccount = AccountService.CreateCreditAccount(this);
        using (var db = new ApplicationDbContext())
        {
            db.Users.Attach(this);
            db.CreditAccounts.Add((CreditAccountEntity)CreditAccount.AccountEntity);
            db.SaveChanges();
        }
        onUserUpdate?.Invoke(this);
    }

    public void DepositMoney(Account account, double money)
    {
        if (account == null) throw new ArgumentException("Invalid account.");
        account.Deposit(money);
    }

    public void WithdrawMoney(Account account, double money)
    {
        if (account == null) throw new ArgumentException("Invalid account.");
        if(account.CanPay(money))
            account.Withdraw(money);
        else throw new ArgumentException("Insufficient funds.");
    }

    public void MakeTransaction(Transaction transaction)
    {
        if (transaction.PayerAccount.CanPay(transaction.Amount))
        {
            transaction.PayerAccount.Withdraw(transaction.Amount);
            transaction.DestinationAccount.Deposit(transaction.Amount);

            using (var db = new ApplicationDbContext())
            {
                db.Entry(transaction.PayerAccount).State = EntityState.Unchanged;
                db.Entry(transaction.DestinationAccount).State = EntityState.Unchanged;

                db.Transactions.Add(transaction);
                db.SaveChanges();
            }
        }
        else throw new ArgumentException("Insufficient funds.");
    }
}
