namespace calorieCounter_backend.Dtos;

public class ProductDto
{
    public string Name { get; set; } = "";
    public float ValuesPer { get; set; }
    public float Energy { get; set; }
    public float Protein { get; set; }
    public float Carbohydrates { get; set; }
    public float Fat { get; set; }
    public string? OwnerId { get; set; }
}