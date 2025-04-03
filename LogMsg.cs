using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

class LogMsg{

    [Key]
    public Guid Id{ get; private set; }
    public string Command{ get; private set; }
    public DateTime Time{ get; private set; }

    public LogMsg(string command)
    {
        Id = Guid.NewGuid();
        Command = command;
        Time = DateTime.Now;
    }
}