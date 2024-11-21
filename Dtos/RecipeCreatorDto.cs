namespace calorieCounter_backend.Dtos;

public class RecipeCreatorDto
{
    public string Name { get; set; } = string.Empty;
    public string Instructions { get; set; } = string.Empty;
    public IEnumerable<RecipeProductCreatorDto> ProductsList { get; set; } = [];
}
