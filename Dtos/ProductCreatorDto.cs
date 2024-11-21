namespace calorieCounter_backend.Dtos;

public class ProductCreatorDto
{
    public string Name { get; set; } = string.Empty;
    public float ValuesPer { get; set; }
    public float Energy { get; set; }
    public float Protein { get; set; }
    public float Carbohydrates { get; set; }
    public float Fat { get; set; }
}