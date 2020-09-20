using dotimo.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace dotimo.Business.IServices
{
    public interface ICheckUpService
    {
        Task<CheckUp> CreateAsync(CheckUp checkUp);

        Task DeleteAsync(CheckUp checkUp);

        Task DeleteByIdAsync(Guid id);

        bool Exists(Guid guid);

        Task<IEnumerable<CheckUp>> GetAllAsync();

        IEnumerable<CheckUp> GetAllByWatchId(Guid id);

        Task<CheckUp> GetByGuidAsync(Guid id);

        Task<CheckUp> GetByIdAsync(int id);

        Task UpdateAsync(CheckUp checkUp);
    }
}
