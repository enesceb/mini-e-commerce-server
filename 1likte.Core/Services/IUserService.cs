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
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> RegisterUser(User user);
        Task<User> GetUserById(int id);
        Task<User> UpdateUser(UserUpdateModel user);
    }
}