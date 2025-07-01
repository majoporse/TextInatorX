using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Database.EF;

public class Repository<TEntity> where TEntity : class
{
    internal ImageProcessorDbContext context;
    internal DbSet<TEntity> dbSet;

    public Repository(ImageProcessorDbContext context)
    {
        this.context = context;
        dbSet = context.Set<TEntity>();
    }

    public virtual IQueryable<TEntity> Get(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "")
    {
        IQueryable<TEntity> query = dbSet;

        if (filter != null) query = query.Where(filter);

        foreach (var includeProperty in includeProperties.Split
                     (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            query = query.Include(includeProperty);

        if (orderBy != null)
            return orderBy(query);
        return query;
    }

    public virtual ValueTask<TEntity?> GetByIDAsync(object id)
    {
        return dbSet.FindAsync(id);
    }

    public virtual async Task Insert(TEntity entity)
    {
        await dbSet.AddAsync(entity);
    }

    public virtual async Task Delete(object id)
    {
        var entityToDelete = await dbSet.FindAsync(id);
        Delete(entityToDelete);
    }

    public virtual void Delete(TEntity entityToDelete)
    {
        dbSet.Remove(entityToDelete);
    }

    public virtual void Update(TEntity entityToUpdate)
    {
        dbSet.Attach(entityToUpdate);
        context.Entry(entityToUpdate).State = EntityState.Modified;
    }

    public virtual async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}