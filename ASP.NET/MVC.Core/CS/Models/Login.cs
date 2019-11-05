using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMvcApplication.Models {
    public class Login {
        [Required(ErrorMessage = "Login is required")]
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
