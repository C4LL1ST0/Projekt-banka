using System.Threading.Tasks;

class Bank
{
    public static readonly double savingsInterest = 0.035;
    public static readonly double loanInterest = 0.052;
    public static readonly double creditAccountCeiling = -200000.0;

    UserService userService;

    public static Bank? instance;
    public string Name { get; set; }

    private Bank() { userService = new(); }

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
        await userService.CreateUser("admin", "", "", 999, 0, Access.ADMIN, "admin");


        await LoginMenu();
    }

    public async Task LoginMenu()
    {
        Console.Clear();
        Console.WriteLine("Enter username:");
        string username = Console.ReadLine();
        if (username == null || username == String.Empty)
        {
            Console.WriteLine("Username cannot be empty.");
            await LoginMenu();
        }

        Console.WriteLine("Enter password:");
        string password = Console.ReadLine();
        User? user = await userService.GetUserByUsername(username);
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
                    //UserMenu(user);
                }
                else
                {
                    //BankerMenu();
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
        string input = Console.ReadLine();
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
            string username = Console.ReadLine();
            System.Console.WriteLine("Enter password:");
            string password = Console.ReadLine();
            System.Console.WriteLine("Enter name:");
            string name = Console.ReadLine();
            System.Console.WriteLine("Enter surname:");
            string surname = Console.ReadLine();
            System.Console.WriteLine("Enter age:");
            int age = int.Parse(Console.ReadLine());
            System.Console.WriteLine("Enter phone:");
            int phone = int.Parse(Console.ReadLine());
            System.Console.WriteLine("Enter access level(ADMIN, BANKER, CLIENT):");
            Access access = (Access)Enum.Parse(typeof(Access), Console.ReadLine());
            if (username != String.Empty && password != String.Empty && name != String.Empty && surname != String.Empty)
            {
                await userService.CreateUser(username, name, surname, age, phone, access, password);
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
            string username = Console.ReadLine();
            if (username != String.Empty)
            {

                User userToDelete = await userService.GetUserByUsername(username);
                if (userToDelete == null)
                {
                    Console.WriteLine("User not found.");
                    AdminMenu();
                }
                await userService.DeleteUser(userToDelete);
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
                User userToAuthorize = await userService.GetUserByUsername(username);
                if (userToAuthorize == null)
                {
                    Console.WriteLine("User not found.");
                    AdminMenu();
                }
                Console.WriteLine("Enter new access level(ADMIN, BANKER, CLIENT):");
                Access newAccess = (Access)Enum.Parse(typeof(Access), Console.ReadLine());
                userToAuthorize.Access = newAccess;
                await userService.UpdateUser(userToAuthorize);
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

}
