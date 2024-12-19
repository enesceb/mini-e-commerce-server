using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace _1likte.Model.DbModels
{
    public class Base
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}