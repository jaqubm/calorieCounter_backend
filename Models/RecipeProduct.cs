using System.ComponentModel.DataAnnotations;

namespace calorieCounter_backend.Models;

public class RecipeProduct
{
    public int RecipeId { get; set; }
    public virtual Recipe Recipe { get; set; }

    public int ProductId { get; set; }
    public virtual Product Product { get; set; }

    [Required]
    public float Weight { get; set; }
    
    public RecipeProduct()
    {
        Weight = 0;
    }

    public RecipeProduct(int recipeId, int productId, float weight)
    {
        RecipeId = recipeId;
        ProductId = productId;
        Weight = weight;
    }
}