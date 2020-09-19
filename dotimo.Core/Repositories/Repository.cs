using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace dotimo.Core.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        private readonly DbContext DbContext;
        private readonly DbSet<TEntity> Dbset;

        public Repository(DbContext context)
        {
            DbContext = context;
            Dbset = DbContext.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
            await Dbset.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await Dbset.AddRangeAsync(entities);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Dbset.Where(predicate);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Dbset.ToListAsync();
        }

        public ValueTask<TEntity> GetByIdAsync(int id)
        {
            return Dbset.FindAsync(id);
        }

        public void Remove(TEntity entity)
        {
            Dbset.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            Dbset.RemoveRange(entities);
        }

        public Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Dbset.SingleOrDefaultAsync(predicate);
        }
    }
}