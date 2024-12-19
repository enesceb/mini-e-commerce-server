using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1likte.Model.ViewModels
{
    public class LoginResponseModel
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
    }
}