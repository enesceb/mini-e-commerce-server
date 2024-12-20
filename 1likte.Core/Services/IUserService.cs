using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _1likte.Model.DbModels;
using _1likte.Model.ViewModels;
using _1likte.Model.ViewModels.User;

namespace _1likte.Core.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponseModel>> GetAllUsersAsync();
        Task<UserResponseModel> GetUserById(int id);
        Task<UserResponseModel> UpdateUser(UserUpdateModel user);
    }
}