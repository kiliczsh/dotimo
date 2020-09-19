using System;
using System.Collections.Generic;

namespace dotimo.Business
{
    public interface IService<T>
    {
        IEnumerable<T> GetAll();

        T GetById(Guid id);

        T Add(T entity);
    }
}