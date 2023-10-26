using Microsoft.AspNetCore.Identity;
using NewSky.API.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace NewSky.API.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        public string? FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        public string UUID { get; set; }

        [Required]
        public DateTime? Birthday { get; set; }

        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
