using Notes_MarketPlace.Context;
using Notes_MarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Data.Entity;
using System.IO;
using System.Net;
using System.Web.Security;
using PagedList.Mvc;
using PagedList;

namespace Notes_MarketPlace.Controllers
{
    public class AdminDashboardController : Controller
    {
        NotesMarketPlaceEntities8 dbObj = new NotesMarketPlaceEntities8();
        [Authorize(Roles = "Super Admin,Admin")]
        public ActionResult Dashboard(int? i, string Search, string sortBy, string Month)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();

            ViewBag.usercount = dbObj.Users.Where(x => x.RoleID == 3).Count();
            ViewBag.NURcount = dbObj.NoteDetails.Where(x => x.Status == 2 || x.Status == 3).Include(x => x.StatusTable).Count();
            ViewBag.downloadscount = dbObj.DownloadedNotes.Where(x=>x.IsAttachmentDownloaded==true).Count();
            ViewBag.pic = dbObj.Admins.Where(x => x.AID == obj.ID).Select(x => x.ProfilePicture).FirstOrDefault();

            var filterTitle = dbObj.NoteDetails.Where(x => x.NoteTitle.Contains(Search) || Search == null);
            var filterCategory = dbObj.NoteDetails.Where(x => x.AddEditCategory.CategoryName.Contains(Search));
            var filterMonth = dbObj.NoteDetails.Where(x => x.ModifiedDate.Value.Month + "-" + x.ModifiedDate.Value.Year == Month);
            var filtereddata = filterTitle.Union(filterCategory).Union(filterMonth);
            List<SelectListItem> Monthlist = new List<SelectListItem>
            {
                new SelectListItem { Value = "1" , Text = "January" },
                new SelectListItem { Value = "2" , Text = "February" },
                new SelectListItem { Value = "3" , Text = "March" },
                new SelectListItem { Value = "4" , Text = "April" },
                new SelectListItem { Value = "5" , Text = "May" },
                new SelectListItem { Value = "6" , Text = "June" },
                new SelectListItem { Value = "7" , Text = "July" },
                new SelectListItem { Value = "8" , Text = "August" },
                new SelectListItem { Value = "9" , Text = "September" },
                new SelectListItem { Value = "10" , Text = "October" },
                new SelectListItem { Value = "11" , Text = "November" },
                new SelectListItem { Value = "12" , Text = "December" }
            };
            ViewBag.Month = Monthlist;
            
            IQueryable<NoteDetail> record;
            if (String.IsNullOrEmpty(Search) && String.IsNullOrEmpty(Month))
            {
                record = dbObj.NoteDetails.Where(x => (x.Status == 4)).Include(x => x.StatusTable).ToList().AsQueryable();
            }
            else
            {
                if (String.IsNullOrEmpty(Month)) // Dropdown is empty
                {
                    record = dbObj.NoteDetails.Where(x => (x.NoteTitle.Contains(Search)  || x.AddEditCategory.CategoryName.Contains(Search) || (x.User.FirstName + " " + x.User.LastName).Contains(Search) || Search == null) && (x.Status == 4)).Include(x => x.StatusTable).ToList().AsQueryable();
                }
                else if (String.IsNullOrEmpty(Search) && !String.IsNullOrEmpty(Month))  // Search is empty
                {
                    record = dbObj.NoteDetails.Where(x => (x.ModifiedDate.Value.Month + "-" + x.ModifiedDate.Value.Year == Month) && (x.Status == 4)).Include(x => x.StatusTable).ToList().AsQueryable();
                }
                else
                {
                    var s = dbObj.NoteDetails.Where(x => (x.NoteTitle.Contains(Search) || x.AddEditCategory.CategoryName.Contains(Search) || (x.User.FirstName + " " + x.User.LastName).Contains(Search) || Search == null) && (x.Status == 4)).Include(x => x.StatusTable).ToList().AsQueryable();
                    var d = dbObj.NoteDetails.Where(x => (x.ModifiedDate.Value.Month + "-" + x.ModifiedDate.Value.Year == Month) && (x.Status == 4)).Include(x => x.StatusTable).ToList().AsQueryable();
                    record = s.Union(d);
                }
            }
            return View(record.ToPagedList(i ?? 1, 5));
        }
    }
}