using dotimo.Business.IServices;
using dotimo.Core;
using dotimo.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotimo.Business.Services
{
    public class CheckUpService : ICheckUpService
    {
        private readonly IUnitOfWork<CheckUp> _unitOfWork;

        public CheckUpService(IUnitOfWork<CheckUp> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CheckUp> CreateAsync(CheckUp checkUp)
        {
            checkUp.IsActive = true;
            await _unitOfWork.GetRepository().AddAsync(checkUp);
            await _unitOfWork.CommitAsync();
            return checkUp;
        }

        public async Task DeleteAsync(CheckUp checkUp)
        {
            _unitOfWork.GetRepository().Remove(checkUp);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var checkUp = await _unitOfWork.GetRepository().GetByGuidAsync(id);
            checkUp.IsActive = false;
            _unitOfWork.GetRepository().Update(checkUp);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<CheckUp>> GetAllAsync()
        {
            return await _unitOfWork.GetRepository().GetAllAsync();
        }

        public IEnumerable<CheckUp> GetAllByWatchId(Guid id)
        {
            var checkUps = _unitOfWork.GetRepository().Find(w => w.WatchId == id && w.IsActive).ToList();
            return checkUps ?? new List<CheckUp>();
        }

        public async Task<CheckUp> GetByGuidAsync(Guid guid)
        {
            return await _unitOfWork.GetRepository().GetByGuidAsync(guid);
        }

        public async Task<CheckUp> GetByIdAsync(int id)
        {
            return await _unitOfWork.GetRepository().GetByIdAsync(id);
        }

        public async Task UpdateAsync(CheckUp checkUp)
        {
            _unitOfWork.GetRepository().Update(checkUp);
            await _unitOfWork.CommitAsync();
        }

        public bool Exists(Guid guid)
        {
            try
            {
                var watch = _unitOfWork.GetRepository().Find(w => w.Id == guid);
                return watch != null;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}