namespace calorieCounter_backend.Dtos;

public class UserDto
{
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public float Energy  { get; set; }
    public float Protein { get; set; }
    public float Carbohydrates { get; set; }
    public float Fat  { get; set; }
}