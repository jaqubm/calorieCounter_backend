namespace calorieCounter_backend.Dtos;

public class RecipeProductDto
{
    public string ProductId { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public float Weight { get; set; }
    public float EnergyPerWeight { get; set; }
    public float ProteinPerWeight { get; set; }
    public float CarbohydratesPerWeight { get; set; }
    public float FatPerWeight { get; set; }
}
