using calorieCounter_backend.Models;

namespace calorieCounter_backend.Repositories;

public interface IRecipeRepository
{
    public Task<bool> SaveChangesAsync();
    public Task AddEntityAsync<T>(T entity);
    public void UpdateEntity<T>(T entity);
    public void DeleteEntity<T>(T entity);

    public Task<User?> GetUserByIdAsync(string userId);
    
    public Task<Product?> GetProductByIdAsync(string id);
    public Task<Recipe?> GetRecipeByIdAsync(string recipeId);
    
    public Task<List<Recipe>> GetRecipesAsync();
    public Task<List<Recipe>> SearchRecipesByNameAsync(string name);
}
