using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _1likte.Model.DbModels;
using _1likte.Model.ViewModels;
using _1likte.Model.ViewModels.Common.MachineGo.Model.ViewModel;
using _1likte.Model.ViewModels.User;

namespace _1likte.Core.Services
{
    public interface IUserService
    {
        Task<ValidatedModel<IEnumerable<UserResponseModel>>> GetAllUsersAsync();
        Task<ValidatedModel<UserResponseModel>> GetUserById(int id);
        Task<ValidatedModel<UserResponseModel>> UpdateUser(UserUpdateModel user);
    }
}