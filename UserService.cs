using Microsoft.EntityFrameworkCore;

class UserService
{
    public static async Task CreateUser(string username, string name, string surname, int age, int phone, Access access, string password)
    {
        await using var db = new ApplicationDbContext();
        if (!await db.Users?.AnyAsync(u => u.Username == username))
        {
            User user = new(username, name, surname, age, phone, access, password);
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

    public static async Task DeleteUser(User user)
    {
        await using var db = new ApplicationDbContext();
        db.Users?.Remove(user);
        await db.SaveChangesAsync();
    }

    public static async Task UpdateUser(User user)
    {
        using (var db = new ApplicationDbContext())
        {
            var existingUser = await db.Users.FindAsync(user.Id);
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

    public static async Task<List<User>> GetUsers()
    {
        await using var db = new ApplicationDbContext();
        return await (db.Users?.ToListAsync() ?? Task.FromResult(new List<User>()));
    }

    public static async Task<User> GetUserByUsername(string username)
    {
        await using var db = new ApplicationDbContext();
        try
        {
            User? user = await db.Users?.FirstAsync(u => u.Username == username);

            user.CommonAccount = await db.CommonAccounts.FirstOrDefaultAsync(ca => ca.UserId == user.Id);
            user.SavingsAccount = await db.SavingsAccounts.FirstOrDefaultAsync(sa => sa.UserId == user.Id);
            user.CreditAccount = await db.CreditAccounts.FirstOrDefaultAsync(ca => ca.UserId == user.Id);

            return user;
        }
        catch (Exception)
        {
            throw new ArgumentException("User not found");
        }
    }
}
