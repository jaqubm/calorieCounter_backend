using calorieCounter_backend.Models;

namespace calorieCounter_backend.Repositories;

public interface IProductRepository
{
    public Task<bool> SaveChangesAsync();
    
    public Task AddEntityAsync<T>(T entity);
    public void UpdateEntity<T>(T entity);
    public void DeleteEntity<T>(T entity);
    
    public Task<User?> GetUserByIdAsync(string userId);
    
    public Task<Product?> GetProductByIdAsync(string id);
    public Task<List<Product>> GetProductsByNameAsync(string name);
}