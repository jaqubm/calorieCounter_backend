using calorieCounter_backend.Models;

namespace calorieCounter_backend.Repositories;

public interface IUserEntryRepository
{
    Task<bool> SaveChangesAsync();
    Task AddEntityAsync<T>(T entity);
    void UpdateEntity<T>(T entity);
    void DeleteEntity<T>(T entity);
    Task<UserEntry?> GetUserEntryByIdAsync(string entryId);
    Task<List<UserEntry>> GetUserEntriesByUserIdAsync(string userId);
}
