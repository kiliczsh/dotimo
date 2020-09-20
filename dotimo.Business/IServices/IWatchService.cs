using dotimo.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace dotimo.Business.IServices
{
    public interface IWatchService
    {
        Task<Watch> CreateAsync(Watch watch);

        Task DeleteAsync(Watch watch);

        Task DeleteByIdAsync(Guid id);

        bool Exists(Guid guid);

        IEnumerable<Watch> Find(Expression<Func<Watch, bool>> predicate);

        Task<IEnumerable<Watch>> GetAllAsync();

        IEnumerable<Watch> GetAllByUserId(Guid userId);

        Task<Watch> GetByGuidAsync(Guid id);

        Task<Watch> GetByIdAsync(int id);

        Task UpdateAsync(Watch watch);
    }
}