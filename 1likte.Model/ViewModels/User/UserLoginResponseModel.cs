using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _1likte.Model.ViewModels.Token;

namespace _1likte.Model.ViewModels
{
    public class UserLoginResponseModel
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string ProfilePhotoUrl { get; set; }
        public string Role { get; set; }
        
        public string AccessToken { get; set; }
        public DateTime AccessTokenExpiration { get; set; }
    }
}