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
    public class AdminNotesController : Controller
    {
        NotesMarketPlaceEntities8 dbObj = new NotesMarketPlaceEntities8();
        [Authorize(Roles = "Super Admin,Admin")]
        public ActionResult ViewNotes(int id)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();

            ViewBag.pic = dbObj.Admins.Where(x => x.AID == obj.ID).Select(x => x.ProfilePicture).FirstOrDefault();
            ViewBag.Buyer = obj.FirstName;
            ViewBag.flagcount = dbObj.SpamReportsTables.Where(x => x.NoteID == id).Select(x => x.NoteID).Count();

            var totalreview = dbObj.NoteDetails.Where(x => x.ID == id).FirstOrDefault().Review;
            var totalstars = dbObj.NoteDetails.Where(x => x.ID == id).FirstOrDefault().Star;
            if (totalreview == null)
            {
                ViewBag.totalreviews = 0;
                ViewBag.Rating = 0;
            }
            else
            {
                ViewBag.totalreviews = totalreview;
                ViewBag.Rating = (totalstars / totalreview) * 20;
            }
            Context.NoteDetail record = dbObj.NoteDetails.Where(x => x.ID == id).FirstOrDefault();
            return View(record);
        }
        public ActionResult NotesDownload(int id)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User buyerobj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();

            SellerNotesAttachment sellerattachobj = dbObj.SellerNotesAttachments.Where(x => x.NoteID == id).FirstOrDefault();
            NoteDetail noteobj = dbObj.NoteDetails.Where(x => x.ID == id).FirstOrDefault();
            Context.User selleruserobj = dbObj.Users.Where(x => x.ID == noteobj.UserID).FirstOrDefault();
            
            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename =" + noteobj.NoteTitle + ".pdf");
            Response.TransmitFile(sellerattachobj.FilePath);
            Response.End();

            return RedirectToAction("ViewNotes", "AdminNotes", new { id = noteobj.ID });
        }

        public ActionResult DeleteReview(int id, int note)
        {
            var r = dbObj.NoteReviews.Where(x => x.ID == id).FirstOrDefault();
            var book = dbObj.NoteDetails.Where(x => x.ID == note).FirstOrDefault();

            dbObj.NoteReviews.Remove(r);
            dbObj.SaveChanges();

            int tr = dbObj.NoteReviews.Where(x => x.NoteID == note).Count();

            if(tr == 0)
            {
                book.Review = 0;
                book.Star = 0;
            }
            else
            {
                int ts = dbObj.NoteReviews.Where(x => x.NoteID == note).Sum(x => x.Ratings);
                book.Review = tr;
                book.Star = ts;
            }

            dbObj.Entry(book).State = System.Data.Entity.EntityState.Modified;
            dbObj.SaveChanges();

            return RedirectToAction("ViewNotes", "AdminNotes", new { id = note });
        }
        [Authorize(Roles = "Super Admin,Admin")]
        public ActionResult DownloadedNotes(int? i, string Search_Keyword, string sortBy,string Seller,string Buyer, string Note)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();
            
            //var record = dbObj.DownloadedNotes.Where(x => x.IsAttachmentDownloaded==true && x.NoteTitle.Contains(Search_Keyword) || Search_Keyword == null).ToList().AsQueryable();

            ViewBag.SortDate = string.IsNullOrEmpty(sortBy) ? "Date Desc" : "";
            ViewBag.SortTitle = sortBy == "Title" ? "Title Desc" : "Title";
            ViewBag.SortCategory = sortBy == "Category" ? "Category Desc" : "Category";
            ViewBag.SortBuyer = sortBy == "Buyer" ? "Buyer Desc" : "Buyer";
            ViewBag.pic = dbObj.Admins.Where(x => x.AID == obj.ID).Select(x => x.ProfilePicture).FirstOrDefault();

            ViewBag.Note = new SelectList(dbObj.DownloadedNotes.Where(x => x.NoteDetail.IsActive == true && x.HasSellerAllowedDownload == true).Select(x => x.NoteTitle).Distinct().ToList());
            ViewBag.Seller = new SelectList(dbObj.DownloadedNotes.Where(x => x.NoteDetail.IsActive == true && x.HasSellerAllowedDownload == true).Select(x => x.User1.FirstName + " " + x.User1.LastName).Distinct().ToList());
            ViewBag.Buyer = new SelectList(dbObj.DownloadedNotes.Where(x=>x.HasSellerAllowedDownload==true).Select(x => x.User.FirstName + " " + x.User.LastName).Distinct().ToList());

            IQueryable<DownloadedNote> record;
            if (String.IsNullOrEmpty(Search_Keyword) && String.IsNullOrEmpty(Seller) && String.IsNullOrEmpty(Buyer) && String.IsNullOrEmpty(Note))
            {
                record = dbObj.DownloadedNotes.Where(x => x.IsAttachmentDownloaded==true).ToList().AsQueryable();
            }
            else
            {
                if (String.IsNullOrEmpty(Seller) && String.IsNullOrEmpty(Buyer) && String.IsNullOrEmpty(Note)) // Dropdown is empty
                {
                    record = dbObj.DownloadedNotes.Where(x => (x.NoteTitle.Contains(Search_Keyword) || x.User.FirstName.Contains(Search_Keyword) || x.User.LastName.Contains(Search_Keyword) || (x.User.FirstName + " " + x.User.LastName).Contains(Search_Keyword) || (x.User1.FirstName + " " + x.User1.LastName).Contains(Search_Keyword) || x.User1.FirstName.Contains(Search_Keyword) || x.User1.LastName.Contains(Search_Keyword) || x.Category.Contains(Search_Keyword) || Search_Keyword == null) && (x.IsAttachmentDownloaded==true)).ToList().AsQueryable();
                }
                else if (String.IsNullOrEmpty(Search_Keyword) && (!String.IsNullOrEmpty(Seller) || !String.IsNullOrEmpty(Buyer) || !String.IsNullOrEmpty(Note)))  // Search is empty
                {
                    var seller = dbObj.DownloadedNotes.Where(x => x.User1.FirstName + " " + x.User1.LastName==Seller && x.HasSellerAllowedDownload == true).ToList().AsQueryable();
                    var buyer = dbObj.DownloadedNotes.Where(x => x.User.FirstName + " " + x.User.LastName==Buyer && x.HasSellerAllowedDownload == true).ToList().AsQueryable();
                    var note = dbObj.DownloadedNotes.Where(x => x.NoteTitle==Note && x.HasSellerAllowedDownload == true).ToList().AsQueryable();
                    record = seller.Union(buyer).Union(note);
                }
                else
                {
                    var search = dbObj.DownloadedNotes.Where(x => (x.NoteTitle.Contains(Search_Keyword) || (x.User.FirstName + " " + x.User.LastName).Contains(Search_Keyword) || Search_Keyword == null) && (x.IsAttachmentDownloaded==true)).ToList().AsQueryable();
                    var seller = dbObj.DownloadedNotes.Where(x => x.User1.FirstName + " " + x.User1.LastName == Seller && x.HasSellerAllowedDownload == true).ToList().AsQueryable();
                    var buyer = dbObj.DownloadedNotes.Where(x => x.User.FirstName + " " + x.User.LastName == Buyer && x.HasSellerAllowedDownload == true).ToList().AsQueryable();
                    var note = dbObj.DownloadedNotes.Where(x => x.NoteTitle == Note && x.HasSellerAllowedDownload == true).ToList().AsQueryable();
                    record = search.Union(seller).Union(buyer).Union(note);
                }
            }

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
                    record = record.OrderBy(x => (x.User.FirstName + x.User.LastName));
                    break;
                case "Buyer Desc":
                    record = record.OrderByDescending(x => (x.User.FirstName + x.User.LastName));
                    break;
                case "Seller":
                    record = record.OrderBy(x => (x.User1.FirstName + x.User1.LastName));
                    break;
                case "Seller Desc":
                    record = record.OrderByDescending(x => (x.User1.FirstName + x.User1.LastName));
                    break;
                default:
                    record = record.OrderBy(x => x.AttachmentDownloadedDate);
                    break;
            }

            return View(record.ToPagedList(i ?? 1, 10));
        }
        [Authorize(Roles = "Super Admin,Admin")]
        public ActionResult NotesUnderReview(int? i, string Search , string sortBy,string Seller)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();

            ViewBag.pic = dbObj.Admins.Where(x => x.AID == obj.ID).Select(x => x.ProfilePicture).FirstOrDefault();
            
            ViewBag.SortDate = string.IsNullOrEmpty(sortBy) ? "Date Desc" : "";
            ViewBag.SortTitle = sortBy == "Title" ? "Title Desc" : "Title";
            ViewBag.SortCategory = sortBy == "Category" ? "Category Desc" : "Category";
            ViewBag.SortSeller = sortBy == "Seller" ? "Seller Desc" : "Seller";

            ViewBag.Seller = new SelectList(dbObj.NoteDetails.Where(x => x.IsActive == true).Select(x => x.User.FirstName + " " + x.User.LastName).Distinct().ToList());

            
            IQueryable<NoteDetail> record;
            if (String.IsNullOrEmpty(Search) && String.IsNullOrEmpty(Seller))
            {
                record = dbObj.NoteDetails.Where(x => (x.Status == 2 || x.Status == 3)).Include(x => x.StatusTable).ToList().AsQueryable();
            }
            else
            {
                if (String.IsNullOrEmpty(Seller)) // Dropdown is empty
                {
                    record = dbObj.NoteDetails.Where(x => (x.NoteTitle.Contains(Search) || (x.User.FirstName + " " + x.User.LastName).Contains(Search)|| x.AddEditCategory.CategoryName.Contains(Search) || Search == null) && (x.Status == 2 || x.Status == 3)).Include(x => x.StatusTable).ToList().AsQueryable();
                }
                else if(String.IsNullOrEmpty(Search) && !String.IsNullOrEmpty(Seller))  // Search is empty
                {
                    record = dbObj.NoteDetails.Where(x => (x.User.FirstName + " " + x.User.LastName).Contains(Seller) && (x.Status == 2 || x.Status == 3)).Include(x => x.StatusTable).ToList().AsQueryable();
                }
                else
                {
                    var s = dbObj.NoteDetails.Where(x => (x.NoteTitle.Contains(Search) || (x.User.FirstName + " " + x.User.LastName).Contains(Search) || Search == null) && (x.Status == 2 || x.Status == 3)).Include(x => x.StatusTable).ToList().AsQueryable();
                    var d = dbObj.NoteDetails.Where(x => (x.User.FirstName + " " + x.User.LastName).Contains(Seller) && (x.Status == 2 || x.Status == 3)).Include(x => x.StatusTable).ToList().AsQueryable();
                    record = s.Union(d);
                }
            }
            
            switch (sortBy)
            {
                case "Date Desc":
                    record = record.OrderByDescending(x => x.CreatedDate);
                    break;
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
                case "Seller":
                    record = record.OrderBy(x => (x.User.FirstName + x.User.LastName));
                    break;
                case "Seller Desc":
                    record = record.OrderByDescending(x => (x.User.FirstName + x.User.LastName));
                    break;
                default:
                    record = record.OrderBy(x => x.CreatedDate);
                    break;
            }

            return View(record.ToPagedList(i ?? 1, 10));
        }
        public ActionResult ApproveNote(int? id)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();
            Context.NoteDetail apnoteobj = dbObj.NoteDetails.Where(x => x.ID == id).FirstOrDefault();
            apnoteobj.Status = 4;
            apnoteobj.ActionBy = obj.ID;
            apnoteobj.ModifiedBy = obj.ID;
            apnoteobj.ModifiedDate = DateTime.Now;
            apnoteobj.IsActive = true;
            dbObj.Entry(apnoteobj).State = System.Data.Entity.EntityState.Modified;
            dbObj.SaveChanges();
            return RedirectToAction("NotesUnderReview");
        }
        public ActionResult RejectNote(int? id, string Remark)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();
            Context.NoteDetail rjtobj = dbObj.NoteDetails.Where(x => x.ID == id).FirstOrDefault();
            rjtobj.Remark = Remark;
            rjtobj.Status = 5;
            rjtobj.ActionBy = obj.ID;
            rjtobj.ModifiedBy = obj.ID;
            rjtobj.ModifiedDate = DateTime.Now;
            rjtobj.IsActive = false;
            dbObj.Entry(rjtobj).State = System.Data.Entity.EntityState.Modified;
            dbObj.SaveChanges();
            return RedirectToAction("NotesUnderReview");
        }
        public ActionResult InReviewNote(int? id)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();
            Context.NoteDetail iriwobj = dbObj.NoteDetails.Where(x => x.ID == id).FirstOrDefault();
            iriwobj.Status = 3;
            iriwobj.ActionBy = obj.ID;
            iriwobj.ModifiedBy = obj.ID;
            iriwobj.ModifiedDate = DateTime.Now;
            iriwobj.IsActive = true;
            dbObj.Entry(iriwobj).State = System.Data.Entity.EntityState.Modified;
            dbObj.SaveChanges();
            return RedirectToAction("NotesUnderReview");
        }
        [Authorize(Roles = "Super Admin,Admin")]
        public ActionResult PublishedNotes(int? i, string Seller, string Search, string sortBy)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();

            ViewBag.pic = dbObj.Admins.Where(x => x.AID == obj.ID).Select(x => x.ProfilePicture).FirstOrDefault();

            ViewBag.SortDate = string.IsNullOrEmpty(sortBy) ? "Date Desc" : "";
            ViewBag.SortTitle = sortBy == "Title" ? "Title Desc" : "Title";
            ViewBag.SortCategory = sortBy == "Category" ? "Category Desc" : "Category";
            ViewBag.SortSeller = sortBy == "Seller" ? "Seller Desc" : "Seller";

            ViewBag.Seller = new SelectList(dbObj.NoteDetails.Where(x => x.IsActive == true).Select(x => x.User.FirstName + " " + x.User.LastName).Distinct().ToList());
           

            IQueryable<NoteDetail> record;
            if (String.IsNullOrEmpty(Search) && String.IsNullOrEmpty(Seller))
            {
                record = dbObj.NoteDetails.Where(x => (x.Status == 4)).Include(x => x.StatusTable).ToList().AsQueryable();
            }
            else
            {
                if (String.IsNullOrEmpty(Seller)) // Dropdown is empty
                {
                    record = dbObj.NoteDetails.Where(x => (x.NoteTitle.Contains(Search) || (x.User.FirstName + " " + x.User.LastName).Contains(Search) || x.AddEditCategory.CategoryName.Contains(Search) || Search == null) && (x.Status == 4)).Include(x => x.StatusTable).ToList().AsQueryable();
                }
                else if (String.IsNullOrEmpty(Search) && !String.IsNullOrEmpty(Seller))  // Search is empty
                {
                    record = dbObj.NoteDetails.Where(x => (x.User.FirstName + " " + x.User.LastName).Contains(Seller) && (x.Status == 4)).Include(x => x.StatusTable).ToList().AsQueryable();
                }
                else
                {
                    var s = dbObj.NoteDetails.Where(x => (x.NoteTitle.Contains(Search) || (x.User.FirstName + " " + x.User.LastName).Contains(Search) || Search == null) && (x.Status == 4)).Include(x => x.StatusTable).ToList().AsQueryable();
                    var d = dbObj.NoteDetails.Where(x => (x.User.FirstName + " " + x.User.LastName).Contains(Seller) && (x.Status == 4)).Include(x => x.StatusTable).ToList().AsQueryable();
                    record = s.Union(d);
                }
            }

            switch (sortBy)
            {
                case "Date Desc":
                    record = record.OrderByDescending(x => x.ModifiedDate);
                    break;
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
                case "Seller":
                    record = record.OrderBy(x => (x.User.FirstName + x.User.LastName));
                    break;
                case "Seller Desc":
                    record = record.OrderByDescending(x => (x.User.FirstName + x.User.LastName));
                    break;
                default:
                    record = record.OrderBy(x => x.ModifiedDate);
                    break;
            }

            return View(record.ToPagedList(i ?? 1, 10));
        }
        public ActionResult UnpublishNotes(int? id,string Remark)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();
            Context.NoteDetail unpobj = dbObj.NoteDetails.Where(x => x.ID == id).FirstOrDefault();

            unpobj.Remark = Remark;
            unpobj.Status = 6;
            unpobj.ModifiedDate = DateTime.Now;
            unpobj.ModifiedBy = obj.ID;
            unpobj.ActionBy = obj.ID;
            unpobj.IsActive = false;
            dbObj.Entry(unpobj).State = System.Data.Entity.EntityState.Modified;
            dbObj.SaveChanges();
            SendSellerUnpublishEmail(unpobj.User.EmailID,unpobj.User.FirstName,unpobj.User.LastName,unpobj.Remark,unpobj.NoteTitle);
            return RedirectToAction("PublishedNotes");
        }
        [Authorize(Roles = "Super Admin,Admin")]
        public ActionResult RejectedNotes(int? i, string Search, string sortBy, string Seller)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();

            ViewBag.pic = dbObj.Admins.Where(x => x.AID == obj.ID).Select(x => x.ProfilePicture).FirstOrDefault();

            ViewBag.SortDate = string.IsNullOrEmpty(sortBy) ? "Date Desc" : "";
            ViewBag.SortTitle = sortBy == "Title" ? "Title Desc" : "Title";
            ViewBag.SortCategory = sortBy == "Category" ? "Category Desc" : "Category";
            ViewBag.Seller = new SelectList(dbObj.NoteDetails.Where(x => x.NoteTitle.Contains(Search) || Search == null && x.IsActive == true).Select(x => x.User.FirstName + " " + x.User.LastName).Distinct().ToList());

            IQueryable<NoteDetail> record;
            if (String.IsNullOrEmpty(Search) && String.IsNullOrEmpty(Seller))
            {
                record = dbObj.NoteDetails.Where(x => (x.Status == 5)).Include(x => x.StatusTable).ToList().AsQueryable();
            }
            else
            {
                if (String.IsNullOrEmpty(Seller)) // Dropdown is empty
                {
                    record = dbObj.NoteDetails.Where(x => (x.NoteTitle.Contains(Search) || x.AddEditCategory.CategoryName.Contains(Search) || (x.User.FirstName + " " + x.User.LastName).Contains(Search) || Search == null) && (x.Status == 5)).Include(x => x.StatusTable).ToList().AsQueryable();
                }
                else if (String.IsNullOrEmpty(Search) && !String.IsNullOrEmpty(Seller))  // Search is empty
                {
                    record = dbObj.NoteDetails.Where(x => (x.User.FirstName + " " + x.User.LastName).Contains(Seller) && (x.Status == 5)).Include(x => x.StatusTable).ToList().AsQueryable();
                }
                else
                {
                    var s = dbObj.NoteDetails.Where(x => (x.NoteTitle.Contains(Search) || (x.User.FirstName + " " + x.User.LastName).Contains(Search) || Search == null) && (x.Status == 5)).Include(x => x.StatusTable).ToList().AsQueryable();
                    var d = dbObj.NoteDetails.Where(x => (x.User.FirstName + " " + x.User.LastName).Contains(Seller) && (x.Status == 5)).Include(x => x.StatusTable).ToList().AsQueryable();
                    record = s.Union(d);
                }
            }

            switch (sortBy)
            {
                case "Date Desc":
                    record = record.OrderByDescending(x => x.ModifiedDate);
                    break;
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
        public ActionResult ApproveRejectedNote(int? id)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();
            Context.NoteDetail apnoteobj = dbObj.NoteDetails.Where(x => x.ID == id).FirstOrDefault();
            apnoteobj.Status = 4;
            apnoteobj.ModifiedDate = DateTime.Now;
            apnoteobj.ModifiedBy = obj.ID;
            apnoteobj.ActionBy = obj.ID;
            apnoteobj.IsActive = true;
            dbObj.Entry(apnoteobj).State = System.Data.Entity.EntityState.Modified;
            dbObj.SaveChanges();
            return RedirectToAction("RejectedNotes");
        }
        [NonAction]
        public void SendSellerUnpublishEmail(string EmailID, string FirstName, string LastName, string Remark, string NoteTitle)
        {
            var fromEmail = new MailAddress(dbObj.ManageSystemConfigurations.FirstOrDefault().SupportEmail);
            var toEmail = new MailAddress(EmailID);
            var fromEmailPassword = dbObj.ManageSystemConfigurations.FirstOrDefault().SupportPassword;
            string subject = "Sorry! We need to remove your notes from our portal.";

            string body = "Hello " + FirstName + LastName + "," +
                "<br/><br/>We want to inform you that, your note " + NoteTitle + " has been removed from the portal." +
                "<br/>Please find our remarks as below -" +
                "<br/>" + Remark +
                "<br/><br/>Regards,<br/>Notes MarketPlace";

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
        }
    }
}