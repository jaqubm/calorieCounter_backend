using calorieCounter_backend.Data;
using calorieCounter_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace calorieCounter_backend.Repositories;

public class RecipeRepository(IConfiguration config) : IRecipeRepository
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

    public async Task<User?> GetUserByIdAsync(string userId)
    {
        return await _entityFramework.User.FindAsync(userId);
    }

    public async Task<Recipe?> GetRecipeByIdAsync(string recipeId)
    {
        return await _entityFramework.Recipe
            .Include(r => r.RecipeProducts)
            .FirstOrDefaultAsync(r => r.Id == recipeId);
    }

    public async Task<List<Recipe>> GetRecipesAsync()
    {
        return await _entityFramework.Recipe
            .Include(r => r.RecipeProducts)
            .Take(30)
            .ToListAsync();
    }

    public async Task<List<Recipe>> SearchRecipesByNameAsync(string name)
    {
        var queryable = _entityFramework.Recipe.AsQueryable();

        if (!string.IsNullOrEmpty(name))
            queryable = queryable.Where(r => r.Name.StartsWith(name));

        return await queryable.Take(30).ToListAsync();
    }
}
