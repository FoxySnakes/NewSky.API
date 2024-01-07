using Microsoft.AspNetCore.Identity;
using NewSky.API.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace NewSky.API.Models
{
    public class User : IdentityUser
    {
        public string UUID { get; set; }
    }
}
