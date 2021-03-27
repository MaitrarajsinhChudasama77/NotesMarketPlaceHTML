using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Notes_MarketPlace.Models
{
    public class ChangePasswordModel
    {
        [Required]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,20}$", ErrorMessage = "Password must contain 8 characters with atleast One uppercase letter, lowercase letter, special character and a number.")]
        public string OldPassword { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,20}$", ErrorMessage = "Password must contain 8 characters with atleast One uppercase letter, lowercase letter, special character and a number.")]
        public string NewPassword { get; set; }
        [Required]
        [Compare("NewPassword", ErrorMessage = "New Password and Confirm Password does not match.")]
        public string ConfirmPassword { get; set; }
    }
}