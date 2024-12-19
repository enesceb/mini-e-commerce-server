using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _1likte.Model.DbModels;
using _1likte.Model.ViewModels;

namespace _1likte.Core.Services
{
    public interface ITokenService
    {
         TokenModel GenerateToken(User user);
        int GetUserIdFromToken(string token);
    }
}