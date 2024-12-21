using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace _1likte.Model.DbModels
{
        public class User : Base
        {
        
            [Required]
            [MaxLength(50)]
            public string FullName { get; set; }

            [Required]
            [EmailAddress]
            [MaxLength(100)]
            public string Email { get; set; }

            [Required]
            [MaxLength(255)]
            public string PasswordHash { get; set; }


            public string ProfilePhotoUrl { get; set; }

             public string Role { get; set; } = "User";
        }
}