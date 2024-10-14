using calorieCounter_backend.Data;
using calorieCounter_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace calorieCounter_backend.Repositories;

public class UserRepository(IConfiguration config) : IUserRepository
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

    public bool UserAlreadyExist(string email)
    {
        return _entityFramework.User.Any(u => u.Email == email);
    }

    public User? GetUserByEmail(string email)
    {
        return _entityFramework.User.FirstOrDefault(u => u.Email == email);
    }
}