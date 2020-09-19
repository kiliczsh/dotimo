using dotimo.Data.Context;
using System.Threading.Tasks;

namespace dotimo.Core
{
    public class UnitOfWork<TEntity> : IUnitOfWork<TEntity> where TEntity : class, new()
    {
        private readonly DotimoDbContext _dbContext;

        public UnitOfWork(DotimoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private IRepository<TEntity> repository;

        public IRepository<TEntity> GetRepository()
        {
            return repository;
        }

        public void SetRepository(IRepository<TEntity> value)
        {
            repository = value;
        }

        public async Task<int> CommitAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}