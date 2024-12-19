using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _1likte.Model.DbModels;
using _1likte.Model.ViewModels;
using _1likte.Model.ViewModels.User;
using AutoMapper;

namespace _1likte.Core.Mapper
{
    public class MapperProfile :Profile
    {
          public MapperProfile()
        {
            // User Mapping
            CreateMap<User, UserResponseModel>();
            CreateMap<UserRegisterRequestModel, User>();
            CreateMap<UserUpdateModel, User>();

    
            CreateMap<Token, TokenModel>();
        }
    }
}