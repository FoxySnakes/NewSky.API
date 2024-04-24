using NewSky.API.Models.Db;
using NewSky.API.Models.Result;

namespace NewSky.API.Services.Interface
{
    public interface ITokenService
    {

        Task<BaseResult> CreateTokenInvalidAsync(string tokenValue);
        Task<BaseResult> DeleteTokenInvalidAsync(string tokenValue);
        Task<List<string>> GetTokensAsync();
    }
}
