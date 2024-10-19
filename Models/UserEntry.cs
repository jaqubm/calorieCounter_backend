using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace calorieCounter_backend.Models;

public class UserEntry
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    [ForeignKey("User")]
    public string UserEmail { get; set; }
    public virtual User User { get; set; }

    [Required]
    [MaxLength(10)]
    public string EntryType { get; set; }

    public int? ProductId { get; set; }
    public virtual Product? Product { get; set; }

    public int? RecipeId { get; set; }
    public virtual Recipe? Recipe { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    [MaxLength(20)]
    public string MealType { get; set; }

    public float? Weight { get; set; }
    
    public UserEntry()
    {
        UserEmail = "";
        EntryType = "";
        Date = DateTime.Now;
        MealType = "";
    }

    public UserEntry(string userEmail, string entryType, DateTime date, string mealType, int? productId = null, int? recipeId = null, float? weight = null)
    {
        UserEmail = userEmail;
        EntryType = entryType;
        Date = date;
        MealType = mealType;
        ProductId = productId;
        RecipeId = recipeId;
        Weight = weight;
    }
}