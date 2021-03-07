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
    public class HomeController : Controller
    {
        NotesMarketPlaceEntities1 dbObj = new NotesMarketPlaceEntities1();
        // GET: Home
        
        public ActionResult Home()
        {
            return View();
        }
        public ActionResult SearchNotes()
        {
            return View();
        }
        [Authorize]
        public ActionResult Dashboard(int? i)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();
            var record = dbObj.NoteDetails.Where(x => x.UserID == obj.ID && (x.Status == 1 || x.Status == 2 || x.Status == 3)).Include(x => x.StatusTable).ToList().AsQueryable();
            return View(record.ToPagedList(i ?? 1,5));
        }
       
        public ActionResult BuyerRequests(int? i)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();
            var record = dbObj.NoteDetails.Where(x => x.UserID == obj.ID && (x.Status == 1 || x.Status == 2 || x.Status == 3)).Include(x => x.StatusTable).ToList().AsQueryable();
            return View(record.ToPagedList(i ?? 1, 10));
        }
        public ActionResult ContactUs()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ContactAdmin(Models.ContactUsModel model)
        {
            if (ModelState.IsValid)
            {
                Context.ContactU obj = new Context.ContactU();
                
                obj.FullName = model.FullName;
                obj.EmailId = model.EmailId;
                obj.Subject = model.Subject;
                obj.Comments_Questions = model.Comments_Questions;
                
                dbObj.ContactUs.Add(obj);
                dbObj.SaveChanges();

                SendContactAdminEmail(model.FullName, model.Comments_Questions);


            }
            ModelState.Clear();
            return RedirectToAction("ContactUs");
        }
        public ActionResult FAQ()
        {
            return View();
        }
        // GET: Notes
        [HttpGet]
        [Route("Home/AddNotes")]
        public ActionResult AddNotes(int? id)
        {
            if (id != null)
            {
                NoteDetail noteobj = dbObj.NoteDetails.Where(x=>x.ID==id).FirstOrDefault();
                AddNotesModel editobj = new AddNotesModel();
                editobj.ID = noteobj.ID;
                editobj.NoteTitle = noteobj.NoteTitle;
                editobj.CategoryID = noteobj.CategoryID;
                editobj.TypeID = noteobj.TypeID;
                editobj.NumberOfPages = noteobj.NumberOfPages;
                editobj.Description = noteobj.Description;
                editobj.CountryID = noteobj.CountryID;
                editobj.InstituitionName = noteobj.InstituitionName;
                editobj.Course = noteobj.Course;
                editobj.CourseCode = noteobj.CourseCode;
                editobj.Professor = noteobj.Professor;
                editobj.IsPaid = noteobj.IsPaid;
                editobj.SellingPrice_USD = noteobj.SellingPrice_USD;
                
                ViewBag.Category = new SelectList(dbObj.AddEditCategories, "ID", "CategoryName");
                ViewBag.Type = new SelectList(dbObj.AddEditTypes, "ID", "Type");
                ViewBag.Country = new SelectList(dbObj.AddEditCountries, "ID", "CountryName");
                return View(editobj);
            }
            
             
                ViewBag.Category = new SelectList(dbObj.AddEditCategories, "ID", "CategoryName");
                ViewBag.Type = new SelectList(dbObj.AddEditTypes, "ID", "Type");
                ViewBag.Country = new SelectList(dbObj.AddEditCountries, "ID", "CountryName");
                return View();
            
        }
        [HttpPost]
        [Route("Home/AddNotes")]
        public ActionResult AddNotes(AddNotesModel model,string submitbutton)
        {
            if (ModelState.IsValid)
            {
                if ((model.IsPaid == true) && (model.NotesPreview == null))
                {
                    ModelState.AddModelError("NotesPreview", "NotesPreview Required");
                    ViewBag.Category = new SelectList(dbObj.AddEditCategories, "ID", "CategoryName");
                    ViewBag.Type = new SelectList(dbObj.AddEditTypes, "ID", "Type");
                    ViewBag.Country = new SelectList(dbObj.AddEditCountries, "ID", "CountryName");
                    return View(model);
                }

                var emailid = User.Identity.Name.ToString();
                Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();
                string path = Path.Combine(Server.MapPath("~/Members"), obj.ID.ToString());

                //Checking for directory

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                //Saving into database

                NoteDetail addnoteobj = new NoteDetail();
                addnoteobj.ID = model.ID;
                addnoteobj.UserID = obj.ID;
                addnoteobj.NoteTitle = model.NoteTitle;
                addnoteobj.CategoryID = model.CategoryID;
                addnoteobj.TypeID = model.TypeID;
                addnoteobj.NumberOfPages = model.NumberOfPages;
                addnoteobj.Description = model.Description;
                addnoteobj.CountryID = model.CountryID;
                addnoteobj.InstituitionName = model.InstituitionName;
                addnoteobj.Course = model.Course;
                addnoteobj.CourseCode = model.CourseCode;
                addnoteobj.Professor = model.Professor;
                addnoteobj.IsPaid = model.IsPaid;
                addnoteobj.SellingPrice_USD = model.SellingPrice_USD;
                if (submitbutton == "1")
                {
                    addnoteobj.Status = 1;
                }
                else
                {
                    addnoteobj.Status = 2;
                }
                addnoteobj.ActionBy = 3;
                addnoteobj.CreatedDate = DateTime.Now;
                addnoteobj.IsActive = true;

                if (model.ID == 0)
                {
                    dbObj.NoteDetails.Add(addnoteobj);
                    dbObj.SaveChanges();
                }
                else
                {
                    dbObj.Entry(addnoteobj).State = EntityState.Modified;
                    dbObj.SaveChanges();
                }

                //Acquiring NoteID

                var NoteID = addnoteobj.ID;
                string finalpath = Path.Combine(Server.MapPath("~/Members/" + obj.ID), NoteID.ToString());

                if (!Directory.Exists(finalpath))
                {
                    Directory.CreateDirectory(finalpath);
                }

                if (model.DisplayPicture != null && model.DisplayPicture.ContentLength > 0)
                {
                    //var displayimagename = Path.GetFileName(model.DisplayPicture.FileName);
                    var displayimagename = DateTime.Now.ToString().Replace(':', '-').Replace(' ', '_') + Path.GetExtension(model.DisplayPicture.FileName);
                    var ImageSavePath = Path.Combine(Server.MapPath("~/Members/" + obj.ID + "/" + addnoteobj.ID + "/") + "DP_" + displayimagename);
                    model.DisplayPicture.SaveAs(ImageSavePath);
                    addnoteobj.DisplayPicture = Path.Combine(("~/Members/" + obj.ID + "/" + addnoteobj.ID + "/"), displayimagename);
                    dbObj.SaveChanges();
                }
                else
                {
                    addnoteobj.DisplayPicture = "C:/Users/admin/source/repos/Notes-MarketPlace/NotesMarketPlace/Default/UserProfileImage.jpg";
                    dbObj.SaveChanges();
                }

                if (model.NotesPreview != null && model.NotesPreview.ContentLength > 0)
                {
                    var notespreviewname = Path.GetFileName(model.NotesPreview.FileName);
                    var PreviewSavePath = Path.Combine(Server.MapPath("~/Members/" + obj.ID + "/" + addnoteobj.ID + "/") + "Preview_" + DateTime.Now.ToString().Replace(':', '-').Replace(' ', '_') + "_" + notespreviewname);
                    model.NotesPreview.SaveAs(PreviewSavePath);
                    addnoteobj.NotesPreview = Path.Combine(("~/Members/" + obj.ID + "/" + addnoteobj.ID + "/"), notespreviewname);
                    dbObj.SaveChanges();
                }

                SellerNotesAttachment nattachobj = new SellerNotesAttachment();
                nattachobj.NoteID = NoteID;
                nattachobj.IsActive = true;
                nattachobj.CreatedBy = obj.ID;
                nattachobj.CreatedDate = DateTime.Now;

                string AttachmentPath = Path.Combine(Server.MapPath("~/Members/" + obj.ID + "/" + addnoteobj.ID), "Attachment");

                if (!Directory.Exists(AttachmentPath))
                {
                    Directory.CreateDirectory(AttachmentPath);
                }

                int counter = 1;
                var uploadfilepath = "";
                var uploadfilename = "";

                foreach (HttpPostedFileBase file in model.File)
                {
                    //Checking file is available to save.  
                    if (file != null)
                    {
                        var InputFileName = Path.GetFileName(file.FileName);
                        var ServerSavePath = Path.Combine(Server.MapPath("~/Members/" + obj.ID + "/" + addnoteobj.ID + "/Attachment/") + "Attachment_" + counter + "_" + DateTime.Now.ToString().Replace(':', '-').Replace(' ', '_') + "_" + InputFileName);
                        counter++;
                        //Save file to server folder
                        file.SaveAs(ServerSavePath);
                        uploadfilepath += ServerSavePath + ";";
                        uploadfilename += InputFileName + ";";
                    }

                }

                nattachobj.FileName = uploadfilename;
                nattachobj.FilePath = uploadfilepath;
                dbObj.SellerNotesAttachments.Add(nattachobj);
                dbObj.SaveChanges();

                return RedirectToAction("Dashboard", "Home");
            }

            return RedirectToAction("Dashboard", "Home");
        }
        public ActionResult DeleteNotes(int id)
        {
            NoteDetail deletenote = dbObj.NoteDetails.Where(x => x.ID == id).FirstOrDefault();
            var NoteID = deletenote.ID;
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();
            deletenote.UserID = obj.ID;
            SellerNotesAttachment nattachobj = dbObj.SellerNotesAttachments.Where(x => x.NoteID == NoteID).FirstOrDefault();
            string mappedPath = Server.MapPath(@"~/Members/" + obj.ID + "/" + id);
            Directory.Delete(mappedPath,true);
            dbObj.SellerNotesAttachments.Remove(nattachobj);
            dbObj.NoteDetails.Remove(deletenote);
            dbObj.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        public ActionResult NoteDetails()
        {
            return View();
        }
        public ActionResult UserProfile() 
        {
            return View();
        }
        [NonAction]
        public void SendContactAdminEmail(string FullName , string Comments_Questions)
        {
            var fromEmail = new MailAddress("thehamojha@gmail.com");
            var toEmail = new MailAddress("maitrarajsinhchudasama05799@gmail.com");
            var fromEmailPassword = "244466666";
            string subject = ""+FullName+"";

            string body = "Hello" + "," +
                "<br/><br/>" + Comments_Questions +
                "<br/><br/>Regards,<br/>" + FullName;

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