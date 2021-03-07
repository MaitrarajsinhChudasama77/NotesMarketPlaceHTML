using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Notes_MarketPlace.Models
{
    public class UserLoginModel
    {
        [Required(ErrorMessage ="The EmailID is not a valid e-mail address.")]
        [EmailAddress]
        public string EmailID { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,20}$", ErrorMessage = "Password must contain 8 characters with atleast One uppercase letter, lowercase letter, special character and a number.")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }

    }
}