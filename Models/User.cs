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
    
    [Required]
    public float Energy { get; set; }

    [Required]
    public float Protein { get; set; }

    [Required]
    public float Carbohydrates { get; set; }

    [Required]
    public float Fat { get; set; }

    public virtual List<UserEntry> UserEntries { get; set; } = [];
    public virtual List<Product> Products { get; set; } = [];
    public virtual List<Recipe> Recipes { get; set; } = [];

    public User()
    {
        Email ??= string.Empty;
        Name ??= string.Empty;
        Energy = 2000.0f;
        Protein = 90.0f;
        Carbohydrates = 210.0f;
        Fat = 60.0f;
    }
}