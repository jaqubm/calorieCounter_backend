using calorieCounter_backend.Models;

namespace calorieCounter_backend.Repositories;

public interface IRecipeRepository
{
    Task<bool> SaveChangesAsync();
    Task AddEntityAsync<T>(T entity);
    void UpdateEntity<T>(T entity);
    void DeleteEntity<T>(T entity);

    Task<User?> GetUserByIdAsync(string userId);
    Task<Recipe?> GetRecipeByIdAsync(string recipeId);
    Task<List<Recipe>> GetRecipesAsync();
    Task<List<Recipe>> SearchRecipesByNameAsync(string name);
}
