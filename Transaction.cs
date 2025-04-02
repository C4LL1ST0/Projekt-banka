using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

class Transaction
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public DateTime Time { get; set; }


    public Guid PayerAccountId { get; set; }

    [ForeignKey("PayerAccountId")]
    public Account PayerAccount { get; set; }


    public Guid DestinationAccountId { get; set; }

    [ForeignKey("DestinationAccountId")]

    public Account DestinationAccount { get; set; }

    public double Amount { get; set; }

    public Transaction() { }
    public Transaction(Account payerAccount, Account destinationAccount, double amount)
    {
        this.Time = DateTime.Now;
        this.PayerAccount = payerAccount;
        this.PayerAccountId = payerAccount.AccountEntity.Id;
        this.DestinationAccount = destinationAccount;
        this.DestinationAccountId = destinationAccount.AccountEntity.Id;
        this.Amount = amount;
    }
}
