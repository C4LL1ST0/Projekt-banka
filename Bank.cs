using System.Threading.Tasks;

class Bank
{
    public static readonly double savingsInterest = 0.03;
    public static readonly double savingsInterestStudent = 0.035;
    public static readonly double loanInterest = 0.052;
    public static readonly double creditAccountCeiling = -200000.0;

    public static readonly DateTime startOfInterestFreePeriod = new(2025, 4, 1);
    public static readonly DateTime endOfInterestFreePeriod = new(2025, 4, 30);
    


    public static Bank? instance;
    public string Name { get; set; }
    TimeManagementService timeManagementService;
    public LogService logService;

    private Bank() { }

    public static Bank CreateBank(string name, LogService logService)
    {
        if (instance == null)
        {
            Bank bank = new();
            bank.Name = name;
            bank.timeManagementService = new();
            bank.logService = logService;

            instance = bank;
        }
        else instance.Name = name;
        instance.logService.AddLog("Bank created ");
        return instance;
    }

    public async void InitiateBank()
    {
        await UserService.CreateUser("admin", "", "", 999, 0, Access.ADMIN, "admin", false);

        await LoginMenu();

        logService.AddLog("Bank initialized.");
    }

    public async Task LoginMenu()
    {
        Console.Clear();
        Console.WriteLine("Enter username:");
        string? username = Console.ReadLine();
        if (username == null || username == String.Empty)
        {
            Console.WriteLine("Username cannot be empty.");
            logService.AddLog("Failed username entry.");
            await LoginMenu();
        }

        Console.WriteLine("Enter password:");
        string? password = Console.ReadLine();
        User? user;
        try { user = await UserService.GetUserByUsername(username); }
        catch (ArgumentException e)
        {
            Console.WriteLine(e.Message);
            logService.AddLog("Failed to find user.");
            Thread.Sleep(2000);
            await LoginMenu();
            return;
        }

        if (user != null)
        {
            if (user.Password == password)
            {
                Console.WriteLine("Login successful.");
                logService.AddLog($"Succesful login as: {user.Username}");
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
                logService.AddLog($"Incorret password for: {user.Username}");
                await LoginMenu();
            }
        }
        else
        {
            Console.WriteLine("User not found.");
            logService.AddLog("User not found.");
        }
    }

    public void AdminMenu()
    {
        logService.AddLog("Using admin menu.");
        Console.Clear();
        Console.WriteLine("Your options are:");
        Console.WriteLine("(1) user creation");
        Console.WriteLine("(2) user deletion");
        Console.WriteLine("(3) authorizacion management");
        Console.WriteLine("(ESCAPE <key>) general menu");
        if (Console.ReadKey(true).Key == ConsoleKey.Escape) GeneralMenu();
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
            logService.AddLog("Using admin create menu.");
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
                bool isStudent = age <=18  ? true : false;
                await UserService.CreateUser(username, name, surname, age, phone, access, password, isStudent);
                Console.Clear();
                Console.WriteLine("User created.");
                logService.AddLog($"User: {username}  created");
            }
            else
            {
                Console.WriteLine("Invalid input.");
                logService.AddLog("Failed to create new user");
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
                logService.AddLog($"User: {username} deleted.");
            }
            else
            {
                Console.WriteLine("Invalid input.");
                logService.AddLog("Failed to delete user");
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
                logService.AddLog("User access level updated.");
            }
            else
            {
                Console.WriteLine("Invalid input.");
                logService.AddLog("Failed to change user access.");
                AdminMenu();
            }
        }
    }

    public void UserMenu(User user)
    {
        logService.AddLog($"Using user menu for: {user.Username}");
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
        Console.WriteLine("(ESCAPE <key>) general menu");

        if (Console.ReadKey(true).Key == ConsoleKey.Escape) GeneralMenu();

        string? input = Console.ReadLine();
        switch (input)
        {
            case "1":
                logService.AddLog("User selected: Look at accounts.");
                LookAtAccounts();
                Console.ReadLine();
                UserMenu(user);
                break;
            case "2":
                logService.AddLog("User selected: Transfer money.");
                try { TransferMoney(); }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                    logService.AddLog($"Transfer money failed: {e.Message}");
                }
                Console.WriteLine("Money successfully transferred.");
                Console.ReadLine();
                UserMenu(user);
                break;
            case "3":
                logService.AddLog("User selected: Deposit money.");
                DepositMoney();
                Console.ReadLine();
                UserMenu(user);
                break;
            case "4":
                logService.AddLog("User selected: Withdraw money.");
                try { WithdrawMoney(); }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                    logService.AddLog($"Withdraw money failed: {e.Message}");
                }
                Console.ReadLine();
                UserMenu(user);
                break;
            case "5":
                logService.AddLog("User selected: Create common account.");
                if (user.CommonAccount != null)
                {
                    Console.WriteLine("Common account already exists.");
                    logService.AddLog("Failed to create common account: already exists.");
                    Thread.Sleep(2000);
                    UserMenu(user);
                }
                user.CreateCommonAccount();
                logService.AddLog("Common account created.");
                UserMenu(user);
                break;
            case "6":
                logService.AddLog("User selected: Create savings account.");
                if (user.SavingsAccount != null)
                {
                    Console.WriteLine("Savings account already exists.");
                    logService.AddLog("Failed to create savings account: already exists.");
                    Thread.Sleep(2000);
                    UserMenu(user);
                }
                user.CreateSavingsAccount();
                logService.AddLog("Savings account created.");
                UserMenu(user);
                break;
            case "7":
                logService.AddLog("User selected: Create credit account.");
                if (user.CreditAccount != null)
                {
                    Console.WriteLine("Credit account already exists.");
                    logService.AddLog("Failed to create credit account: already exists.");
                    Thread.Sleep(2000);
                    UserMenu(user);
                }
                user.CreateCreditAccount();
                logService.AddLog("Credit account created.");
                UserMenu(user);
                break;
            default:
                logService.AddLog("Invalid input in user menu.");
                Console.WriteLine("Invalid input.");
                UserMenu(user);
                break;
        }

        async void LookAtAccounts()
        {
            logService.AddLog("User selected: Look at accounts.");
            List<Account> allAccounts = await AccountService.GetUsersAccounts(user.Id);

            if (allAccounts.Count == 0 || allAccounts == null)
            {
                Console.WriteLine("No accounts found.");
                logService.AddLog("No accounts found for user.");
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
            if (payerAccount == null || payerAccount.UserId == null)
            {
                logService.AddLog("Transfer money failed: Invalid payer account.");
                UserMenu(user);
            }
            Console.WriteLine("Enter the destination account ID:");
            IAccount destinationAccount = await AccountService.GetAccount(Console.ReadLine());
            if (destinationAccount == null || destinationAccount.UserId == null)
            {
                logService.AddLog("Transfer money failed: Invalid destination account.");
                UserMenu(user);
            }
            user.MakeTransaction(new((Account)payerAccount, (Account)destinationAccount, amount));
            logService.AddLog($"Money transferred: {amount} from {payerAccount.Id} to {destinationAccount.Id}");
        }

        void DepositMoney()
        {
            Console.WriteLine("Enter the amount you want to deposit:");
            double amount = double.Parse(Console.ReadLine());
            if (user.CommonAccount == null)
            {
                Console.WriteLine("Common account not found.");
                logService.AddLog("Deposit money failed: Common account not found.");
                UserMenu(user);
            }
            if (amount < 0)
            {
                Console.WriteLine("Invalid amount.");
                logService.AddLog("Deposit money failed: Invalid amount.");
                UserMenu(user);
            }
            user.DepositMoney(user.CommonAccount, amount);
            logService.AddLog($"Money deposited: {amount} to common account.");
        }

        void WithdrawMoney()
        {
            Console.WriteLine("Enter the amount you want to withdraw:");
            double amount;
            try { amount = double.Parse(Console.ReadLine()); }
            catch (FormatException)
            {
                Console.WriteLine("Invalid amount.");
                logService.AddLog("Withdraw money failed: Invalid amount format.");
                UserMenu(user);
                return;
            }
            if (user.CommonAccount == null)
            {
                Console.WriteLine("Common account not found.");
                logService.AddLog("Withdraw money failed: Common account not found.");
                UserMenu(user);
            }

            if (amount < 0)
            {
                Console.WriteLine("Invalid amount.");
                logService.AddLog("Withdraw money failed: Negative amount.");
                UserMenu(user);
            }

            user.WithdrawMoney(user.CommonAccount, amount);
            logService.AddLog($"Money withdrawn: {amount} from common account.");
        }
    }

    public async Task BankerMenu()
    {
        logService.AddLog("Using banker menu.");
        Console.Clear();
        Console.WriteLine("(ESCAPE <key>) general menu");
        if (Console.ReadKey(true).Key == ConsoleKey.Escape) GeneralMenu();

        Console.WriteLine("Total money: " + AccountService.GetTotalMoney().Result);
        logService.AddLog("Displayed total money.");

        Console.WriteLine();
        Console.WriteLine("All accounts:");
        List<Account> allAccounts = await AccountService.GetAllAccounts();
        foreach (Account account in allAccounts)
        {
            Console.WriteLine(account);
            logService.AddLog($"Displayed account: {account.Id}");
        }

        Console.ReadLine();
    }

    public async Task GeneralMenu()
    {
        logService.AddLog("Using general menu.");
        Console.Clear();
        Console.WriteLine("Actual time:" + timeManagementService.ProgramDate.ToString());
        Console.WriteLine();
        Console.WriteLine("Welcome to the bank.");
        Console.WriteLine("Your options are:");
        Console.WriteLine("(1) Use different account.");
        Console.WriteLine("(2) Show account types.");
        Console.WriteLine("(3) Jump in time.");
        Console.WriteLine("(4) Logs settings.");
        Console.WriteLine("(5) Exit the program.");
        string? input = Console.ReadLine();

        switch (input)
        {
            case "1":
                logService.AddLog("Selected: Use different account.");
                await LoginMenu();
                break;
            case "2":
                logService.AddLog("Selected: Show account types.");
                await AccountTypesMenu();
                break;
            case "3":
                logService.AddLog("Selected: Jump in time.");
                JumpInTimeMenu();
                break;
            case "4":
                logService.AddLog("Selected: Logs settings.");
                LogsSettingsMenu();
                break;
            case "5":
                logService.AddLog("Selected: Exit the program.");
                await logService.SaveLogs();
                Environment.Exit(0);
                break;
            default:
                logService.AddLog("Invalid input in general menu.");
                Console.WriteLine("Invalid input.");
                Thread.Sleep(2000);
                await GeneralMenu();
                break;
        }

        async Task AccountTypesMenu()
        {
            logService.AddLog("Using account types menu.");
            Console.Clear();
            Console.WriteLine("Account types:");
            Console.WriteLine("1. Common account: used for daily transactions.");
            Console.WriteLine("2. Savings account: used for saving money with interest.");
            Console.WriteLine("3. Credit account: used for borrowing money with interest.");
            Console.WriteLine("Press RETURN <key> to go back.");
            Console.ReadKey();
            await GeneralMenu();
        }

        void JumpInTimeMenu()
        {
            logService.AddLog("Using jump in time menu.");
            Console.WriteLine("Enter the number of months you want to jump in time:");
            int months = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the number of days you want to jump in time:");
            int days = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the number of hours you want to jump in time:");
            int hours = int.Parse(Console.ReadLine());
            if (hours < 0 || days < 0 || months < 0)
            {
                Console.WriteLine("Invalid input.");
                logService.AddLog("Invalid input in jump in time menu.");
                Thread.Sleep(2000);
                JumpInTimeMenu();
            }
            timeManagementService.JumpTime(months, days, hours);
            logService.AddLog($"Time jumped by {months} months, {days} days, and {hours} hours.");
        }

        void LogsSettingsMenu()
        {
            logService.AddLog("Using logs settings menu.");
            Console.Clear();
            Console.WriteLine("(1) Use database for logs storing. [default]");
            Console.WriteLine("(2) Use text file in JSON format for logs storing.");
            string? input = Console.ReadLine();
            if (input == "1")
            {
                logService.useJson = false;
                logService.AddLog("Logs set to use database.");
            }
            else if (input == "2")
            {
                logService.useJson = true;
                logService.AddLog("Logs set to use JSON file.");
            }
            else
            {
                Console.WriteLine("Invalid input.");
                logService.AddLog("Invalid input in logs settings menu.");
                Thread.Sleep(1000);
                return;
            }
        }

    }
}
