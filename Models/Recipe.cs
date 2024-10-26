using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace calorieCounter_backend.Models;

public class Recipe
{
    [Key]
    [MaxLength(50)]
    public string Id { get; set; }

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
        Id = Guid.NewGuid().ToString();
        Name = "";
        Instructions = "";
        OwnerEmail = "";
    }

    public Recipe(string name, string instructions, string? ownerEmail)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        Instructions = instructions;
        OwnerEmail = ownerEmail;
    }
}