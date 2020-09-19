using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public IEnumerable<TEntity> GetAll()
        {
            return Dbset.AsEnumerable();
        }

        public TEntity GetById(Guid id)
        {
            return Dbset.Find(id);
        }

        public TEntity Add(TEntity entity)
        {
            Dbset.Add(entity);
            return entity;
        }
    }
}