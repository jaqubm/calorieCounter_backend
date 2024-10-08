using System.ComponentModel.DataAnnotations;

namespace calorieCounter_backend.Models;

public class User
{
    [Key]
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