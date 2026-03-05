using FinalProject.Hubs;
using FinalProject.Models.Responses;
using Microsoft.AspNetCore.SignalR;

namespace FinalProject.Services
{
    public class NotificationService(IHubContext<NotificationHub> hub) : INotificationService
    {
        public Task TaskCreatedOrUpdatedAsync(int assigneeUserId, ProjectTaskResponse task)
        {
            return hub.Clients.User(assigneeUserId.ToString())
                .SendAsync("taskUpdated", task);
        }
    }
}
