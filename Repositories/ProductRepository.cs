using calorieCounter_backend.Data;
using calorieCounter_backend.Models;

namespace calorieCounter_backend.Repositories;

public class ProductRepository(IConfiguration config) : IProductRepository
{
    private readonly DataContext _entityFramework = new(config);

    public bool SaveChanges()
    {
        return _entityFramework.SaveChanges() > 0;
    }

    public void AddEntity<T>(T entity)
    {
        if (entity is not null)
            _entityFramework.Add(entity);
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

    public Product? GetProductById(string id)
    {
        return _entityFramework
            .Product
            .FirstOrDefault(p => p.Id == id);
    }
}