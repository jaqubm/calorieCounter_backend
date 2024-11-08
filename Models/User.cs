using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace calorieCounter_backend.Models;

public class User
{
    [Key]
    [Required]
    [MaxLength(21)]
    public string Id { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string Email { get; set; }

    [Required]
    [MaxLength(255)]
    public string Name { get; set; }

    public virtual List<UserEntry> UserEntries { get; set; } = [];
    public virtual List<Product> Products { get; set; } = [];
    public virtual List<Recipe> Recipes { get; set; } = [];

    public User()
    {
        Email ??= string.Empty;
        Name ??= string.Empty;
    }

    public User(string id, string email, string name)
    {
        Id = id;
        Email = email;
        Name = name;
    }
}