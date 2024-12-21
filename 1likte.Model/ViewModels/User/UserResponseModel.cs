using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1likte.Model.ViewModels
{
    public class UserResponseModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ProfilePhotoUrl { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}