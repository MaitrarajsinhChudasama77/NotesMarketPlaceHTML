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
    public class AdminReportsController : Controller
    {
        NotesMarketPlaceEntities8 dbObj = new NotesMarketPlaceEntities8();

        [Authorize(Roles = "Super Admin,Admin")]
        public ActionResult SpamReports(int? i,string sortBy,string Search)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();
            
            ViewBag.pic = dbObj.Admins.Where(x => x.AID == obj.ID).Select(x => x.ProfilePicture).FirstOrDefault();
            
            ViewBag.SortDate = string.IsNullOrEmpty(sortBy) ? "Date Desc" : "";
            ViewBag.SortTitle = sortBy == "Title" ? "Title Desc" : "Title";
            ViewBag.SortCategory = sortBy == "Category" ? "Category Desc" : "Category";
            ViewBag.SortReportedBy = sortBy == "ReportedBy" ? "ReportedBy Desc" : "ReportedBy";

            var record = dbObj.SpamReportsTables.Where(x=>x.NoteDetail.NoteTitle.Contains(Search) || x.User.FirstName.Contains(Search) || x.User.LastName.Contains(Search) || (x.User.FirstName + " " + x.User.LastName).Contains(Search) || x.NoteDetail.AddEditCategory.CategoryName.Contains(Search) || Search == null ).ToList().AsQueryable();

            switch (sortBy)
            {
                case "Date Desc":
                    record = record.OrderByDescending(x => x.CreatedDate);
                    break;
                case "Title":
                    record = record.OrderBy(x => x.NoteDetail.NoteTitle);
                    break;
                case "Title Desc":
                    record = record.OrderByDescending(x => x.NoteDetail.NoteTitle);
                    break;
                case "Category":
                    record = record.OrderBy(x => x.NoteDetail.AddEditCategory.CategoryName);
                    break;
                case "Category Desc":
                    record = record.OrderByDescending(x => x.NoteDetail.AddEditCategory.CategoryName);
                    break;
                case "ReportedBy":
                    record = record.OrderBy(x => (x.User.FirstName + " " + x.User.LastName));
                    break;
                case "ReportedBy Desc":
                    record = record.OrderByDescending(x => (x.User.FirstName + " " + x.User.LastName));
                    break;
                default:
                    record = record.OrderBy(x => x.CreatedDate);
                    break;
            }

            return View(record.ToPagedList(i ?? 1, 5));
        }
        public ActionResult DeleteSpamReports(int id)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();

            Context.SpamReportsTable deletespamreport = dbObj.SpamReportsTables.Where(x => x.ID == id).FirstOrDefault();
            dbObj.SpamReportsTables.Remove(deletespamreport);
            dbObj.SaveChanges();
            return RedirectToAction("SpamReports");
        }
    }
}