using Microsoft.EntityFrameworkCore;

class AccountService
{


    public static List<SavingsAccount> GetSavingsAccounts()
    {
        using (var db = new ApplicationDbContext())
        {
            var accountEntities = db.SavingsAccounts.ToList();
            return accountEntities.Select(ae => new SavingsAccount(ae)).ToList();
        }
    }

    public static async Task<List<Account>> GetAllAccounts()
    {
        List<Account> accounts = new();
        using (var db = new ApplicationDbContext())
        {

            var commonAccountsEntities = await db.CommonAccounts.ToListAsync();
            accounts.AddRange(commonAccountsEntities.Select(cae => new CommonAccount(cae)));

            var savingsAccountsEntities = await db.SavingsAccounts.ToListAsync();
            accounts.AddRange(savingsAccountsEntities.Select(sae => new SavingsAccount(sae)));

            var creditAccountsEntities = await db.CreditAccounts.ToListAsync();
            accounts.AddRange(creditAccountsEntities.Select(cae => new CreditAccount(cae, cae.Interest)));
        }
        return accounts;
    }

    public static async Task<double> GetTotalMoney()
    {
        double total = 0.0;
        GetAllAccounts().Result.ForEach(acc => total += acc.AccountEntity.Money);
        return total;
    }

    public static async Task<List<Account>> GetUsersAccounts(Guid userId)
    {
        List<Account> accounts = new();
        using (var db = new ApplicationDbContext())
        {
            var commonAccount = await db.CommonAccounts.FirstOrDefaultAsync(cae => cae.UserId == userId);
            if (commonAccount != null) accounts.Add(new CommonAccount(commonAccount));

            var savingsAccount = await db.SavingsAccounts.FirstOrDefaultAsync(sae => sae.UserId == userId);
            if (savingsAccount != null) accounts.Add(new SavingsAccount(savingsAccount));

            var creditAccount = await db.CreditAccounts.FirstOrDefaultAsync(cae => cae.UserId == userId);
            if (creditAccount != null) accounts.Add(new CreditAccount(creditAccount, creditAccount.Interest));
        }
        return accounts;
    }

    public static async Task<Account> GetAccountById(string accountId)
    {
        using (var db = new ApplicationDbContext())
        {

            var commonAccountEntity = await db.CommonAccounts.FindAsync(Guid.Parse(accountId));
            if (commonAccountEntity != null) return new CommonAccount(commonAccountEntity);


            var savingsAccountEntity = await db.SavingsAccounts.FindAsync(Guid.Parse(accountId));
            if (savingsAccountEntity != null) return new SavingsAccount(savingsAccountEntity);


            var creditAccountEntity = await db.CreditAccounts.FindAsync(Guid.Parse(accountId));
            if (creditAccountEntity != null) return new CreditAccount(creditAccountEntity, creditAccountEntity.Interest);


            throw new ArgumentException("Invalid account.");
        }
    }

    public static CommonAccount CreateCommonAccount(User user)
    {
        CommonAccountEntity commonAccountEntity = new();
        commonAccountEntity.UserId = user.Id;
        commonAccountEntity.Money = 0.0;
        commonAccountEntity.CreatedAt = DateTime.Now;

        return new CommonAccount(commonAccountEntity);
    }

    public static SavingsAccount CreateSavingsAccount(User user)
    {
        SavingsAccountEntity savingsAccountEntity = new();
        savingsAccountEntity.UserId = user.Id;
        savingsAccountEntity.Money = 0.0;
        savingsAccountEntity.CreatedAt = DateTime.Now;
        savingsAccountEntity.Interest = Bank.savingsInterest;
        savingsAccountEntity.ComputedBonus = 0.0;

        return new SavingsAccount(savingsAccountEntity);
    }

    public static CreditAccount CreateCreditAccount(User user)
    {
        CreditAccountEntity creditAccountEntity = new();
        creditAccountEntity.UserId = user.Id;
        creditAccountEntity.Money = 0.0;
        creditAccountEntity.CreatedAt = DateTime.Now;
        creditAccountEntity.Interest = Bank.loanInterest;
        creditAccountEntity.Ceiling = Bank.creditAccountCeiling;


        return new CreditAccount(creditAccountEntity, creditAccountEntity.Interest);
    }
    
}