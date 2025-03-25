
class TimeManagementService
{
    private DateTime lastUpdateDate;
    public DateTime LastUpdateDate { get; private set; }
    private DateTime programDate;
    public DateTime ProgramDate
    {
        get
        {   
            var timeDifference = DateTime.Now - ProgramDate;
            programDate = ProgramDate.AddSeconds(timeDifference.Seconds);
            return programDate;
        }
        private set
        {
            programDate = value;
        }
    }

    public TimeManagementService()
    {
        LastUpdateDate = DateTime.Now;
        ProgramDate = DateTime.Now;
    }
}