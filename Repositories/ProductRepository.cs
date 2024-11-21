using calorieCounter_backend.Data;
using calorieCounter_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace calorieCounter_backend.Repositories;

public class ProductRepository(IConfiguration config) : IProductRepository
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
        return await _entityFramework.User
            .Include(u => u.Products)
            .Include(u => u.Recipes)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<Product?> GetProductByIdAsync(string id)
    {
        return await _entityFramework.Product
            .Include(p => p.RecipeProducts)
            .ThenInclude(rp => rp.Recipe)
            .Include(p => p.Owner)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Product>> GetProductsByNameAsync(string name)
    {
        var queryable = _entityFramework.Product
            .Include(p => p.RecipeProducts)
            .ThenInclude(rp => rp.Recipe)
            .Include(p => p.Owner)
            .AsQueryable();

        if (!string.IsNullOrEmpty(name))
            queryable = queryable.Where(p => p.Name.StartsWith(name));

        return await queryable.Take(30).ToListAsync();
    }
}
