namespace NewSky.API.Models.Dto
{
    public class PasswordDto
    {
        public string OldPassword { get; set; } = "";
        public string? NewPassword { get; set; }
    }
}
