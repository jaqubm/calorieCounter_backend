using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace calorieCounter_backend.Models;

public class Recipe
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    public string Instructions { get; set; }

    [MaxLength(255)]
    [ForeignKey("Owner")]
    public string? OwnerEmail { get; set; }
    public virtual User? Owner { get; set; }

    public virtual ICollection<RecipeProduct> RecipeProducts { get; set; } = new List<RecipeProduct>();
    
    public Recipe()
    {
        Name = "";
        Instructions = "";
        OwnerEmail = "";
    }

    public Recipe(string name, string instructions, string? ownerEmail)
    {
        Name = name;
        Instructions = instructions;
        OwnerEmail = ownerEmail;
    }
}