using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ControleFinanceiroAPI.Models;

[Table("user_credential")]
public class UserCredential
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [JsonIgnore]
    [Column("id")]
    //[JsonIgnore]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(100)]
    [Required]
    //[JsonIgnore]
    public string Name { get; set; }

    [Column("email")]
    [StringLength(300)]
    [Required]
    public string Email { get; set; }

    [Column("password")]
    [Required]
    public string Password { get; set; }
}
