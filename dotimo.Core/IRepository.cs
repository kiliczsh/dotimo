using System;
using System.Collections.Generic;

namespace dotimo.Core
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();

        T GetById(Guid id);

        T Add(T entity);
    }
}