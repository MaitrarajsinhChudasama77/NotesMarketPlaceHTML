using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Notes_MarketPlace.Models
{
    public class AddNotesModel
    {
        public int? ID { get; set; }
        public int UserID { get; set; }
        public int CategoryID { get; set; }
        public int TypeID { get; set; }
        public int CountryID { get; set; }
        public int Status { get; set; }
        [Required]
        public string NoteTitle { get; set; }
        public HttpPostedFileBase DisplayPicture { get; set; }
        [Required]
        public HttpPostedFileBase[] File { get; set; }
        public string NumberOfPages { get; set; }
        [Required]
        public string Description { get; set; }
        public string University { get; set; }
        public string InstituitionName { get; set; }
        public string Course { get; set; }
        public string CourseCode { get; set; }
        public string Professor { get; set; }
        public bool IsPaid { get; set; }
        public decimal SellingPrice_USD { get; set; }
        public HttpPostedFileBase NotesPreview { get; set; }
        public int ActionBy { get; set; }
        public string Remark { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public bool IsActive { get; set; }
        public int Rate { get; set; }
    }
}