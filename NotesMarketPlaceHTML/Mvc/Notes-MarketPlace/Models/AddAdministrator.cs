using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Notes_MarketPlace.Models
{
    public class AddAdministrator
    {
        public int ID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required(ErrorMessage = "The EmailID is not a valid e-mail address.")]
        [EmailAddress]
        public string EmailID { get; set; }
        public string PhoneNumber_CountryCode { get; set; }
        public string PhoneNumber { get; set; }

        
    }
}