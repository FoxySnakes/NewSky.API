
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using NewSky.API.Models;
using NewSky.API.Services.Interface;
using System.Text;

namespace NewSky.API.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly UserManager<User> _userManager;

        public EmailSender(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        private string _brevoApiKey = "6dy0h7m1GQNDSEHx";

        public EmailSender()
        {
        }

        public async Task SendEmailConfirmationAsync(string email, User user)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));


            throw new NotImplementedException();
        }
    }
}
