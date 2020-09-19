using dotimo.Data.Models;

namespace dotimo.Business.IServices
{
    public interface INotificationService
    {
        void Send(Notification notification);
    }
}