using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Notes_MarketPlace.Models
{
    public class ForgotPasswordModel
    {
        [Required(ErrorMessage ="This EmailID is not valid.")]
        [EmailAddress]
        public string EmailID { get; set; }
    }
}