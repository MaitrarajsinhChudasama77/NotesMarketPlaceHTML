using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Notes_MarketPlace.Models
{
    public class UserProfileModel
    {
        public int? ID { get; set; }
        public int UserID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required(ErrorMessage = "The EmailID is not a valid e-mail address.")]
        [EmailAddress]
        public string EmailID { get; set; }
        public string SecondaryEmail { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string Gender { get; set; }
        [Required]
        public string PhoneNumber_CountryCode { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public HttpPostedFileBase ProfilePicture { get; set; }
        [Required]
        public string AddressLine_1 { get; set; }
        [Required]
        public string AddressLine_2 { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string ZipCode { get; set; }
        [Required]
        public int CountryID { get; set; }
        public string University { get; set; }
        public string College { get; set; }
        public Nullable<System.DateTime> SubmittedDate { get; set; }
        public Nullable<int> SubmittedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
    }
}