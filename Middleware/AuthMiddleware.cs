using Microsoft.Extensions.Options;
using NewSky.API.Models;
using NewSky.API.Services.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NewSky.API.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ISecurityService _securityService;

        public AuthMiddleware(
            RequestDelegate next,
            ISecurityService securityService)
        {
            _next = next;
            _securityService = securityService;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                var claims = jwtToken.Claims;

                var identity = new ClaimsIdentity(claims);
                var principal = new ClaimsPrincipal(identity);
                context.User = principal;
            }

            await _next(context);
        }
    }
}
