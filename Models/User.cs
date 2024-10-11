using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace calorieCounter_backend.Models;

[Table("Users")]
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Email { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string Name { get; set; }

    public User()
    {
        Email ??= "";
        Name ??= "";
    }

    public User(string email, string name)
    {
        Email = email;
        Name = name;
    }
}