namespace BNPL.Api.Server.src.Application.Abstractions.Notification
{
    public interface INotificationService
    {
        Task SendEmailAsync(string to, string subject, string body);
        Task SendWhatsAppAsync(string phoneNumber, string message);
    }
}
