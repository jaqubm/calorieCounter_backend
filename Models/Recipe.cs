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

    [MaxLength(21)]
    [ForeignKey("Owner")]
    public string? OwnerId { get; set; }
    public virtual User? Owner { get; set; }

    public virtual List<UserEntry> UserEntries { get; set; } = [];
    public virtual List<RecipeProduct> RecipeProducts { get; set; } = [];
    
    public Recipe()
    {
        Id = Guid.NewGuid().ToString();
        Name = string.Empty;
        Instructions = string.Empty;
        OwnerId = string.Empty;
    }

    public Recipe(string name, string instructions, string? ownerId)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        Instructions = instructions;
        OwnerId = ownerId;
    }
}