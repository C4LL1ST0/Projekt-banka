using Microsoft.EntityFrameworkCore;

class AccountService{
    

    public static List<SavingsAccount> GetSavingsAccounts()
    {
        using (var db = new ApplicationDbContext())
        {
            return db.SavingsAccounts.ToList();
        }
    }

    public static async Task<List<Account>> GetAllAccounts()
    {
        var accounts = new List<Account>();
        using (var db = new ApplicationDbContext())
        {
            accounts.AddRange(await db.CommonAccounts.ToListAsync());
            accounts.AddRange(await db.SavingsAccounts.ToListAsync());
            accounts.AddRange(await db.CreditAccounts.ToListAsync());
        }
        return accounts;
    }

    public static async Task<double> GetTotalMoney(){
        double total = 0.0;
        GetAllAccounts().Result.ForEach(acc => total += acc.Money);
        return total;
    }

    public static async Task<List<Account>> GetUsersAccounts(Guid userId)
    {
        var accounts = new List<Account>();
        using (var db = new ApplicationDbContext())
        {
            var commonAccount = await db.CommonAccounts.FirstOrDefaultAsync(ca => ca.UserId == userId);
            if (commonAccount != null) accounts.Add(commonAccount);

            var savingsAccount = await db.SavingsAccounts.FirstOrDefaultAsync(ca => ca.UserId == userId);
            if (savingsAccount != null) accounts.Add(savingsAccount);

            var creditAccount = await db.CreditAccounts.FirstOrDefaultAsync(ca => ca.UserId == userId);
            if (creditAccount != null) accounts.Add(creditAccount);
        }
        return accounts;
    }

    public static async Task<Account> GetAccount(string accountId)
    {
        using (var db = new ApplicationDbContext())
        {
            
            var commonAccount = await db.CommonAccounts.FindAsync(Guid.Parse(accountId));
            if (commonAccount != null) return commonAccount;

            
            var savingsAccount = await db.SavingsAccounts.FindAsync(Guid.Parse(accountId));
            if (savingsAccount != null) return savingsAccount;

            
            var creditAccount = await db.CreditAccounts.FindAsync(Guid.Parse(accountId));
            if (creditAccount != null) return creditAccount;

            
            throw new ArgumentException("Invalid account.");
        }
    }
}