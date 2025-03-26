
class AccountService{
    

    public List<SavingsAccount> GetSavingsAccounts()
    {
        using (var db = new ApplicationDbContext())
        {
            return db.SavingsAccounts.ToList();
        }
    }
}