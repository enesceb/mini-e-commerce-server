using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1likte.Model.DbModels
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ProfilePhotoUrl { get; set; }
    }
}