using t = System.Timers;
class TimeManagementService
{
    t.Timer timer;
    AccountService accountService;
    DateTime LastProgramDate { get; set; }
    public DateTime ProgramDate { get; private set; }

    public TimeManagementService()
    {
        timer = new();
        timer.Interval = 5000;
        timer.Elapsed += CountTime;
        timer.Start();

        LastProgramDate = DateTime.Now;
        ProgramDate = DateTime.Now;

        accountService = new();
    }

    public void CountTime(object? sender, t.ElapsedEventArgs e)
    {
        LastProgramDate = ProgramDate;
        ProgramDate += new TimeSpan(0, 0, 5);

        List<SavingsAccount> allSavingsAccounts = accountService.GetSavingsAccounts();

        if (ProgramDate.Hour == 0 && ProgramDate.Minute == 0 && ProgramDate.Second == 0)
            foreach (SavingsAccount sa in allSavingsAccounts) sa.HandleDailyInterest();
        

        if (ProgramDate.Day == 1 && ProgramDate.Hour == 0 && ProgramDate.Minute == 0 && ProgramDate.Second == 0)
            foreach (SavingsAccount sa in allSavingsAccounts) sa.ApplyInterest();

    }

    public void JumpTime(int days, int hours, int minutes)
    {
        LastProgramDate = ProgramDate;
        ProgramDate = ProgramDate.AddDays(days).AddHours(hours).AddMinutes(minutes);
        int timeDifferenceInDays = (int)(ProgramDate - LastProgramDate).TotalDays;
        List<SavingsAccount> allSavingsAccounts = accountService.GetSavingsAccounts();

        if (timeDifferenceInDays == 0) return;

        for (int i = 0; i < timeDifferenceInDays; i++)
            foreach (SavingsAccount sa in allSavingsAccounts) sa.HandleDailyInterest();
        

        if (ProgramDate.Month != LastProgramDate.Month)
            foreach (SavingsAccount sa in allSavingsAccounts) sa.ApplyInterest();
        
    }
}