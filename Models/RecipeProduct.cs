using System.ComponentModel.DataAnnotations;

namespace calorieCounter_backend.Models;

public class RecipeProduct
{
    [MaxLength(50)]
    public string RecipeId { get; set; }
    public virtual Recipe Recipe { get; set; }

    [MaxLength(50)]
    public string ProductId { get; set; }
    public virtual Product Product { get; set; }

    [Required]
    public float Weight { get; set; }
    
    public RecipeProduct()
    {
        Weight = 0;
    }

    public RecipeProduct(string recipeId, string productId, float weight)
    {
        RecipeId = recipeId;
        ProductId = productId;
        Weight = weight;
    }
}