using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _1likte.Model.ViewModels;

namespace _1likte.Core.Services
{
    public interface IAuthenticationService
    {
        Task<TokenModel> AuthenticateAsync(UserLoginRequestModel login);
        Task<TokenModel> RegisterAsync(UserRegisterRequestModel createUser);

        Task<bool> RevokeRefreshTokenAsync(string refreshToken);

        Task<TokenModel> RefreshTokenAsync(string refreshToken);
    }
}