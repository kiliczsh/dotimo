using System;
using System.Threading.Tasks;

namespace dotimo.Core
{
    public interface IUnitOfWork<TEntity> : IDisposable
    {
        IRepository<TEntity> GetRepository();

        void SetRepository(IRepository<TEntity> value);

        Task<int> CommitAsync();

        void Dispose();
    }
}