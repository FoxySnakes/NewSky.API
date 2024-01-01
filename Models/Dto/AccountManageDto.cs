namespace NewSky.API.Models.Dto
{
    public class AccountManageDto
    {
        public bool Success { get; set; }

        public string? Error { get; set; }

        public AccountManageDto(bool success, string? error)
        {
            Success = success;
            Error = error;
        }
    }
}
