using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Notes_MarketPlace.Models
{
    public class ManageSysConfigModel
    {

        [Required]
        public string SupportEmail { get; set; }
        [Required]
        public string SupportContactNumber { get; set; }
        [Required]
        public string EmailAddress_es { get; set; }
        public string FacebookURL { get; set; }
        public string TwitterURL { get; set; }
        public string LinkedInURL { get; set; }
        public HttpPostedFileBase DefaultNoteDisplayImage { get; set; }
        public HttpPostedFileBase DefaultProfilePicture { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
    }
}