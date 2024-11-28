namespace calorieCounter_backend.Dtos;

public class UserEntryCreatorDto
{
    public string EntryType { get; set; } = string.Empty;
    public string? ProductId { get; set; }
    public string? RecipeId { get; set; }
    public DateTime Date { get; set; }
    public string MealType { get; set; } = string.Empty;
    public float? Weight { get; set; }
}
