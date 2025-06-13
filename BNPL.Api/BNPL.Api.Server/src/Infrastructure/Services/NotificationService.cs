using BNPL.Api.Server.src.Application.Abstractions.Notification;

namespace BNPL.Api.Server.src.Infrastructure.Services
{
    // TODO
    public class NotificationService : INotificationService
    {
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            await Task.CompletedTask;
        }

        public async Task SendWhatsAppAsync(string phoneNumber, string message)
        {
            await Task.CompletedTask;
        }
    }
}
