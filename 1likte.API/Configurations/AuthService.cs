using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using _1likte.Model.DbModels;
using _1likte.Model.ViewModels;
using _1likte.Model.ViewModels.Common;
using _1likte.Model.ViewModels.Common.MachineGo.Model.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace _1likte.API.Configurations
{
    public class AuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IOptions<JwtSettings> _options;

        public AuthService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IOptions<JwtSettings> options)
        {
            _httpContextAccessor = httpContextAccessor;
            _options = options;
        }

        public ValidatedModel<LoginResponseModel> WriteToken(LoginResponseModel user)
        {
            var jwtKey = _options.Value.Key;
            var token = GenerateJwtToken(jwtKey, user);

            var context = _httpContextAccessor.HttpContext;

            if (context == null)
                return new ValidatedModel<LoginResponseModel>("Geçersiz işlem");

            // Token'ı cookie'ye ekle
            context.Response.Cookies.Append("access-token", token, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true // Yalnızca HTTPS üzerinden kullanılabilir
            });

            // Token'ı header olarak ekle
            context.Response.Headers["X-Access-Token"] = token;

            return new ValidatedModel<LoginResponseModel>(user);
        }

        private string GenerateJwtToken(string key, LoginResponseModel user)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var securityKey = new SymmetricSecurityKey(keyBytes);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.FullName)
        };

            var token = new JwtSecurityToken(
                issuer: _options.Value.Issuer,
                audience: _options.Value.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_options.Value.ExpireMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
