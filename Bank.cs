using System.Threading.Tasks;

class Bank
{
    public static readonly double savingsInterest = 0.035;
    public static readonly double loanInterest = 0.052;
    public static readonly double creditAccountCeiling = -200000.0;


    public static Bank? instance;
    public string Name { get; set; }

    private Bank() { }

    public static Bank CreateBank(string name)
    {
        if (instance == null)
        {
            Bank bank = new();
            bank.Name = name;
            instance = bank;
        }
        else instance.Name = name;
        return instance;
    }

    public async void InitiateBank()
    {
        await UserService.CreateUser("admin", "", "", 999, 0, Access.ADMIN, "admin");

        await LoginMenu();
    }

    public async Task LoginMenu()
    {
        Console.Clear();
        Console.WriteLine("Enter username:");
        string? username = Console.ReadLine();
        if (username == null || username == String.Empty)
        {
            Console.WriteLine("Username cannot be empty.");
            await LoginMenu();
        }

        Console.WriteLine("Enter password:");
        string? password = Console.ReadLine();
        User? user;
        try { user = await UserService.GetUserByUsername(username); }
        catch (ArgumentException e)
        {
            Console.WriteLine(e.Message);
            Thread.Sleep(2000);
            await LoginMenu();
            return;
        }

        if (user != null)
        {
            if (user.Password == password)
            {
                Console.WriteLine("Login successful.");
                if (user.Access == Access.ADMIN)
                {
                    AdminMenu();
                }
                else if (user.Access == Access.CLIENT)
                {
                    UserMenu(user);
                }
                else
                {
                    await BankerMenu();
                }
            }
            else
            {
                Console.WriteLine("Incorrect password.");
                await LoginMenu();
            }
        }
        else
        {
            Console.WriteLine("User not found.");
        }
    }

    public void AdminMenu()
    {
        Console.Clear();
        Console.WriteLine("Your options are:");
        Console.WriteLine("(1) user creation");
        Console.WriteLine("(2) user deletion");
        Console.WriteLine("(3) authorizacion management");
        string? input = Console.ReadLine();
        if (input == "1") CreateMenu();
        else if (input == "2") DeleteMenu();
        else if (input == "3") AuthorizationMenu();
        else
        {
            Console.WriteLine("Invalid input.");
            AdminMenu();
        }
        AdminMenu();


        async void CreateMenu()
        {
            System.Console.WriteLine("Enter username:");
            string? username = Console.ReadLine();
            System.Console.WriteLine("Enter password:");
            string? password = Console.ReadLine();
            System.Console.WriteLine("Enter name:");
            string? name = Console.ReadLine();
            System.Console.WriteLine("Enter surname:");
            string? surname = Console.ReadLine();
            System.Console.WriteLine("Enter age:");
            int age = int.Parse(Console.ReadLine());
            System.Console.WriteLine("Enter phone:");
            int phone = int.Parse(Console.ReadLine());
            System.Console.WriteLine("Enter access level(ADMIN, BANKER, CLIENT):");
            Access access;
            try { access = (Access)Enum.Parse(typeof(Access), Console.ReadLine()); }
            catch (ArgumentException)
            {
                Console.WriteLine("Input valid access level.");
                Thread.Sleep(2000);
                AdminMenu();
                return;
            }

            if (username != String.Empty && password != String.Empty && name != String.Empty && surname != String.Empty)
            {
                await UserService.CreateUser(username, name, surname, age, phone, access, password);
                Console.Clear();
                Console.WriteLine("User created.");
            }
            else
            {
                Console.WriteLine("Invalid input.");
                AdminMenu();
            }
        }

        async void DeleteMenu()
        {
            Console.WriteLine("Enter username:");
            string? username = Console.ReadLine();
            if (username != String.Empty || username != null)
            {

                User userToDelete = await UserService.GetUserByUsername(username);
                if (userToDelete == null)
                {
                    Console.WriteLine("User not found.");
                    AdminMenu();
                }
                await UserService.DeleteUser(userToDelete);
                Console.Clear();
                Console.WriteLine("User deleted.");
            }
            else
            {
                Console.WriteLine("Invalid input.");
                AdminMenu();
            }
        }

        async void AuthorizationMenu()
        {
            Console.WriteLine("Enter username:");
            string username = Console.ReadLine();
            if (username != String.Empty)
            {
                User userToAuthorize = await UserService.GetUserByUsername(username);
                if (userToAuthorize == null)
                {
                    Console.WriteLine("User not found.");
                    AdminMenu();
                }
                Console.WriteLine("Enter new access level(ADMIN, BANKER, CLIENT):");
                Access newAccess = (Access)Enum.Parse(typeof(Access), Console.ReadLine());
                userToAuthorize.Access = newAccess;
                await UserService.UpdateUser(userToAuthorize);
                Console.Clear();
                Console.WriteLine("User access level updated.");
            }
            else
            {
                Console.WriteLine("Invalid input.");
                AdminMenu();
            }
        }
    }

    public void UserMenu(User user)
    {
        Console.WriteLine();
        Console.WriteLine("Welcome to your account.");
        Console.WriteLine("Your options are:");

        Console.WriteLine("(1) Look at your accounts");
        Console.WriteLine("(2) Transfer money");
        Console.WriteLine("(3) Deposit money");
        Console.WriteLine("(4) Withdraw money");
        Console.WriteLine("(5) Create common account");
        Console.WriteLine("(6) Create savings account");
        Console.WriteLine("(7) Create credit account");

        string? input = Console.ReadLine();
        switch (input)
        {
            case "1":
                LookAtAccounts();
                Console.ReadLine();
                UserMenu(user);
                break;
            case "2":
                try { TransferMoney(); }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                }
                Console.WriteLine("Money succesfully transfered");
                Console.ReadLine();
                UserMenu(user);
                break;
            case "3":
                DepositMoney();
                Console.ReadLine();
                UserMenu(user);
                break;
            case "4":
                try { WithdrawMoney(); }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                }
                Console.ReadLine();
                UserMenu(user);
                break;
            case "5":
                if (user.CommonAccount != null)
                {
                    Console.WriteLine("Common account already exists.");
                    Thread.Sleep(2000);
                    UserMenu(user);
                }
                user.CreateCommonAccount();
                UserMenu(user);
                break;
            case "6":
                if (user.SavingsAccount != null)
                {
                    Console.WriteLine("Savings account already exists.");
                    Thread.Sleep(2000);
                    UserMenu(user);
                }
                user.CreateSavingsAccount();
                UserMenu(user);
                break;
            case "7":
                if (user.CreditAccount != null)
                {
                    Console.WriteLine("Credit account already exists.");
                    Thread.Sleep(2000);
                    UserMenu(user);
                }
                user.CreateCreditAccount();
                UserMenu(user);
                break;
            default:
                Console.WriteLine("Invalid input.");
                UserMenu(user);
                break;
        }

        async void LookAtAccounts()
        {
            List<Account> allAccounts = await AccountService.GetUsersAccounts(user.Id);

            if (allAccounts.Count == 0 || allAccounts == null)
            {
                Console.WriteLine("No accounts found.");
                Thread.Sleep(2000);
                UserMenu(user);
            }
            foreach (Account account in allAccounts) Console.WriteLine(account.ToString());

        }

        async void TransferMoney()
        {
            Console.WriteLine("Enter the amount you want to transfer:");
            double amount = double.Parse(Console.ReadLine());
            Console.WriteLine("Enter your account ID:");
            IAccount payerAccount = await AccountService.GetAccount(Console.ReadLine());
            if (payerAccount == null || payerAccount.UserId == null) UserMenu(user);
            Console.WriteLine("Enter the destination account ID:");
            IAccount destinationAccount = await AccountService.GetAccount(Console.ReadLine());
            if (destinationAccount == null || destinationAccount.UserId == null) UserMenu(user);
            user.MakeTransaction(new((Account)payerAccount, (Account)destinationAccount, amount));
        }

        void DepositMoney()
        {
            Console.WriteLine("Enter the amount you want to deposit:");
            double amount = double.Parse(Console.ReadLine());
            if (user.CommonAccount == null)
            {
                Console.WriteLine("Common account not found.");
                UserMenu(user);
            }
            if (amount < 0)
            {
                Console.WriteLine("Invalid amount.");
                UserMenu(user);
            }
            user.DepositMoney(user.CommonAccount, amount);
        }

        void WithdrawMoney()
        {
            Console.WriteLine("Enter the amount you want to withdraw:");
            double amount;
            try{amount = double.Parse(Console.ReadLine());}
            catch (FormatException)
            {
                Console.WriteLine("Invalid amount.");
                UserMenu(user);
                return;
            }
            if (user.CommonAccount == null)
            {
                Console.WriteLine("Common account not found.");
                UserMenu(user);
            }
            
            if (amount < 0)
            {
                Console.WriteLine("Invalid amount.");
                UserMenu(user);
            }

            user.WithdrawMoney(user.CommonAccount, amount);
        }
    }

    public async Task BankerMenu()
    {
        Console.Clear();
        Console.WriteLine("Total money: " + AccountService.GetTotalMoney().Result);
        Console.WriteLine();
        Console.WriteLine("All accounts:");
        List<Account> allAccounts = await AccountService.GetAllAccounts();
        foreach (Account account in allAccounts)
        {
            Console.WriteLine(account);
        }

        Console.ReadLine();
    }
}
