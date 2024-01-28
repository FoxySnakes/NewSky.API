namespace NewSky.API.Models.Dto
{
    public class UpdateUserPunishmentDto
    {
        public string Username { get; set; }

        public DateTime? BanishmentEnd { get; set; }

        public DateTime? LockoutEnd { get; set; }
    }
}
