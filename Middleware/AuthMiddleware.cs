using Microsoft.Extensions.Options;
using NewSky.API.Models;
using NewSky.API.Services.Interface;
using System.IdentityModel.Tokens.Jwt;

namespace NewSky.API.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITokenService _tokenService;

        public AuthMiddleware(
            RequestDelegate next,
            ITokenService tokenService)
        {
            _next = next;
            _tokenService = tokenService;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                var principal = _tokenService.GetPrincipalClaims(token);
                context.User = principal;
            }

            await _next(context);
        }
    }
}
