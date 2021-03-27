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
        NotesMarketPlaceEntities6 dbObj = new NotesMarketPlaceEntities6();
        public ActionResult Home()
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();
            if (obj == null)
            {
                return View();
            }
            else 
            {
                ViewBag.pic = dbObj.UserProfiles.Where(x => x.UserID == obj.ID).Select(x => x.ProfilePicture).FirstOrDefault();
                return View();
            }
                
        }
        public ActionResult SearchNotes(string Search_keyword, string Type, string Category, string University, string Courses, string Country, int? i,int? id)
        {
            /*var rating = new List<string>() { "5-Star", "4-Star", "3-Star", "2-Star", "1-Star" };
            ViewBag.list = rating;*/
            
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();
            if (obj == null)
            {
                var country = dbObj.AddEditCountries.ToList();
                var category = dbObj.AddEditCategories.ToList();
                var type = dbObj.AddEditTypes.ToList();
                var university = dbObj.NoteDetails.Where(x => x.InstituitionName != null).Select(x => x.InstituitionName).Distinct().ToList();
                var course = dbObj.NoteDetails.Where(x => x.Course != null).Select(x => x.Course).Distinct().ToList();
                var note = dbObj.NoteDetails.Where(x => x.NoteTitle.Contains(Search_keyword) || Search_keyword == null).ToList().AsQueryable();
                var dropdownitems = new SearchNotesModel()
                {
                    Country = country,
                    Category = category,
                    Type = type,
                    University = university,
                    Courses = course,
                    Note = note.Where(x => x.TypeID.ToString() == Type || String.IsNullOrEmpty(Type) && x.CategoryID.ToString() == Category || String.IsNullOrEmpty(Category) && x.InstituitionName == University || String.IsNullOrEmpty(University) && x.Course == Courses || String.IsNullOrEmpty(Courses) && x.CountryID.ToString() == Country || String.IsNullOrEmpty(Country)).ToPagedList(i ?? 1, 9)

                };
                ViewBag.flagcount = dbObj.SpamReportsTables.Where(x => x.NoteID == i).Select(x => x.NoteID).Count();
                ViewBag.countnotes = dbObj.NoteDetails.Count();
                return View(dropdownitems);
            }
            else
            {
                var country = dbObj.AddEditCountries.ToList();
                var category = dbObj.AddEditCategories.ToList();
                var type = dbObj.AddEditTypes.ToList();
                var university = dbObj.NoteDetails.Where(x => x.InstituitionName != null).Select(x => x.InstituitionName).Distinct().ToList();
                var course = dbObj.NoteDetails.Where(x => x.Course != null).Select(x => x.Course).Distinct().ToList();
                var note = dbObj.NoteDetails.Where(x => x.NoteTitle.Contains(Search_keyword) || Search_keyword == null).ToList().AsQueryable();
                
                ViewBag.pic = dbObj.UserProfiles.Where(x => x.UserID == obj.ID).Select(x => x.ProfilePicture).FirstOrDefault();
                ViewBag.flagcount = dbObj.SpamReportsTables.Where(x => x.NoteID == i).Select(x => x.NoteID).Count();
                var dropdownitems = new SearchNotesModel()
                {
                    Country = country,
                    Category = category,
                    Type = type,
                    University = university,
                    Courses = course,
                    Note = note.Where(x => x.TypeID.ToString() == Type || String.IsNullOrEmpty(Type) && x.CategoryID.ToString() == Category || String.IsNullOrEmpty(Category) && x.InstituitionName == University || String.IsNullOrEmpty(University) && x.Course == Courses || String.IsNullOrEmpty(Courses) && x.CountryID.ToString() == Country || String.IsNullOrEmpty(Country)).ToPagedList(i ?? 1, 9)

                };
                ViewBag.countnotes = dbObj.NoteDetails.Count();
                return View(dropdownitems);
            }
        }
        
        [Authorize]
        [OutputCache(NoStore = true,Duration =0,VaryByParam ="None")]
        public ActionResult Dashboard(int? i, string search, string sortBy)
        {
            ViewBag.SortDate = string.IsNullOrEmpty(sortBy) ? "Date Desc" : "";
            ViewBag.SortTitle = sortBy == "Title" ? "Title Desc" : "Title";
            ViewBag.SortCategory = sortBy == "Category" ? "Category Desc" : "Category";
            var filterTitle = dbObj.NoteDetails.Where(x => x.NoteTitle.Contains(search) || search == null);
            var filterCategory = dbObj.NoteDetails.Where(x => x.AddEditCategory.CategoryName.Contains(search));
            var filtereddata = filterTitle.Union(filterCategory);

            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();
            ViewBag.downloadscount = dbObj.DownloadedNotes.Where(x => x.Downloader == obj.ID).Count();
            ViewBag.buyerrequestscount = dbObj.DownloadedNotes.Where(x => x.Seller == obj.ID).Count();
            ViewBag.earningcount = dbObj.DownloadedNotes.Where(x => x.Seller == obj.ID && (x.HasSellerAllowedDownload == true)).Select(x => x.PurchasedPrice).Sum() ?? 0;
            ViewBag.soldnotescount = dbObj.DownloadedNotes.Where(x => x.Seller == obj.ID && (x.HasSellerAllowedDownload == true)).Count();
            var record = filtereddata.Where(x => x.UserID == obj.ID && (x.Status == 1 || x.Status == 2 || x.Status == 3)).Include(x => x.StatusTable).ToList().AsQueryable();

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
                default:
                    record = record.OrderBy(x => x.CreatedDate);
                    break;
            }
            ViewBag.pic = dbObj.UserProfiles.Where(x => x.UserID == obj.ID).Select(x => x.ProfilePicture).FirstOrDefault();
            return View(record.ToPagedList(i ?? 1,5));
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult BuyerRequests(int? i, string sortBy, string search)
        {
            ViewBag.SortDate = string.IsNullOrEmpty(sortBy) ? "Date Desc" : "";
            ViewBag.SortNoteTitle = sortBy == "Title" ? "Title Desc" : "Title";
            ViewBag.SortCategory = sortBy == "Category" ? "Category Desc" : "Category";
            var filterTitle = dbObj.DownloadedNotes.Where(x => x.NoteTitle.Contains(search) || search == null);
            var filterCategory = dbObj.DownloadedNotes.Where(x => x.Category.Contains(search));
            var filtereddata = filterTitle.Union(filterCategory);
            
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();
            NoteDetail noteID = dbObj.NoteDetails.Where(x => x.UserID == obj.ID).FirstOrDefault();
            var record = dbObj.DownloadedNotes.Where(x => x.Seller == noteID.UserID).ToList().AsQueryable();
            
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
                    record = record.OrderBy(x => x.Category);
                    break;
                case "Category Desc":
                    record = record.OrderByDescending(x => x.Category);
                    break;
                default:
                    record = record.OrderBy(x => x.CreatedDate);
                    break;
            }
            ViewBag.pic = dbObj.UserProfiles.Where(x => x.UserID == obj.ID).Select(x => x.ProfilePicture).FirstOrDefault();
            return View(record.ToPagedList(i ?? 1, 10));
        }
        [HttpGet]
        public ActionResult AllowDownload(int? id)
        {
            Context.DownloadedNote downloads = dbObj.DownloadedNotes.Where(x => x.ID == id).FirstOrDefault();
            downloads.HasSellerAllowedDownload = true;

            var NoteID = downloads.NoteID;

            Context.NoteDetail ndetail = dbObj.NoteDetails.Where(x => x.ID == NoteID).FirstOrDefault();


            Context.SellerNotesAttachment sellernotes = dbObj.SellerNotesAttachments.Where(x => x.NoteID == NoteID).FirstOrDefault();
            downloads.AttachmentPath = sellernotes.FilePath;
            dbObj.SaveChanges();

            var fromEmail = new MailAddress("thehamojha@gmail.com"); //Email of Company
            var toEmail = new MailAddress(downloads.User1.EmailID); //Buyer EmailAddress
            var fromEmailPassword = "244466666"; // Replace with actual password
            string subject = downloads.User.FirstName + " - Allows you to download a note";

            string body = "Hello " + downloads.User1.FirstName + "," +
                "<br/><br/>We would like to inform you that," + downloads.User.FirstName + " Allows you to download a note." +
                " Please login and see My Downloads tab to download particular note." +
                "<br/><br/>Regards," +
                "<br/>Notes Marketplace";

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
            return RedirectToAction("BuyerRequests");
        }
        [HttpGet]
        public ActionResult ContactUs()
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();
            if (obj == null)
            {
                return View();
            }
            else
            {
                ViewBag.pic = dbObj.UserProfiles.Where(x => x.UserID == obj.ID).Select(x => x.ProfilePicture).FirstOrDefault();
                return View();
            }
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
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();
            if (obj == null)
            {
                return View();
            }
            else
            {
                ViewBag.pic = dbObj.UserProfiles.Where(x => x.UserID == obj.ID).Select(x => x.ProfilePicture).FirstOrDefault();
                return View();
            }
        }
        [HttpGet]
        [Route("Home/AddNotes")]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult AddNotes(int? id)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();
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
                ViewBag.pic = dbObj.UserProfiles.Where(x => x.UserID == obj.ID).Select(x => x.ProfilePicture).FirstOrDefault();
                return View(editobj);
            }
                ViewBag.Category = new SelectList(dbObj.AddEditCategories, "ID", "CategoryName");
                ViewBag.Type = new SelectList(dbObj.AddEditTypes, "ID", "Type");
                ViewBag.Country = new SelectList(dbObj.AddEditCountries, "ID", "CountryName");
                ViewBag.pic = dbObj.UserProfiles.Where(x => x.UserID == obj.ID).Select(x => x.ProfilePicture).FirstOrDefault();
                return View();
        }
        [HttpPost]
        [Route("Home/AddNotes")]
        public ActionResult AddNotes(AddNotesModel model, string submitbutton)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();
            
            
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
                //Checking for directory
                string path = Path.Combine(Server.MapPath("~/Members"), obj.ID.ToString());
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                //Saving into database
                if(model.ID == null) // Adding New Notes
                {
                    NoteDetail addnoteobj = new NoteDetail();
                    //addnoteobj.ID = model.ID;
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
                    if(submitbutton == "1")
                    {
                        addnoteobj.Status = 1;
                    }
                    else
                    {
                        addnoteobj.Status = 2;
                    }

                    addnoteobj.ActionBy = 3;
                    addnoteobj.CreatedDate = DateTime.Now;
                    addnoteobj.CreatedBy = obj.ID;
                    addnoteobj.IsActive = true;
                    
                    dbObj.NoteDetails.Add(addnoteobj);
                    dbObj.SaveChanges();

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
                        var displayimagename = "DP_" + DateTime.Now.ToString("dd-MM-yy").Replace(':', '-').Replace(' ', '_') + Path.GetExtension(model.DisplayPicture.FileName);
                        var ImageSavePath = Path.Combine(Server.MapPath("~/Members/" + obj.ID + "/" + addnoteobj.ID + "/") + displayimagename);
                        model.DisplayPicture.SaveAs(ImageSavePath);
                        addnoteobj.DisplayPicture = Path.Combine(("~/Members/" + obj.ID + "/" + addnoteobj.ID + "/"), displayimagename);
                        dbObj.SaveChanges();
                    }
                    /*else
                    {
                        addnoteobj.DisplayPicture = "C:/Users/Maitrarajsinh/source/repos/Notes-MarketPlace/Default/1.jpg";
                        dbObj.SaveChanges();
                    }
                    if (model.NotesPreview != null && model.NotesPreview.ContentLength > 0)
                    {
                        var notespreviewname = "Preview_" + Path.GetFileName(model.NotesPreview.FileName);
                        var PreviewSavePath = Path.Combine(Server.MapPath("~/Members/" + obj.ID + "/" + addnoteobj.ID + "/") + DateTime.Now.ToString("dd-MM-yy").Replace(':', '-').Replace(' ', '_') + "_" + notespreviewname);
                        model.NotesPreview.SaveAs(PreviewSavePath);
                        addnoteobj.NotesPreview = Path.Combine(("~/Members/" + obj.ID + "/" + addnoteobj.ID + "/"), notespreviewname);
                        dbObj.SaveChanges();
                    }*/

                    if (model.NotesPreview != null && model.NotesPreview.ContentLength > 0)
                    {
                        var notespreviewname = "Preview_" + DateTime.Now.ToString("dd-MM-yy").Replace(':', '-').Replace(' ', '_') + Path.GetExtension(model.NotesPreview.FileName);
                        var PreviewSavePath = Path.Combine(Server.MapPath("~/Members/" + obj.ID + "/" + addnoteobj.ID + "/") + notespreviewname);
                        model.NotesPreview.SaveAs(PreviewSavePath);
                        addnoteobj.NotesPreview = Path.Combine(("~/Members/" + obj.ID + "/" + addnoteobj.ID + "/"), notespreviewname);
                        dbObj.SaveChanges();
                    }

                    SellerNotesAttachment nattachobj = new SellerNotesAttachment();
                    nattachobj.NoteID = NoteID;
                    nattachobj.IsActive = true;
                    nattachobj.CreatedBy = obj.ID;
                    nattachobj.CreatedDate = DateTime.Now;

                    string storeUploadPath = Path.Combine(finalpath, "Attachment");

                    if (!Directory.Exists(storeUploadPath))
                    {
                        Directory.CreateDirectory(storeUploadPath);
                    }

                    //Upload Notes
                    int count = 1;
                    var uploadfilepath = "";
                    var uploadfilename = "";

                    foreach (HttpPostedFileBase file in model.File)
                    {
                        string _FileName = Path.GetFileNameWithoutExtension(file.FileName);
                        string extension = Path.GetExtension(file.FileName);
                        _FileName = "Attachment" + count + "_" + DateTime.Now.ToString("dd-MM-yy") + extension;
                        string final = Path.Combine(storeUploadPath, _FileName);
                        file.SaveAs(final);
                        uploadfilename += _FileName + ";";
                        uploadfilepath += Path.Combine(("/Members/" + obj.ID + "/" + NoteID + "/Attachment/"), _FileName);
                        count++;

                    }

                    nattachobj.FileName = uploadfilename;
                    nattachobj.FilePath = uploadfilepath;
                    dbObj.SellerNotesAttachments.Add(nattachobj);
                    dbObj.SaveChanges();

                    return RedirectToAction("Dashboard", "Home");
                }
                else // Editing old notes
                {
                    NoteDetail editnoteobj = dbObj.NoteDetails.Where(x => x.ID == model.ID).FirstOrDefault();
                    //addnoteobj.ID = model.ID;
                    //editnoteobj.UserID = obj.ID;
                    editnoteobj.NoteTitle = model.NoteTitle;
                    editnoteobj.CategoryID = model.CategoryID;
                    editnoteobj.TypeID = model.TypeID;
                    editnoteobj.NumberOfPages = model.NumberOfPages;
                    editnoteobj.Description = model.Description;
                    editnoteobj.CountryID = model.CountryID;
                    editnoteobj.InstituitionName = model.InstituitionName;
                    editnoteobj.Course = model.Course;
                    editnoteobj.CourseCode = model.CourseCode;
                    editnoteobj.Professor = model.Professor;
                    editnoteobj.IsPaid = model.IsPaid;
                    editnoteobj.SellingPrice_USD = model.SellingPrice_USD;

                    if (submitbutton == "1")
                    {
                        editnoteobj.Status = 1;
                    }
                    else
                    {
                        editnoteobj.Status = 2;
                    }

                    editnoteobj.ActionBy = 3;
                    editnoteobj.ModifiedDate = DateTime.Now;
                    editnoteobj.ModifiedBy = obj.ID;
                    editnoteobj.IsActive = true;

                    dbObj.Entry(editnoteobj).State = System.Data.Entity.EntityState.Modified;
                    dbObj.SaveChanges();
                    

                    //Acquiring NoteID

                    var NoteID = editnoteobj.ID;
                    string finalpath = Path.Combine(Server.MapPath("~/Members/" + obj.ID), NoteID.ToString());

                    if (!Directory.Exists(finalpath))
                    {
                        Directory.CreateDirectory(finalpath);
                    }

                    if (model.DisplayPicture != null && model.DisplayPicture.ContentLength > 0)
                    {
                        //var displayimagename = Path.GetFileName(model.DisplayPicture.FileName);
                        var displayimagename = "DP_" + DateTime.Now.ToString("dd-MM-yy").Replace(':', '-').Replace(' ', '_') + Path.GetExtension(model.DisplayPicture.FileName);
                        var ImageSavePath = Path.Combine(Server.MapPath("~/Members/" + obj.ID + "/" + editnoteobj.ID + "/") + displayimagename);
                        model.DisplayPicture.SaveAs(ImageSavePath);
                        editnoteobj.DisplayPicture = Path.Combine(("~/Members/" + obj.ID + "/" + editnoteobj.ID + "/"), displayimagename);
                        dbObj.SaveChanges();
                    }
                    /*else
                    {
                        editnoteobj.DisplayPicture = "C:/Users/Maitrarajsinh/source/repos/Notes-MarketPlace/Default/1.jpg";
                        dbObj.SaveChanges();
                    }*/

                    if (model.NotesPreview != null && model.NotesPreview.ContentLength > 0)
                    {
                        var notespreviewname = "Preview_" + DateTime.Now.ToString("dd-MM-yy").Replace(':', '-').Replace(' ', '_') + Path.GetExtension(model.NotesPreview.FileName);
                        var PreviewSavePath = Path.Combine(Server.MapPath("~/Members/" + obj.ID + "/" + editnoteobj.ID + "/") + notespreviewname);
                        model.NotesPreview.SaveAs(PreviewSavePath);
                        editnoteobj.NotesPreview = Path.Combine(("~/Members/" + obj.ID + "/" + editnoteobj.ID + "/"), notespreviewname);
                        dbObj.SaveChanges();
                    }

                    SellerNotesAttachment editattachobj = dbObj.SellerNotesAttachments.Where(x => x.NoteID == model.ID).FirstOrDefault();
                    
                    editattachobj.ModifiedDate = DateTime.Now;
                    editattachobj.ModifiedBy = obj.ID;

                    string storeUploadPath = Path.Combine(finalpath, "Attachment");

                    if (!Directory.Exists(storeUploadPath))
                    {
                        Directory.CreateDirectory(storeUploadPath);
                    }

                    //Upload Notes
                    int count = 1;
                    var uploadfilepath = "";
                    var uploadfilename = "";

                    foreach (HttpPostedFileBase file in model.File)
                    {
                        string _FileName = Path.GetFileNameWithoutExtension(file.FileName);
                        string extension = Path.GetExtension(file.FileName);
                        _FileName = "Attachment" + count + "_" + DateTime.Now.ToString("dd-MM-yy") + extension;
                        string final = Path.Combine(storeUploadPath, _FileName);
                        file.SaveAs(final);
                        uploadfilename += _FileName + ";";
                        uploadfilepath += Path.Combine(("/Members/" + obj.ID + "/" + NoteID + "/Attachment/"), _FileName);
                        count++;

                    }

                    editattachobj.FileName = uploadfilename;
                    editattachobj.FilePath = uploadfilepath;
                   
                    dbObj.Entry(editattachobj).State = System.Data.Entity.EntityState.Modified;
                    dbObj.SaveChanges();
                }
                return RedirectToAction("Dashboard", "Home");
            }
            ViewBag.Category = new SelectList(dbObj.AddEditCategories, "ID", "CategoryName");
            ViewBag.Type = new SelectList(dbObj.AddEditTypes, "ID", "Type");
            ViewBag.Country = new SelectList(dbObj.AddEditCountries, "ID", "CountryName");
            return View();
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
        [HttpGet]
        [Route("Home/NoteDetails")]
        public ActionResult NoteDetails(int id)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();
            if (obj == null)
            {
                ViewBag.flagcount = dbObj.SpamReportsTables.Where(x => x.NoteID == id).Select(x => x.NoteID).Count();
                Context.NoteDetail record = dbObj.NoteDetails.Where(x => x.ID == id).FirstOrDefault();
                return View(record);
            }
            else
            {
                ViewBag.Buyer = obj.FirstName;
                ViewBag.flagcount = dbObj.SpamReportsTables.Where(x => x.NoteID == id).Select(x => x.NoteID).Count();
                ViewBag.pic = dbObj.UserProfiles.Where(x => x.UserID == obj.ID).Select(x => x.ProfilePicture).FirstOrDefault();
                var totalreview = dbObj.NoteDetails.Where(x => x.ID == id).FirstOrDefault().Review;
                var totalstars = dbObj.NoteDetails.Where(x => x.ID == id).FirstOrDefault().Star;
                if(totalreview == null)
                {
                    ViewBag.totalreviews = 0;
                    ViewBag.Rating = 0;
                }
                else
                {
                    ViewBag.totalreviews = totalreview;
                    ViewBag.Rating = (totalstars / totalreview)*20;
                }
                Context.NoteDetail record = dbObj.NoteDetails.Where(x => x.ID == id).FirstOrDefault();
                return View(record);
            }
        }
        [Authorize]
        [HttpPost]
        [Route("Home/NotesDownload")]
        public ActionResult NotesDownload(Context.NoteDetail model)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User buyerobj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();

            SellerNotesAttachment sellerattachobj = dbObj.SellerNotesAttachments.Where(x => x.NoteID == model.ID).FirstOrDefault();
            NoteDetail noteobj = dbObj.NoteDetails.Where(x => x.ID == model.ID).FirstOrDefault();
            Context.User selleruserobj = dbObj.Users.Where(x => x.ID == noteobj.UserID).FirstOrDefault();
            DownloadedNote notedwnldobj = new DownloadedNote();
            if (ModelState.IsValid)
            {
                if (noteobj.IsPaid == false)
                {
                    Response.ContentType = "application/pdf";
                    Response.AppendHeader("Content-Disposition", "attachment; filename =" + noteobj.NoteTitle + ".pdf");
                    Response.TransmitFile(sellerattachobj.FilePath);
                    Response.End();
                }
                else
                {
                    SendDownloadEmailToAdmin(selleruserobj.EmailID, selleruserobj.FirstName, selleruserobj.LastName, buyerobj.FirstName, buyerobj.LastName);
                }
                if (noteobj.UserID == buyerobj.ID)
                {
                    TempData["ErrorMsg"] = "Email Address is not  yet verified";
                }
                else
                { 
                    if (noteobj.IsPaid.Equals(false))
                    {
                        notedwnldobj.NoteID = noteobj.ID;
                        notedwnldobj.Seller = noteobj.UserID;
                        notedwnldobj.Downloader = buyerobj.ID;
                        notedwnldobj.HasSellerAllowedDownload = true;
                        notedwnldobj.AttachmentPath = sellerattachobj.FilePath;
                        notedwnldobj.IsAttachmentDownloaded = true;
                        notedwnldobj.AttachmentDownloadedDate = DateTime.Now;
                        notedwnldobj.IsPaid = noteobj.IsPaid;
                        notedwnldobj.PurchasedPrice = noteobj.SellingPrice_USD;
                        notedwnldobj.NoteTitle = noteobj.NoteTitle;
                        notedwnldobj.Category = noteobj.AddEditCategory.CategoryName;
                        notedwnldobj.CreatedDate = DateTime.Now;
                    }
                    else
                    {
                        notedwnldobj.NoteID = noteobj.ID;
                        notedwnldobj.Seller = noteobj.UserID;
                        notedwnldobj.Downloader = buyerobj.ID;
                        notedwnldobj.HasSellerAllowedDownload = false;
                        notedwnldobj.AttachmentPath = null;
                        notedwnldobj.IsAttachmentDownloaded = false;
                        notedwnldobj.AttachmentDownloadedDate = null;
                        notedwnldobj.IsPaid = noteobj.IsPaid;
                        notedwnldobj.PurchasedPrice = noteobj.SellingPrice_USD;
                        notedwnldobj.NoteTitle = noteobj.NoteTitle;
                        notedwnldobj.Category = noteobj.AddEditCategory.CategoryName;
                        notedwnldobj.CreatedDate = DateTime.Now;
                    }
                    dbObj.DownloadedNotes.Add(notedwnldobj);
                    dbObj.SaveChanges();
                }
            }
            return RedirectToAction("NoteDetails", "Home", new  { id=noteobj.ID});
        }
        [NonAction]
        public void SendContactAdminEmail(string FullName , string Comments_Questions)
        {
            var fromEmail = new MailAddress("thehamojha@gmail.com"); //Company Email
            var toEmail = new MailAddress("maitrarajsinhchudasama05799@gmail.com"); //Admin Email
            var fromEmailPassword = "244466666"; //Password of Company Email
            string subject = "" + FullName + "";

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
        [NonAction]
        public void SendDownloadEmailToAdmin(string SellerEmailID, string SellerFirstName, string SellerLastName , string BuyerFirstName, string BuyerLastName)
        {
            var fromEmail = new MailAddress("thehamojha@gmail.com"); //Company Email
            var toEmail = new MailAddress(SellerEmailID); //Seller Email
            var fromEmailPassword = "244466666"; //Password of Company Email
            string subject = BuyerFirstName + BuyerLastName + " wants to purchase your notes";

            string body = "Hello" + " " + SellerFirstName + " " + SellerLastName + "," +
                "<br/><br/>We would like to inform you that, " + BuyerFirstName + " " + BuyerLastName + " wants to purchase your notes. Please see " +
                "Buyer Requests tab and allow download access to Buyer if you have received the payment from " +
                "him." +
                "<br/><br/>Regards," +
                "<br/>Notes Marketplace";

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