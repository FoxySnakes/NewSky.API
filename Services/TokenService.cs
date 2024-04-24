using Microsoft.EntityFrameworkCore;
using NewSky.API.Models.Db;
using NewSky.API.Models.Result;
using NewSky.API.Services.Interface;

namespace NewSky.API.Services
{
    public class TokenService : ITokenService
    {
        private readonly IRepository<TokenInvalid> _tokenInvalidRepository;

        public TokenService(IRepository<TokenInvalid> tokenInvalidRepository)
        {
            _tokenInvalidRepository = tokenInvalidRepository;
        }

        public async Task<List<string>> GetTokensAsync()
        {
            return await _tokenInvalidRepository.Query().Select(x => x.Value).ToListAsync();
        }


        public async Task<BaseResult> CreateTokenInvalidAsync(string tokenValue)
        {
            var token = new TokenInvalid(tokenValue);
            var result = await _tokenInvalidRepository.CreateAsync(token);
            if (result.IsSuccess)
            {
                return new BaseResult();
            }
            else
            {
                var baseResult = new BaseResult();
                baseResult.Errors.Add(result.Errors.First().Message);
                return baseResult;
            }
        }
        public async Task<BaseResult> DeleteTokenInvalidAsync(string tokenValue)
        {
            var baseResult = new BaseResult();
            var token = await _tokenInvalidRepository.Query().FirstOrDefaultAsync(x => x.Value == tokenValue);
            if (token != null)
            {
                var deleteResult = await _tokenInvalidRepository.DeleteAsync(token.Id);
                if (!deleteResult.IsSuccess)
                {
                    baseResult.Errors.Add(deleteResult.Errors.First().Message);
                }
            }
            else
            {
                baseResult.Errors.Add("Ce token n'existe pas");
            }

            return baseResult;
        }
    }
}
