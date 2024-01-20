namespace NewSky.API.Models.Result
{
    public class AccountManageResult
    {
        public bool Success => Error == null;

        public string? Error { get; set; }

        public bool NeedDisconnect { get; set; } = false;
    }
}
