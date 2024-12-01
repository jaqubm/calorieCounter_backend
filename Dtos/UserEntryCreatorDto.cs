namespace calorieCounter_backend.Dtos;

public class UserEntryCreatorDto
{
    public string EntryType { get; set; } = string.Empty;
    public string EntryId { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string MealType { get; set; } = string.Empty;
    public float? Weight { get; set; }
}