namespace calorieCounter_backend.Repositories;

public interface IUserRepository
{
    public bool SaveChanges();
    
    public void AddEntity<T>(T entity);
    public void UpdateEntity<T>(T entity);
}