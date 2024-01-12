namespace NewSky.API.Models.Result
{
    public class BaseResult
    {
        public bool Success { get; set; }

        public List<string> Errors { get; set; }
    }
}
