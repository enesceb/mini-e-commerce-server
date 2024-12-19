using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace _1likte.Model.ViewModels.Common
{

    public class ErrorModel
    {
        public ErrorModel(string errorMessage)
        {
            Title = errorMessage;
        }

        [Required]
        public string Title { get; set; } = null!;
    }
}