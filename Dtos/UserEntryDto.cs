namespace calorieCounter_backend.Dtos;

public class UserEntryDto
{
    public string Id { get; set; } = string.Empty;
    public string EntryType { get; set; } = string.Empty;
    public string? EntryId { get; set; }
    public string? EntryName { get; set; }
    public DateTime Date { get; set; }
    public string MealType { get; set; } = string.Empty;
    public float? Weight { get; set; }
}