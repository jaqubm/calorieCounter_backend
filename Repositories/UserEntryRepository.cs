using calorieCounter_backend.Data;
using calorieCounter_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace calorieCounter_backend.Repositories;

public class UserEntryRepository(IConfiguration config) : IUserEntryRepository
{
    private readonly DataContext _entityFramework = new(config);

    public async Task<bool> SaveChangesAsync()
    {
        return await _entityFramework.SaveChangesAsync() > 0;
    }

    public async Task AddEntityAsync<T>(T entity)
    {
        if (entity is not null)
            await _entityFramework.AddAsync(entity);
    }

    public void UpdateEntity<T>(T entity)
    {
        if (entity is not null)
            _entityFramework.Update(entity);
    }

    public void DeleteEntity<T>(T entity)
    {
        if (entity is not null)
            _entityFramework.Remove(entity);
    }

    public async Task<UserEntry?> GetUserEntryByIdAsync(string entryId)
    {
        return await _entityFramework.UserEntry
            .Include(ue => ue.Product)
            .Include(ue => ue.Recipe)
            .FirstOrDefaultAsync(ue => ue.Id == entryId);
    }

    public async Task<List<UserEntry>> GetUserEntriesByUserIdAsync(string userId)
    {
        return await _entityFramework.UserEntry
            .Where(ue => ue.UserId == userId)
            .Include(ue => ue.Product)
            .Include(ue => ue.Recipe)
            .ToListAsync();
    }
}
