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
    public class ProfileController : Controller
    {
        NotesMarketPlaceEntities8 dbObj = new NotesMarketPlaceEntities8();
        
        [Authorize(Roles = "User")]
        [HttpGet]
        [Route("Profile/MyDownloads")]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult MyDownloads(int? i, string Search, string sortBy)
        {
            ViewBag.SortDate = string.IsNullOrEmpty(sortBy) ? "Date Desc" : "";
            ViewBag.SortNoteTitle = sortBy == "Title" ? "Title Desc" : "Title";
            ViewBag.SortCategory = sortBy == "Category" ? "Category Desc" : "Category";
            ViewBag.SortSeller = sortBy == "Seller" ? "Seller Desc" : "Seller";
            var filterTitle = dbObj.DownloadedNotes.Where(x => x.NoteTitle.Contains(Search) || Search == null);
            var filterCategory = dbObj.DownloadedNotes.Where(x => x.Category.Contains(Search) || Search == null);
            var filtereddata = filterTitle.Union(filterCategory);

            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();
            var record = dbObj.DownloadedNotes.Where(x => x.NoteTitle.Contains(Search) || x.Category.Contains(Search) || x.User.FirstName.Contains(Search) || x.User.LastName.Contains(Search) || (x.User.FirstName + " " + x.User.LastName).Contains(Search) || Search == null).ToList().AsQueryable();
            record = record.Where(x => x.Downloader == obj.ID).ToList().AsQueryable();

            switch (sortBy)
            {
                case "Date Desc":
                    record = record.OrderByDescending(x => x.AttachmentDownloadedDate);
                    break;
                case "Title":
                    record = record.OrderBy(x => x.NoteTitle);
                    break;
                case "Title Desc":
                    record = record.OrderByDescending(x => x.NoteTitle);
                    break;
                case "Category":
                    record = record.OrderBy(x => x.Category);
                    break;
                case "Category Desc":
                    record = record.OrderByDescending(x => x.Category);
                    break;
                case "Seller":
                    record = record.OrderBy(x => (x.User.FirstName + x.User.LastName));
                    break;
                case "Seller Desc":
                    record = record.OrderByDescending(x => (x.User.FirstName + x.User.LastName));
                    break;
                default:
                    record = record.OrderBy(x => x.AttachmentDownloadedDate);
                    break;
            }
            ViewBag.pic = dbObj.UserProfiles.Where(x => x.UserID == obj.ID).Select(x => x.ProfilePicture).FirstOrDefault();
            return View(record.ToPagedList(i ?? 1, 10));
        }
        [Route("Profile/DownloadNotes")]
        public ActionResult DownloadNotes(int? id)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User buyerobj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();
            Context.DownloadedNote downloads = dbObj.DownloadedNotes.Where(x => x.ID == id).FirstOrDefault();
            var NoteID = downloads.NoteID;
            Context.NoteDetail noteobj = dbObj.NoteDetails.Where(x => x.ID == NoteID).FirstOrDefault();
            Context.SellerNotesAttachment seller_notes = dbObj.SellerNotesAttachments.Where(x => x.NoteID == NoteID).FirstOrDefault();

            if (downloads.HasSellerAllowedDownload == true)
            {
                
                Response.ContentType = "application/pdf";
                Response.AppendHeader("Content-Disposition", "attachment; filename =" + noteobj.NoteTitle + ".pdf");
                Response.TransmitFile(seller_notes.FilePath);
                Response.End();
                downloads.NoteID = noteobj.ID;
                downloads.Seller = noteobj.UserID;
                downloads.Downloader = buyerobj.ID;
                downloads.HasSellerAllowedDownload = true;
                downloads.AttachmentPath = seller_notes.FilePath;
                downloads.IsAttachmentDownloaded = true;
                downloads.AttachmentDownloadedDate = DateTime.Now;
                downloads.IsPaid = noteobj.IsPaid;
                downloads.PurchasedPrice = noteobj.SellingPrice_USD;
                downloads.NoteTitle = noteobj.NoteTitle;
                downloads.Category = noteobj.AddEditCategory.CategoryName;

                dbObj.Entry(downloads).State = System.Data.Entity.EntityState.Modified;
                dbObj.SaveChanges();
            }
            return RedirectToAction("MyDownloads","Profile");
        }
        [HttpPost]
        public ActionResult RateNotes(int ID, string Comment, int Rate)
        {

            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();
            DownloadedNote downloads = dbObj.DownloadedNotes.Where(x => x.NoteID == ID && x.Downloader == obj.ID).FirstOrDefault();
            var review = dbObj.NoteReviews.Where(x=>x.ReviewedByID == obj.ID).FirstOrDefault();
            if(review == null)
            {
                NoteReview note_review = new NoteReview();
                note_review.NoteID = ID;
                note_review.AgainstDownloadsID = downloads.ID;
                note_review.ReviewedByID = obj.ID;
                note_review.Ratings = Rate;
                note_review.Comments = Comment;
                note_review.CreatedDate = DateTime.Now;
                note_review.CreatedBy = obj.ID;
                note_review.IsActive = true;

                dbObj.NoteReviews.Add(note_review);
                dbObj.SaveChanges();
                var book = dbObj.NoteDetails.Where(x => x.ID == ID).FirstOrDefault();
                book.Review = dbObj.NoteReviews.Where(x => x.NoteID == ID).Count();
                book.Star = dbObj.NoteReviews.Where(x => x.NoteID == ID).Sum(x=>x.Ratings);
                dbObj.Entry(book).State = System.Data.Entity.EntityState.Modified;
                dbObj.SaveChanges();
            }
            else
            {
                review.Ratings = Rate;
                review.Comments = Comment;
                review.ModifiedDate = DateTime.Now;
                review.ModifiedBy = obj.ID;
                dbObj.Entry(review).State = System.Data.Entity.EntityState.Modified;
                dbObj.SaveChanges();
                var book = dbObj.NoteDetails.Where(x => x.ID == ID).FirstOrDefault();
                book.Review = dbObj.NoteReviews.Where(x => x.NoteID == ID).Count();
                book.Star = dbObj.NoteReviews.Where(x => x.NoteID == ID).Sum(x => x.Ratings);
                dbObj.Entry(book).State = System.Data.Entity.EntityState.Modified;
                dbObj.SaveChanges();
            }

            return RedirectToAction("MyDownloads", "Profile");
        }
        [HttpPost]
        public ActionResult MarkInappropriate(int id, string Remark)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();
            DownloadedNote downloads = dbObj.DownloadedNotes.Where(x => x.NoteID == id && x.Downloader == obj.ID).FirstOrDefault();
            ViewBag.notetitle = downloads.NoteTitle;

            var report = dbObj.SpamReportsTables.Where(x => x.ReportedByID == obj.ID).FirstOrDefault();
            if (report == null)
            {
                SpamReportsTable spam_reports = new SpamReportsTable();
                spam_reports.NoteID = id;
                spam_reports.AgainstDownloadsID = downloads.ID;
                spam_reports.ReportedByID = obj.ID;
                spam_reports.Remarks = Remark;
                spam_reports.CreatedDate = DateTime.Now;
                spam_reports.CreatedBy = obj.ID;
                dbObj.SpamReportsTables.Add(spam_reports);
                dbObj.SaveChanges();
            }
            else
            {
                report.Remarks = Remark;
                report.ModifiedDate = DateTime.Now;
                report.ModifiedBy = obj.ID;
                dbObj.Entry(report).State = System.Data.Entity.EntityState.Modified;
                dbObj.SaveChanges();
            }
            var fromEmail = new MailAddress("thehamojha@gmail.com"); //Email of Company
            var toEmail = new MailAddress("maitrarajsinhchudasama05799@gmail.com"); //Admin
            var fromEmailPassword = "244466666"; // Replace with actual password
            string subject = obj.FirstName + " Reported an issue for " + downloads.NoteTitle;

            string body = "Hello, Admins" +
                "<br/><br/>We want to inform you that, " + obj.FirstName + "  Reported an issue for " + downloads.User.FirstName + "’s Note with " +
                "title " + downloads.NoteTitle + ". Please look at the notes and take required actions." +

                "<br/><br/><br/>Regards," +
                "<br/>Notes Marketplace.";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
            return RedirectToAction("MyDownloads", "Profile");
        }
        [Authorize(Roles="User")]
        [HttpGet]
        [Route("Profile/MySoldNotes")]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult MySoldNotes(int? i, string Search, string sortBy)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();

            ViewBag.SortDate = string.IsNullOrEmpty(sortBy) ? "Date Desc" : "";
            ViewBag.SortTitle = sortBy == "Title" ? "Title Desc" : "Title";
            ViewBag.SortCategory = sortBy == "Category" ? "Category Desc" : "Category";
            ViewBag.SortBuyer = sortBy == "Buyer" ? "Buyer Desc" : "Buyer";
            ViewBag.pic = dbObj.UserProfiles.Where(x => x.UserID == obj.ID).Select(x => x.ProfilePicture).FirstOrDefault();
            
            var record = dbObj.DownloadedNotes.Where(x => x.NoteTitle.Contains(Search) || x.Category.Contains(Search) || x.User.EmailID.Contains(Search) || Search == null).ToList().AsQueryable();
            record = record.Where(x => x.Seller == obj.ID).ToList().AsQueryable();
            
            switch (sortBy)
            {
                case "Date Desc":
                    record = record.OrderByDescending(x => x.AttachmentDownloadedDate);
                    break;
                case "Title":
                    record = record.OrderBy(x => x.NoteTitle);
                    break;
                case "Title Desc":
                    record = record.OrderByDescending(x => x.NoteTitle);
                    break;
                case "Category":
                    record = record.OrderBy(x => x.Category);
                    break;
                case "Category Desc":
                    record = record.OrderByDescending(x => x.Category);
                    break;
                case "Buyer":
                    record = record.OrderBy(x => x.User.EmailID);
                    break;
                case "Buyer Desc":
                    record = record.OrderByDescending(x => x.User.EmailID);
                    break;
                default:
                    record = record.OrderBy(x => x.AttachmentDownloadedDate);
                    break;
            }

            return View(record.ToPagedList(i ?? 1, 10));
        }
        [Authorize(Roles="User")]
        [HttpGet]
        [Route("MyRejectedNotes/UserProfile")]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult MyRejectedNotes(int? i,string Search, string sortBy)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();
            
            ViewBag.SortTitle = sortBy == "Title" ? "Title Desc" : "Title";
            ViewBag.SortCategory = sortBy == "Category" ? "Category Desc" : "Category";
            ViewBag.pic = dbObj.UserProfiles.Where(x => x.UserID == obj.ID).Select(x => x.ProfilePicture).FirstOrDefault();
            
            var record = dbObj.NoteDetails.Where(x=>x.NoteTitle.Contains(Search) || x.AddEditCategory.CategoryName.Contains(Search) || x.Remark.Contains(Search) ||  Search == null).ToList().AsQueryable();
            record = record.Where(x => x.UserID == obj.ID && x.Status == 5).Include(x => x.StatusTable).ToList().AsQueryable();

            switch (sortBy)
            {
                case "Title":
                    record = record.OrderBy(x => x.NoteTitle);
                    break;
                case "Title Desc":
                    record = record.OrderByDescending(x => x.NoteTitle);
                    break;
                case "Category":
                    record = record.OrderBy(x => x.AddEditCategory.CategoryName);
                    break;
                case "Category Desc":
                    record = record.OrderByDescending(x => x.AddEditCategory.CategoryName);
                    break;
                default:
                    record = record.OrderBy(x => x.ModifiedDate);
                    break;
            }

            return View(record.ToPagedList(i ?? 1, 10));
        }
        [HttpGet]
        [Authorize(Roles="User")]
        [Route("Profile/UserProfile")]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult UserProfile()
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();
            UserProfileModel proobj = new UserProfileModel();
            var gender = new List<string>() { "Male", "Female", "Other" };
            var oldobj = dbObj.UserProfiles.Where(x => x.UserID == obj.ID).FirstOrDefault();

            if (oldobj != null)
            {
                proobj.UserID = obj.ID;
                proobj.FirstName = obj.FirstName;
                proobj.LastName = obj.LastName;
                proobj.EmailID = obj.EmailID;

                UserProfile upobj = dbObj.UserProfiles.Where(x => x.UserID == obj.ID).FirstOrDefault();
                proobj.DateOfBirth = upobj.DateOfBirth;
                proobj.Gender = upobj.Gender;
                proobj.PhoneNumber_CountryCode = upobj.PhoneNumber_CountryCode;
                proobj.PhoneNumber = upobj.PhoneNumber;
                proobj.AddressLine_1 = upobj.AddressLine_1;
                proobj.AddressLine_2 = upobj.AddressLine_2;
                proobj.City = upobj.City;
                proobj.State = upobj.State;
                proobj.ZipCode = upobj.ZipCode;
                proobj.CountryID = upobj.CountryID;
                proobj.University = upobj.University;
                proobj.College = upobj.College;

                ViewBag.list = gender;
                ViewBag.PhoneNumber_CountryCode = new SelectList(dbObj.AddEditCountries, "CountryCode", "CountryCode");//Value,text
                ViewBag.Country = new SelectList(dbObj.AddEditCountries, "ID", "CountryName");//Value,text
                ViewBag.pic = dbObj.UserProfiles.Where(x => x.UserID == obj.ID).Select(x => x.ProfilePicture).FirstOrDefault();
                return View(proobj);
            }
            else
            {
                proobj.UserID = obj.ID;
                proobj.FirstName = obj.FirstName;
                proobj.LastName = obj.LastName;
                proobj.EmailID = obj.EmailID;

                ViewBag.list = gender;
                ViewBag.PhoneNumber_CountryCode = new SelectList(dbObj.AddEditCountries, "CountryCode", "CountryCode");//Value,text
                ViewBag.Country = new SelectList(dbObj.AddEditCountries, "ID", "CountryName");//Value,text
                ViewBag.pic = dbObj.UserProfiles.Where(x => x.UserID == obj.ID).Select(x => x.ProfilePicture).FirstOrDefault();
                return View(proobj);
            }
        }
        [HttpPost]
        [Route("Profile/UserProfile")]
        public ActionResult UserProfile(UserProfileModel model)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();
            if (ModelState.IsValid)
            {
                var oldobj = dbObj.UserProfiles.Where(x => x.UserID == obj.ID).FirstOrDefault();

                string path = Path.Combine(Server.MapPath("~/Members"), obj.ID.ToString());

                //Checking for directory

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (oldobj == null) // User adding profile details first time
                {
                    //Saving into database

                    UserProfile userobj = new UserProfile();
                    //addnoteobj.ID = model.ID;
                    userobj.UserID = obj.ID;
                    userobj.DateOfBirth = model.DateOfBirth;
                    userobj.Gender = model.Gender;
                    userobj.PhoneNumber_CountryCode = model.PhoneNumber_CountryCode;
                    userobj.PhoneNumber = model.PhoneNumber;
                    userobj.AddressLine_1 = model.AddressLine_1;
                    userobj.AddressLine_2 = model.AddressLine_2;
                    userobj.City = model.City;
                    userobj.State = model.State;
                    userobj.ZipCode = model.ZipCode;
                    userobj.CountryID = model.CountryID;
                    userobj.University = model.University;
                    userobj.College = model.College;
                    userobj.SubmittedDate = DateTime.Now;
                    userobj.SubmittedBy = obj.ID;
                    dbObj.UserProfiles.Add(userobj);
                    dbObj.SaveChanges();

                    string finalpath = Path.Combine(Server.MapPath("Members/" + obj.ID));

                    if (!Directory.Exists(finalpath))
                    {
                        Directory.CreateDirectory(finalpath);
                    }

                    if (model.ProfilePicture != null && model.ProfilePicture.ContentLength > 0)
                    {
                        var displayimagename = "DP_" + DateTime.Now.ToString("dd-MM-yyyy").Replace(':', '-').Replace(' ', '_') + Path.GetExtension(model.ProfilePicture.FileName);
                        var ImageSavePath = Path.Combine(Server.MapPath("~/Members/" + obj.ID + "/") +  displayimagename);
                        model.ProfilePicture.SaveAs(ImageSavePath);
                        userobj.ProfilePicture = Path.Combine(("Members/" + obj.ID + "/"), displayimagename);
                        dbObj.SaveChanges();
                    }
                    else
                    {
                        userobj.ProfilePicture = "/Default/Joker.jpg";
                        dbObj.SaveChanges();
                    }
                }
                else //editing old user profile
                {
                    //Saving into database

                    UserProfile edituserobj = dbObj.UserProfiles.Where(x => x.UserID == obj.ID).FirstOrDefault();
                    Context.User userobj1 = dbObj.Users.Where(x => x.ID == obj.ID).FirstOrDefault();
                    
                    userobj1.FirstName = model.FirstName;
                    userobj1.LastName = model.LastName;
                    userobj1.EmailID = model.EmailID;
                    
                    edituserobj.DateOfBirth = model.DateOfBirth;
                    edituserobj.Gender = model.Gender;
                    edituserobj.PhoneNumber_CountryCode = model.PhoneNumber_CountryCode;
                    edituserobj.PhoneNumber = model.PhoneNumber;
                    edituserobj.AddressLine_1 = model.AddressLine_1;
                    edituserobj.AddressLine_2 = model.AddressLine_2;
                    edituserobj.City = model.City;
                    edituserobj.State = model.State;
                    edituserobj.ZipCode = model.ZipCode;
                    edituserobj.CountryID = model.CountryID;
                    edituserobj.University = model.University;
                    edituserobj.College = model.College;
                    edituserobj.ModifiedDate = DateTime.Now;
                    edituserobj.ModifiedBy = obj.ID;
                    dbObj.Entry(edituserobj).State = System.Data.Entity.EntityState.Modified;
                    dbObj.Entry(userobj1).State = System.Data.Entity.EntityState.Modified;
                    dbObj.SaveChanges();

                    string finalpath = Path.Combine(Server.MapPath("Members/" + obj.ID));

                    if (!Directory.Exists(finalpath))
                    {
                        Directory.CreateDirectory(finalpath);
                    }

                    if (model.ProfilePicture != null && model.ProfilePicture.ContentLength > 0)
                    {
                        var displayimagename = "DP_" + DateTime.Now.ToString("dd-MM-yyyy").Replace(':', '-').Replace(' ', '_') + Path.GetExtension(model.ProfilePicture.FileName);
                        var ImageSavePath = Path.Combine(Server.MapPath("~/Members/" + obj.ID + "/") + displayimagename);
                        model.ProfilePicture.SaveAs(ImageSavePath);
                        edituserobj.ProfilePicture = Path.Combine(("Members/" + obj.ID + "/"), displayimagename);
                        dbObj.SaveChanges();
                    }
                    else
                    {
                        edituserobj.ProfilePicture = "Default/Joker.jpg";
                        dbObj.SaveChanges();
                    }
                }
                return RedirectToAction("SearchNotes", "Home");
            }
            var gender = new List<string>() { "Male", "Female", "Other" };
            ViewBag.list = gender;
            ViewBag.PhoneNumber_CountryCode = new SelectList(dbObj.AddEditCountries, "CountryCode", "CountryCode");//Value,text
            ViewBag.Country = new SelectList(dbObj.AddEditCountries, "ID", "CountryName");//Value,text
            return View();
        }
    }
}