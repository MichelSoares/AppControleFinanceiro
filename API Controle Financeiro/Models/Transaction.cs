using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ControleFinanceiroAPI.Models;

[Table("transaction")]
public class Transaction
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //[JsonIgnore]
    [Column("id")]
    public int Id { get; set; }

    [Column("type")]
    [Required]
    public TransactionType Type { get; set; }

    [Column("name")]
    [MaxLength(100)]
    [Required]
    public string Name { get; set; }

    [Column("date", TypeName = "timestamp without time zone")]
    [DataType(DataType.Date)]
    [Required]
    public DateTime Date { get; set; }

    [Column("value", TypeName = "decimal(10,2)")]
    [Required]
    public double Value { get; set; }

    [Column("user_credential_id")]
    [Required]
    public int UserCredentialId { get; set; }

    //[Column("user_transaction")]
    //public UserCredential? UserCredential { get; set; }
}

public enum TransactionType
{
    Income = 1,
    Outcome = 2
}
