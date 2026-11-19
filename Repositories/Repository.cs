using BarberShopBooking.Models;
using Microsoft.EntityFrameworkCore;

namespace BarberShopBooking.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext Context;
    protected readonly DbSet<T> DbSet;

    public Repository(AppDbContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
    }

    public T? GetById(int id) => DbSet.Find(id);

    public async Task<T?> GetByIdAsync(int id) => await DbSet.FindAsync(id);

    public List<T> GetAll() => DbSet.ToList();

    public async Task<List<T>> GetAllAsync() => await DbSet.ToListAsync();

    public IQueryable<T> Query() => DbSet.AsQueryable();

    public void Add(T entity) => DbSet.Add(entity);

    public async Task AddAsync(T entity) => await DbSet.AddAsync(entity);

    public void Update(T entity) => DbSet.Update(entity);

    public void Delete(T entity) => DbSet.Remove(entity);

    public void SaveChanges() => Context.SaveChanges();

    public async Task SaveChangesAsync() => await Context.SaveChangesAsync();
}
