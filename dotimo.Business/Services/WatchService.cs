using dotimo.Core;
using dotimo.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotimo.Business.Services
{
    public class WatchService : IWatchService
    {
        private readonly IUnitOfWork<Watch> _unitOfWork;

        public WatchService(IUnitOfWork<Watch> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Watch> CreateAsync(Watch watch)
        {
            await _unitOfWork.GetRepository().AddAsync(watch);
            await _unitOfWork.CommitAsync();
            return watch;
        }

        public async Task DeleteAsync(Watch watch)
        {
            _unitOfWork.GetRepository().Remove(watch);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var watch = await _unitOfWork.GetRepository().GetByGuidAsync(id);
            watch.IsActive = false;
            _unitOfWork.GetRepository().Update(watch);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<Watch>> GetAllAsync()
        {
            return await _unitOfWork.GetRepository().GetAllAsync();
        }

        public IEnumerable<Watch> GetAllByUserId(Guid userId)
        {
            var watches = _unitOfWork.GetRepository().Find(w => w.UserId == userId && w.IsActive).ToList();
            return watches ?? new List<Watch>();
        }

        public async Task<Watch> GetByGuidAsync(Guid guid)
        {
            return await _unitOfWork.GetRepository().GetByGuidAsync(guid);
        }

        public async Task<Watch> GetByIdAsync(int id)
        {
            return await _unitOfWork.GetRepository().GetByIdAsync(id);
        }

        public async Task UpdateAsync(Watch watch)
        {
            _unitOfWork.GetRepository().Update(watch);
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