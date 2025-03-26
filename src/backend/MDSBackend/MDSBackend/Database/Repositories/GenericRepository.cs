using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace MDSBackend.Database.Repositories;

public class GenericRepository<TEntity> where TEntity : class
{
    internal ApplicationContext context;
    internal DbSet<TEntity> dbSet;

    public GenericRepository(ApplicationContext context)
    {
        this.context = context;
        this.dbSet = context.Set<TEntity>();
    }

    public virtual IQueryable<TEntity> Get(
        Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        string includeProperties = "")
    {
        IQueryable<TEntity> query = dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties.Split
                     (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        if (orderBy != null)
        {
            return orderBy(query);
        }
        else
        {
            return query;
        }
    }

    public virtual TEntity? GetByID(object id)
    {
        return dbSet.Find(id);
    }
    public async virtual Task<TEntity?> GetByIDAsync(object id)
    {
        return await dbSet.FindAsync(id);
    }
    public virtual void Insert(TEntity entity)
    {
        dbSet.Add(entity);
    }
    public virtual void InsertRange(IQueryable<TEntity> entities)
    {
        dbSet.AddRange(entities);
    }
    public async virtual Task InsertAsync(TEntity entity)
    {
        await dbSet.AddAsync(entity);
    }
    public virtual void Delete(object id)
    {
        TEntity? entityToDelete = dbSet.Find(id);

        if (entityToDelete == null)
        {
            // It's probably a good idea to throw an error here
            // but I'm leaving it as is for now
            return;
        }

        Delete(entityToDelete);
    }

    
    public virtual void Delete(TEntity entityToDelete)
    {
        if (context.Entry(entityToDelete).State == EntityState.Detached)
        {
            dbSet.Attach(entityToDelete);
        }
        dbSet.Remove(entityToDelete);
    }

    public virtual void Update(TEntity entityToUpdate)
    {
        dbSet.Attach(entityToUpdate);
        context.Entry(entityToUpdate).State = EntityState.Modified;
    }
}
