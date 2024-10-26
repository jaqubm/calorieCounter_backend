using calorieCounter_backend.Models;

namespace calorieCounter_backend.Repositories;

public interface IUserRepository
{
    public bool SaveChanges();
    
    public void AddEntity<T>(T entity);
    public void UpdateEntity<T>(T entity);
    public void DeleteEntity<T>(T entity);
    
    public bool UserAlreadyExist(string email);
    public User? GetUserByEmail(string email);
}