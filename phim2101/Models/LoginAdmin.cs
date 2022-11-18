using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace phim2101.Models
{
    public class LoginAdmin
    {
        [Required]
        public string email { get; set; }

        [Required]
        public string password { get; set; }
    }
}