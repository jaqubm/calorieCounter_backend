using calorieCounter_backend.Models;

namespace calorieCounter_backend.Repositories;

public interface IProductRepository
{
    public bool SaveChanges();
    
    public void AddEntity<T>(T entity);
    public void UpdateEntity<T>(T entity);
    public void DeleteEntity<T>(T entity);
    
    public Product? GetProductById(string id);
}