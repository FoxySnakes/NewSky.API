using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NewSky.API.Models.Result
{
    public class BaseResult
    {
        public bool Success => Errors.Count == 0;

        public List<string> Errors { get; set; } = new List<string>();
    }
}
