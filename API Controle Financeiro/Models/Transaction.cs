using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControleFinanceiroAPI.Models;

[Table("transaction")]
public class Transaction
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("type")]
    [Required]
    public TransactionType Type { get; set; }

    [Column("name")]
    [MaxLength(100)]
    [Required]
    public string Name { get; set; }

    [Column("date")]
    [Required]
    public DateTime Date { get; set; }

    [Column("value", TypeName = "decimal(10,2)")]
    [Required]
    public double Value { get; set; }
}

public enum TransactionType
{
    Income,
    Outcome
}
