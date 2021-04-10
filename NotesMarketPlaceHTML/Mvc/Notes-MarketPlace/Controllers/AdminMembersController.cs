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
    public class AdminMembersController : Controller
    {
        NotesMarketPlaceEntities8 dbObj = new NotesMarketPlaceEntities8();
        [Authorize(Roles = "Super Admin,Admin")]
        public ActionResult Members(int? i, string sortBy,string Search)
        {
            ViewBag.SortDate = string.IsNullOrEmpty(sortBy) ? "Date Desc" : "";
            ViewBag.SortFname = sortBy == "First Name" ? "First Name Desc" : "First Name";
            ViewBag.SortLname = sortBy == "Last Name" ? "Last Name Desc" : "Last Name";
            ViewBag.SortEmail = sortBy == "Email" ? "Email Desc" : "Email";

            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();

            ViewBag.pic = dbObj.Admins.Where(x => x.AID == obj.ID).Select(x => x.ProfilePicture).FirstOrDefault();

            var record = dbObj.Users.Where(x => x.FirstName.Contains(Search) || x.LastName.Contains(Search) || (x.FirstName + " " + x.LastName).Contains(Search) || x.EmailID.Contains(Search) ||  Search == null).ToList().AsQueryable();
            record = record.Where(x => x.RoleID == 3).ToList().AsQueryable();
            switch (sortBy)
            {
                case "Date Desc":
                    record = record.OrderByDescending(x => x.CreatedDate);
                    break;
                case "First Name":
                    record = record.OrderBy(x => x.FirstName);
                    break;
                case "First Name Desc":
                    record = record.OrderByDescending(x => x.FirstName);
                    break;
                case "Last Name":
                    record = record.OrderBy(x => x.LastName);
                    break;
                case "Last Name Desc":
                    record = record.OrderByDescending(x => x.LastName);
                    break;
                case "Email":
                    record = record.OrderBy(x => x.EmailID);
                    break;
                case "Email Desc":
                    record = record.OrderByDescending(x => x.EmailID);
                    break;
                default:
                    record = record.OrderBy(x => x.CreatedDate);
                    break;
            }

            return View(record.ToPagedList(i ?? 1, 5));
        }
        [Authorize(Roles = "Super Admin,Admin")]
        public ActionResult MemberDetails(int? id,int? i, string sortBy)
        {
            ViewBag.SortDate = string.IsNullOrEmpty(sortBy) ? "Date Desc" : "";
            ViewBag.SortTitle = sortBy == "Title" ? "Title Desc" : "Title";
            ViewBag.SortCategory = sortBy == "Category" ? "Category Desc" : "Category";
            ViewBag.PublishedDate = string.IsNullOrEmpty(sortBy) ? "PublishedDate Desc" : "";

            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();

            ViewBag.pic = dbObj.Admins.Where(x => x.AID == obj.ID).Select(x => x.ProfilePicture).FirstOrDefault();

            var note = dbObj.NoteDetails.Where(x => x.UserID == id).ToList().AsQueryable();
            var profile = dbObj.UserProfiles.Where(x => x.UserID == id).FirstOrDefault();
            var user = dbObj.Users.Where(x => x.ID == id).FirstOrDefault();

            switch (sortBy)
            {
                case "Date Desc":
                    note = note.OrderByDescending(x => x.CreatedDate);
                    break;
                case "PublishedDate Desc":
                    note = note.OrderByDescending(x => x.ModifiedDate);
                    break;
                case "Title":
                    note = note.OrderBy(x => x.NoteTitle);
                    break;
                case "Title Desc":
                    note = note.OrderByDescending(x => x.NoteTitle);
                    break;
                case "Category":
                    note = note.OrderBy(x => x.AddEditCategory.CategoryName);
                    break;
                case "Category Desc":
                    note = note.OrderByDescending(x => x.AddEditCategory.CategoryName);
                    break;
                default:
                    note = note.OrderBy(x => x.CreatedDate);
                    break;
            }
            MemberDetailsModel mobj = new MemberDetailsModel();
            mobj.Note = note.ToPagedList(i ?? 1, 5);
            mobj.User = user;
            mobj.Profile = profile;
            return View(mobj);
        }
        public ActionResult DeactivateMember(int id)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();

            Context.User deleteuser = dbObj.Users.Where(x => x.ID == id).FirstOrDefault();

            if (deleteuser.IsActive == true)
            {
                deleteuser.IsActive = false;
                deleteuser.ModifiedDate = DateTime.Now;
                deleteuser.ModifiedBy = obj.ID;

                dbObj.Entry(deleteuser).State = System.Data.Entity.EntityState.Modified;
                dbObj.SaveChanges();
            }
            else
            {
                deleteuser.IsActive = true;
                deleteuser.ModifiedDate = DateTime.Now;
                deleteuser.ModifiedBy = obj.ID;

                dbObj.Entry(deleteuser).State = System.Data.Entity.EntityState.Modified;
                dbObj.SaveChanges();
            }
            return RedirectToAction("Members");
        }
    }
}