namespace calorieCounter_backend.Dtos;

public class RecipeDto
{
    public string Name { get; set; } = string.Empty;
    public string Instructions { get; set; } = string.Empty;
    public string? OwnerId { get; set; }
    public List<RecipeProductDto> RecipeProducts { get; set; } = new();
}
