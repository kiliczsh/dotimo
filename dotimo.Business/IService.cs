using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotimo.Business
{
    public interface IService<T>
    {
        Task<T> CreateAsync(T t);

        Task DeleteAsync(T t);

        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(int id);
    }
}