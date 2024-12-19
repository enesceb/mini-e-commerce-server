using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _1likte.Model.DbModels;
using _1likte.Model.ViewModels;

namespace _1likte.Core.Services
{
    public interface IUserService
    {
        LoginResponseModel Login( LoginRequest loginRequest);
        Task<User> RegisterUser(User user);
        Task<User> GetUserById(int id);
        Task<User> UpdateUser(User user);
    }
}