using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace calorieCounter_backend.Models;

public class UserEntry
{
    [Key]
    [MaxLength(50)]
    public string Id { get; set; }

    [Required]
    [MaxLength(255)]
    [ForeignKey("User")]
    public string UserEmail { get; set; }
    public virtual User User { get; set; }

    [Required]
    [MaxLength(10)]
    public string EntryType { get; set; }

    [MaxLength(50)]
    public string? ProductId { get; set; }
    public virtual Product? Product { get; set; }

    [MaxLength(50)]
    public string? RecipeId { get; set; }
    public virtual Recipe? Recipe { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    [MaxLength(20)]
    public string MealType { get; set; }

    public float? Weight { get; set; }
    
    public UserEntry()
    {
        Id = Guid.NewGuid().ToString();
        UserEmail = "";
        EntryType = "";
        Date = DateTime.Now;
        MealType = "";
    }

    public UserEntry(string userEmail, string entryType, DateTime date, string mealType, string? productId = null, string? recipeId = null, float? weight = null)
    {
        Id = Guid.NewGuid().ToString();
        UserEmail = userEmail;
        EntryType = entryType;
        Date = date;
        MealType = mealType;
        ProductId = productId;
        RecipeId = recipeId;
        Weight = weight;
    }
}