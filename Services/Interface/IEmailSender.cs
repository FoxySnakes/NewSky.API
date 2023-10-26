using NewSky.API.Models;

namespace NewSky.API.Services.Interface
{
    public interface IEmailSender
    {
        Task SendEmailConfirmationAsync(string email, User user);
    }
}
