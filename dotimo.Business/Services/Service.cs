using dotimo.Core;
using System;
using System.Collections.Generic;

namespace dotimo.Business.Services
{
    public class Service<T> : IService<T> where T : class, new()
    {
        public IRepository<T> Repository { get; set; }

        public Service(IRepository<T> repository)
        {
            Repository = repository;
        }

        public T Add(T entity)
        {
            return Repository.Add(entity);
        }

        public IEnumerable<T> GetAll()
        {
            return Repository.GetAll();
        }

        public T GetById(Guid id)
        {
            return Repository.GetById(id);
        }
    }
}