using System.Threading.Tasks;

namespace dotimo.Business.IServices
{
    public interface IHangfireService
    {
        Task CreateRecurringJobsAsync();
    }
}