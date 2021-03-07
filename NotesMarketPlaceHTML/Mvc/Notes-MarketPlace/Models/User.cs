using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Notes_MarketPlace.Models
{
    public class User
    {
        public int ID { get; set; }
        public int RoleID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required(ErrorMessage = "The EmailID is not a valid e-mail address.")]
        [EmailAddress]
        public string EmailID { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,20}$", ErrorMessage = "Password must contain 8 characters with atleast One uppercase letter, lowercase letter, special character and a number.")]
        public string Password { get; set; }
        [Compare("Password",ErrorMessage ="Password and Confirm Password does not match.")]
        public string ConfirmPassword { get; set; }
        public bool IsEmailVerified { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.Guid> ActivationCode { get; set; }
    }
}