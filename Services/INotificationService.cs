using FinalProject.Models.Responses;

namespace FinalProject.Services
{
    public interface INotificationService
    {
        Task TaskCreatedOrUpdatedAsync(int assigneeUserId, ProjectTaskResponse task);
    }
}
