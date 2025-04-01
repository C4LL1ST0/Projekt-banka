using t = System.Timers;
class TimeManagementService
{
    t.Timer timer;
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
    }

    public void CountTime(object? sender, t.ElapsedEventArgs e)
    {
        LastProgramDate = ProgramDate;
        ProgramDate += new TimeSpan(0, 0, 5);



        if (ProgramDate.Hour == 0 && ProgramDate.Minute == 0 && ProgramDate.Second == 0)
        {
            List<SavingsAccount> allSavingsAccounts = AccountService.GetSavingsAccounts();
            foreach (SavingsAccount sa in allSavingsAccounts) sa.HandleDailyInterest();
        }



        if (ProgramDate.Day == 1 && ProgramDate.Hour == 0 && ProgramDate.Minute == 0 && ProgramDate.Second == 0)
        {
            List<SavingsAccount> allSavingsAccounts = AccountService.GetSavingsAccounts();
            foreach (SavingsAccount sa in allSavingsAccounts) sa.ApplyInterest();
        }


    }

    public void JumpTime(int months, int days, int hours)
    {
        LastProgramDate = ProgramDate;
        DateTime FutureDate = ProgramDate.AddMonths(months).AddDays(days).AddHours(hours);
        int timeDifferenceInDays = (int)(FutureDate - LastProgramDate).TotalDays;

        if (timeDifferenceInDays == 0) return;

        List<SavingsAccount> allSavingsAccounts = AccountService.GetSavingsAccounts();

        for (int i = 0; i < timeDifferenceInDays; i++)
        {
            ProgramDate = ProgramDate.AddDays(1);

            foreach (SavingsAccount sa in allSavingsAccounts) sa.HandleDailyInterest();

            if (ProgramDate.Day == DateTime.DaysInMonth(ProgramDate.Year, ProgramDate.Month))
                foreach (SavingsAccount sa in allSavingsAccounts) sa.ApplyInterest();
        }

        ProgramDate = FutureDate;
    }
}