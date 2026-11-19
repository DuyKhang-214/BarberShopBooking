using System.Linq.Expressions;

namespace BarberShopBooking.Repositories;

public interface IRepository<T> where T : class
{
    T? GetById(int id);
    Task<T?> GetByIdAsync(int id);
    List<T> GetAll();
    Task<List<T>> GetAllAsync();
    IQueryable<T> Query();
    void Add(T entity);
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    void SaveChanges();
    Task SaveChangesAsync();
}
