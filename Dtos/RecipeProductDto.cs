namespace calorieCounter_backend.Dtos;

public class RecipeProductDto
{
    public string RecipeId { get; set; } = string.Empty;
    public string ProductId { get; set; } = string.Empty;
    public float Weight { get; set; }
}
