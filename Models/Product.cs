using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace calorieCounter_backend.Models;

public class Product
{
    [Key]
    [MaxLength(50)]
    public string Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    public float ValuesPer { get; set; }

    [Required]
    public float Energy { get; set; }

    [Required]
    public float Protein { get; set; }

    [Required]
    public float Carbohydrates { get; set; }

    [Required]
    public float Fat { get; set; }

    [MaxLength(21)]
    [ForeignKey("Owner")]
    public string? OwnerId { get; set; }
    public virtual User? Owner { get; set; }

    public virtual List<UserEntry> UserEntries { get; set; } = [];
    public virtual List<RecipeProduct> RecipeProducts { get; set; } = [];
    
    public Product()
    {
        Id = Guid.NewGuid().ToString();
        Name = string.Empty;
        ValuesPer = 0;
        Energy = 0;
        Protein = 0;
        Carbohydrates = 0;
        Fat = 0;
    }

    public Product(string name, float valuesPer, float energy, float protein, float carbohydrates, float fat, string? ownerId)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        ValuesPer = valuesPer;
        Energy = energy;
        Protein = protein;
        Carbohydrates = carbohydrates;
        Fat = fat;
        OwnerId = ownerId;
    }
}