using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMvcApplication.Models {
    [VerifyUserNameAndPassword]
    public class Login {
        [Required(ErrorMessage = "Login is required")]
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class VerifyUserNameAndPasswordAttribute : ValidationAttribute {
        public VerifyUserNameAndPasswordAttribute() {}
        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            SecurityProvider securityProvider = (SecurityProvider)validationContext.GetService(typeof(SecurityProvider));
            Login login = (Login)value;
            if (securityProvider.InitConnection(login.UserName, login.Password)) {
                return ValidationResult.Success;
            }
            return new ValidationResult("User name or password is incorrect");
        }
    }
}
