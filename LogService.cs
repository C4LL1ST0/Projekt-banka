
using Newtonsoft.Json;

class LogService
{
    List<LogMsg>? LogMsgs { get; set; }
    public bool useJson;
    public LogService()
    {
        LogMsgs = new List<LogMsg>();
        if(File.Exists("logs.json"))
            useJson = true;
        else
            useJson = false;
    }

    public async Task SaveLogs()
    {
        if (useJson)
        {
            string json = JsonConvert.SerializeObject(LogMsgs);
            File.WriteAllText("logs.json", json);
            LogMsgs?.Clear();
        }
        else
        {
            using (var db = new ApplicationDbContext())
            {
                if(LogMsgs == null || LogMsgs.Count == 0) return;
                db.Logs?.AddRange(LogMsgs);
                await db.SaveChangesAsync();
                LogMsgs?.Clear();
            }
        }
    }
    public void AddLog(string message)
    {
        LogMsgs?.Add(new LogMsg(message));
    }


}