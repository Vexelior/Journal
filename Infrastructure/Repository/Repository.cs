using Application.Repository;
using Infrastructure.Data;

namespace Infrastructure.Repository;

public class Repository<TEntity>(ApplicationDbContext context) : IRepository<TEntity> where TEntity : class
{
    public async Task<IEnumerable<TEntity>> GetAll()
    {
        return await Task.FromResult(context.Set<TEntity>().AsEnumerable());
    }
    public async Task<TEntity?> GetById(int id)
    {
        return await context.Set<TEntity>().FindAsync(id);
    }
    public async Task Add(TEntity entity)
    {
        await context.Set<TEntity>().AddAsync(entity);
        await context.SaveChangesAsync();
    }
    public async Task Update(TEntity entity)
    {
        context.Set<TEntity>().Update(entity);
        await context.SaveChangesAsync();
    }
    public async Task Delete(TEntity entity)
    {
        context.Set<TEntity>().Remove(entity);
        await context.SaveChangesAsync();
    }
}
