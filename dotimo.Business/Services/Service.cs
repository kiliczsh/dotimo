using dotimo.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotimo.Business.Services
{
    public class Service<T> : IService<T> where T : class, new()
    {
        private readonly IUnitOfWork<T> _unitOfWork;

        public Service(IUnitOfWork<T> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<T> CreateAsync(T t)
        {
            await _unitOfWork.GetRepository().AddAsync(t);
            await _unitOfWork.CommitAsync();
            return t;
        }

        public async Task DeleteAsync(T t)
        {
            _unitOfWork.GetRepository().Remove(t);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _unitOfWork.GetRepository().GetAllAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _unitOfWork.GetRepository().GetByIdAsync(id);
        }

        public async Task<T> GetByGuidAsync(Guid guid)
        {
            return await _unitOfWork.GetRepository().GetByGuidAsync(guid);
        }

        public async Task UpdateAsync(T t)
        {
            _unitOfWork.GetRepository().Update(t);
            await _unitOfWork.CommitAsync();
        }
    }
}