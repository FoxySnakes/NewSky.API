namespace NewSky.API.Models.Result
{
    public class AccountManageResult
    {
        public bool Success { get; set; }

        public string? Error { get; set; }

        public bool NeedDisconnect { get; set; } = false;

        public AccountManageResult(bool success, string? error)
        {
            Success = success;
            Error = error;
        }
    }
}
