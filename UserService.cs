using Microsoft.EntityFrameworkCore;

class UserService
{
    public async Task CreateUser(string username, string name, string surname, int age, int phone, Access access, string password)
    {
        await using var db = new ApplicationDbContext();
        if (!await db.Users?.AnyAsync(u => u.Username == username))
        {
            User user = new(username, name, surname, age,  phone, access, password);
            user.onUserUpdate += UpdateUser;
            db.Users?.Add(user);
            await db.SaveChangesAsync();
            db.Entry(user).State = EntityState.Detached;
        }
        else
        {
            Console.WriteLine("User already exists.");
        }
    }

    public async Task DeleteUser(User user)
    {
        await using var db = new ApplicationDbContext();
        db.Users?.Remove(user);
        await db.SaveChangesAsync();
    }

    public async Task UpdateUser(User user)
    {
        using (var db = new ApplicationDbContext())
        {
            var existingUser = await db.Users.FindAsync(user.UserId);
            if (existingUser != null)
            {
                db.Entry(existingUser).CurrentValues.SetValues(user);
                await db.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("User not found.");
            }
        }
    }

    public async Task<List<User>> GetUsers()
    {
        await using var db = new ApplicationDbContext();
        return await (db.Users?.ToListAsync() ?? Task.FromResult(new List<User>()));
    }

    public async Task<User> GetUserByUsername(string username)
    {
        await using var db = new ApplicationDbContext();
        try
        {
            User? user = await db.Users?.FirstAsync(u => u.Username == username);

            async Task InitiateAccounts()
            {
                using (var db = new ApplicationDbContext())
                {
                    user.CommonAccount = await db.CommonAccounts.FirstOrDefaultAsync(ca => ca.UserId == user.UserId);
                    user.SavingsAccount = await db.SavingsAccounts.FirstOrDefaultAsync(sa => sa.UserId == user.UserId);
                    user.CreditAccount = await db.CreditAccounts.FirstOrDefaultAsync(ca => ca.UserId == user.UserId);
                }
            }

            InitiateAccounts().Wait();
            return user;
        }
        catch (Exception)
        {
            throw new ArgumentException("User not found");
        }
    }
}
