using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

interface IAccountEntity
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }
    public double Money { get; set; }

    public Guid UserId { get; set; }
    public User Owner { get; set; }
}
class CommonAccountEntity : IAccountEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }
    public double Money { get; set; }


    [ForeignKey("User")]
    public Guid UserId { get; set; }
    public User Owner { get; set; }
}

class SavingsAccountEntity : IAccountEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }
    public double Money { get; set; }


    [ForeignKey("User")]
    public Guid UserId { get; set; }
    public User Owner { get; set; }

    public double Interest { get; set; }
    public double ComputedBonus { get; set; }
}

class CreditAccountEntity : IAccountEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }
    public double Money { get; set; }


    [ForeignKey("User")]
    public Guid UserId { get; set; }

    public User Owner { get; set; }
    public double Ceiling { get; set; }
    public double Interest { get; set; }
    public TimeSpan? InterestFreePeriod { get; set; }
}
