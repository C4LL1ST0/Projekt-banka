class Program
{
    public static void Main()
    {
        LogService logService = new();
        try
        {
            Bank bank = Bank.CreateBank("Ano banka", logService);
            bank.InitiateBank();
        }
        catch (Exception e)
        {
            logService.AddLog(e.Message);
            logService.SaveLogs().Wait();
        }



        Console.ReadLine();
    }
}
