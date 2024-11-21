namespace calorieCounter_backend.Dtos;

public class RecipeDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Instructions { get; set; } = string.Empty;
    public float TotalWeight { get; set; }
    public float TotalEnergy { get; set; }
    public float TotalProtein { get; set; }
    public float TotalCarbohydrates { get; set; }
    public float TotalFat { get; set; }
    public List<RecipeProductDto> RecipeProducts { get; set; } = [];
}
