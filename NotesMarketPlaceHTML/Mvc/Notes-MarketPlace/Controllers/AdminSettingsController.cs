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
    public class AdminSettingsController : Controller
    {
        NotesMarketPlaceEntities8 dbObj = new NotesMarketPlaceEntities8();

        [HttpGet]
        public ActionResult AddAdministrator()
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();
            ViewBag.PhoneNumber_CountryCode = new SelectList(dbObj.AddEditCountries, "CountryCode", "CountryCode");//Value,text
            ViewBag.pic = dbObj.Admins.Where(x => x.AID == obj.ID).Select(x => x.ProfilePicture).FirstOrDefault();
            return View();
        }
        [HttpPost]
        public ActionResult AddAdministrator(Models.AddAdministrator model)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj33 = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();

            ViewBag.PhoneNumber_CountryCode = new SelectList(dbObj.AddEditCountries, "CountryCode", "CountryCode"); //Value,text

            if (ModelState.IsValid)
            {
                Context.User obj = new Context.User();
                Context.Admin adobj = new Context.Admin();

                obj.RoleID = 2;
                obj.FirstName = model.FirstName;
                obj.LastName = model.LastName;
                obj.EmailID = model.EmailID;
                obj.IsEmailVerified = false;
                obj.CreatedDate = DateTime.Now;
                obj.IsActive = true;
                obj.ActivationCode = Guid.NewGuid();
                string adminpassword = Membership.GeneratePassword(10, 2);
                obj.Password = adminpassword;
                obj.CreatedBy = obj33.ID;

                adobj.AID = obj.ID;
                adobj.PhoneNumber_CountryCode = model.PhoneNumber_CountryCode;
                adobj.PhoneNumber = model.PhoneNumber;
                adobj.IsActive = true;

                dbObj.Admins.Add(adobj);
                dbObj.Users.Add(obj);
                dbObj.SaveChanges();

                SendAcknowledgeAdminEmail(model.EmailID, model.FirstName, adminpassword);
            }
            ModelState.Clear();
            return RedirectToAction("ManageAdministrator");
        }
        [HttpGet]
        public ActionResult EditAdministrator(int? id)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.ID == id).FirstOrDefault();
            Models.AddAdministrator proobj = new Models.AddAdministrator();
            var oldobj = dbObj.Users.Where(x => x.ID == obj.ID).FirstOrDefault();

            if (oldobj != null)
            {
                proobj.ID = obj.ID;
                proobj.FirstName = obj.FirstName;
                proobj.LastName = obj.LastName;
                proobj.EmailID = obj.EmailID;

                Context.Admin upobj = dbObj.Admins.Where(x => x.AID == obj.ID).FirstOrDefault();

                proobj.PhoneNumber_CountryCode = upobj.PhoneNumber_CountryCode;
                proobj.PhoneNumber = upobj.PhoneNumber;

                ViewBag.PhoneNumber_CountryCode = new SelectList(dbObj.AddEditCountries, "CountryCode", "CountryCode");//Value,text
                ViewBag.pic = dbObj.Admins.Where(x => x.AID == obj.ID).Select(x => x.ProfilePicture).FirstOrDefault();
                return View(proobj);
            }
            else
            {
                proobj.FirstName = obj.FirstName;
                proobj.LastName = obj.LastName;
                proobj.EmailID = obj.EmailID;

                ViewBag.PhoneNumber_CountryCode = new SelectList(dbObj.AddEditCountries, "CountryCode", "CountryCode");//Value,text
                ViewBag.pic = dbObj.Admins.Where(x => x.AID == obj.ID).Select(x => x.ProfilePicture).FirstOrDefault();

                return View(proobj);
            }
        }
        [HttpPost]
        public ActionResult EditAdministrator(Models.AddAdministrator model)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj33 = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();

            ViewBag.PhoneNumber_CountryCode = new SelectList(dbObj.AddEditCountries, "CountryCode", "CountryCode"); //Value,text
            if (ModelState.IsValid)
            {
                Context.User obj = dbObj.Users.Where(x => x.ID == model.ID).FirstOrDefault();
                Context.Admin adobj = dbObj.Admins.Where(x => x.AID == model.ID).FirstOrDefault();

                obj.FirstName = model.FirstName;
                obj.LastName = model.LastName;
                obj.EmailID = model.EmailID;
                obj.ModifiedDate = DateTime.Now;
                obj.ModifiedBy = obj33.ID;

                adobj.PhoneNumber_CountryCode = model.PhoneNumber_CountryCode;
                adobj.PhoneNumber = model.PhoneNumber;

                dbObj.Entry(adobj).State = System.Data.Entity.EntityState.Modified;
                dbObj.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                dbObj.SaveChanges();
            }
            ModelState.Clear();
            return RedirectToAction("ManageAdministrator");
        }
        public ActionResult DeactivateAdministrator(int? id)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj33 = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();

            Context.User dltadmin = dbObj.Users.Where(x => x.ID == id).FirstOrDefault();
            Context.Admin deleteadmin = dbObj.Admins.Where(x => x.AID == id).FirstOrDefault();

            if (dltadmin.IsActive == true && deleteadmin.IsActive == true)
            {
                dltadmin.IsActive = false;
                dltadmin.ModifiedDate = DateTime.Now;
                dltadmin.ModifiedBy = obj33.ID;

                deleteadmin.IsActive = false;
                deleteadmin.ModifiedDate = DateTime.Now;
                deleteadmin.ModifiedBy = obj33.ID;

                dbObj.Entry(deleteadmin).State = System.Data.Entity.EntityState.Modified;
                dbObj.Entry(dltadmin).State = System.Data.Entity.EntityState.Modified;
                dbObj.SaveChanges();
            }
            else
            {
                dltadmin.IsActive = true;
                dltadmin.ModifiedDate = DateTime.Now;
                dltadmin.ModifiedBy = obj33.ID;

                deleteadmin.IsActive = true;
                deleteadmin.ModifiedDate = DateTime.Now;
                deleteadmin.ModifiedBy = obj33.ID;

                dbObj.Entry(deleteadmin).State = System.Data.Entity.EntityState.Modified;
                dbObj.Entry(dltadmin).State = System.Data.Entity.EntityState.Modified;
                dbObj.SaveChanges();
            }
            return RedirectToAction("ManageAdministrator");
        }
        [Authorize(Roles = "Super Admin")]
        public ActionResult ManageAdministrator(int? i, string Search, string sortBy)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();

            ViewBag.SortDate = string.IsNullOrEmpty(sortBy) ? "Date Desc" : "";
            ViewBag.SortFname = sortBy == "First Name" ? "First Name Desc" : "First Name";
            ViewBag.SortLname = sortBy == "Last Name" ? "Last Name Desc" : "Last Name";
            ViewBag.SortEmail = sortBy == "Email" ? "Email Desc" : "Email";

            var record = dbObj.Users.Where(x => x.FirstName.Contains(Search) || x.LastName.Contains(Search) || (x.FirstName + " " + x.LastName).Contains(Search) || x.EmailID.Contains(Search) || Search == null).ToList().AsQueryable();
            record = record.Where(x => x.RoleID == 2).ToList().AsQueryable();

            ViewBag.pic = dbObj.Admins.Where(x => x.AID == obj.ID).Select(x => x.ProfilePicture).FirstOrDefault();

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
        public ActionResult ManageCategory(int? i, string Search, string sortBy)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();

            ViewBag.SortDate = string.IsNullOrEmpty(sortBy) ? "Date Desc" : "";
            ViewBag.SortCategory = sortBy == "Category" ? "Category Desc" : "Category";
            ViewBag.Description = sortBy == "Description" ? "Description Desc" : "Description";

            var record = dbObj.AddEditCategories.Where(x => x.CategoryName.Contains(Search) || x.Description.Contains(Search) || Search == null).ToList().AsQueryable();

            ViewBag.pic = dbObj.Admins.Where(x => x.AID == obj.ID).Select(x => x.ProfilePicture).FirstOrDefault();

            switch (sortBy)
            {
                case "Date Desc":
                    record = record.OrderByDescending(x => x.CreatedDate);
                    break;

                case "Category":
                    record = record.OrderBy(x => x.CategoryName);
                    break;
                case "Category Desc":
                    record = record.OrderByDescending(x => x.CategoryName);
                    break;
                case "Description":
                    record = record.OrderBy(x => x.Description);
                    break;
                case "Description Desc":
                    record = record.OrderByDescending(x => x.Description);
                    break;
                default:
                    record = record.OrderBy(x => x.CreatedDate);
                    break;
            }

            return View(record.ToPagedList(i ?? 1, 5));
        }
        [HttpGet]
        public ActionResult AddCategory(int? id)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();
            var oldobj = dbObj.AddEditCategories.Where(x => x.ID == id).FirstOrDefault();
            ManageCategoryModel catobj = new ManageCategoryModel();
            if (oldobj != null)
            {
                Context.AddEditCategory viewobj = dbObj.AddEditCategories.Where(x => x.ID == id).FirstOrDefault();
                catobj.CategoryName = viewobj.CategoryName;
                catobj.Description = viewobj.Description;
                ViewBag.pic = dbObj.Admins.Where(x => x.AID == obj.ID).Select(x => x.ProfilePicture).FirstOrDefault();
                return View(catobj);
            }
            ViewBag.pic = dbObj.Admins.Where(x => x.AID == obj.ID).Select(x => x.ProfilePicture).FirstOrDefault();
            return View();
        }
        [HttpPost]
        public ActionResult AddCategory(Models.ManageCategoryModel model)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();

            if (ModelState.IsValid || (model.ID == null))
            {

                if (model.ID == null)
                {
                    Context.AddEditCategory addobj = new Context.AddEditCategory();

                    addobj.CategoryName = model.CategoryName;
                    addobj.Description = model.Description;
                    addobj.CreatedDate = DateTime.Now;
                    addobj.CreatedBy = obj.ID;
                    addobj.IsActive = true;

                    dbObj.AddEditCategories.Add(addobj);
                    dbObj.SaveChanges();
                }
                else
                {
                    var oldobj = dbObj.AddEditCategories.Where(x => x.ID == model.ID).FirstOrDefault();
                    oldobj.CategoryName = model.CategoryName;
                    oldobj.Description = model.Description;
                    oldobj.ModifiedDate = DateTime.Now;
                    oldobj.ModifiedBy = obj.ID;
                    dbObj.Entry(oldobj).State = System.Data.Entity.EntityState.Modified;
                    dbObj.SaveChanges();
                }
            }
            ModelState.Clear();
            return RedirectToAction("ManageCategory");
        }
        public ActionResult DeleteCategory(int id)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();

            AddEditCategory deletecategory = dbObj.AddEditCategories.Where(x => x.ID == id).FirstOrDefault();

            if (deletecategory.IsActive == true)
            {
                deletecategory.IsActive = false;
                deletecategory.ModifiedDate = DateTime.Now;
                deletecategory.ModifiedBy = obj.ID;

                dbObj.Entry(deletecategory).State = System.Data.Entity.EntityState.Modified;
                dbObj.SaveChanges();
            }
            else
            {
                deletecategory.IsActive = true;
                deletecategory.ModifiedDate = DateTime.Now;
                deletecategory.ModifiedBy = obj.ID;

                dbObj.Entry(deletecategory).State = System.Data.Entity.EntityState.Modified;
                dbObj.SaveChanges();
            }
            return RedirectToAction("ManageCategory");
        }
        [Authorize(Roles = "Super Admin,Admin")]
        public ActionResult ManageCountry(int? i, string Search, string sortBy)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();

            ViewBag.SortDate = string.IsNullOrEmpty(sortBy) ? "Date Desc" : "";
            ViewBag.SortCountry = sortBy == "Country" ? "Country Desc" : "Country";
            ViewBag.CountryCode = sortBy == "CountryCode" ? "CountryCode Desc" : "CountryCode";

            var record = dbObj.AddEditCountries.Where(x => x.CountryName.Contains(Search) || x.CountryCode.Contains(Search) || Search == null).ToList().AsQueryable();

            switch (sortBy)
            {
                case "Date Desc":
                    record = record.OrderByDescending(x => x.CreatedDate);
                    break;

                case "Country":
                    record = record.OrderBy(x => x.CountryName);
                    break;
                case "Country Desc":
                    record = record.OrderByDescending(x => x.CountryName);
                    break;
                case "CountryCode":
                    record = record.OrderBy(x => x.CountryCode);
                    break;
                case "CountryCode Desc":
                    record = record.OrderByDescending(x => x.CountryCode);
                    break;
                default:
                    record = record.OrderBy(x => x.CreatedDate);
                    break;
            }

            ViewBag.pic = dbObj.Admins.Where(x => x.AID == obj.ID).Select(x => x.ProfilePicture).FirstOrDefault();
            return View(record.ToPagedList(i ?? 1, 5));
        }
        public ActionResult AddCountry(int? id)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();
            var oldobj = dbObj.AddEditCountries.Where(x => x.ID == id).FirstOrDefault();
            ManageCountryModel cntryobj = new ManageCountryModel();
            if (oldobj != null)
            {
                Context.AddEditCountry viewobj = dbObj.AddEditCountries.Where(x => x.ID == id).FirstOrDefault();
                cntryobj.CountryName = viewobj.CountryName;
                cntryobj.CountryCode = viewobj.CountryCode;
                ViewBag.pic = dbObj.Admins.Where(x => x.AID == obj.ID).Select(x => x.ProfilePicture).FirstOrDefault();
                return View(cntryobj);
            }
            ViewBag.pic = dbObj.Admins.Where(x => x.AID == obj.ID).Select(x => x.ProfilePicture).FirstOrDefault();
            return View();
        }
        [HttpPost]
        public ActionResult AddCountry(Models.ManageCountryModel model)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();

            if (ModelState.IsValid || model.ID == null)
            {
                if (model.ID == null)
                {
                    Context.AddEditCountry addobj = new Context.AddEditCountry();
                    addobj.CountryName = model.CountryName;
                    addobj.CountryCode = model.CountryCode;
                    addobj.CreatedDate = DateTime.Now;
                    addobj.CreatedBy = obj.ID;
                    addobj.IsActive = true;
                    dbObj.AddEditCountries.Add(addobj);
                    dbObj.SaveChanges();
                }
                else
                {
                    var oldobj = dbObj.AddEditCountries.Where(x => x.ID == model.ID).FirstOrDefault();
                    oldobj.CountryName = model.CountryName;
                    oldobj.CountryCode = model.CountryCode;
                    oldobj.ModifiedDate = DateTime.Now;
                    oldobj.ModifiedBy = obj.ID;
                    dbObj.Entry(oldobj).State = System.Data.Entity.EntityState.Modified;
                    dbObj.SaveChanges();
                }
            }
            ModelState.Clear();
            return RedirectToAction("ManageCountry");
        }
        public ActionResult DeleteCountry(int id)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();
            
            AddEditCountry deletecountry = dbObj.AddEditCountries.Where(x => x.ID == id).FirstOrDefault();

            if (deletecountry.IsActive == true)
            {
                deletecountry.IsActive = false;
                deletecountry.ModifiedDate = DateTime.Now;
                deletecountry.ModifiedBy = obj.ID;

                dbObj.Entry(deletecountry).State = System.Data.Entity.EntityState.Modified;
                dbObj.SaveChanges();
            }
            else
            {
                deletecountry.IsActive = true;
                deletecountry.ModifiedDate = DateTime.Now;
                deletecountry.ModifiedBy = obj.ID;

                dbObj.Entry(deletecountry).State = System.Data.Entity.EntityState.Modified;
                dbObj.SaveChanges();
            }
            return RedirectToAction("ManageCountry");
        }
        [Authorize(Roles = "Super Admin,Admin")]
        public ActionResult ManageType(int? i, string Search, string sortBy)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();

            ViewBag.SortDate = string.IsNullOrEmpty(sortBy) ? "Date Desc" : "";
            ViewBag.SortType = sortBy == "Type" ? "Type Desc" : "Type";
            ViewBag.Description = sortBy == "Description" ? "Description Desc" : "Description";

            var record = dbObj.AddEditTypes.Where(x => x.Type.Contains(Search) || x.Description.Contains(Search) || Search == null).ToList().AsQueryable();

            switch (sortBy)
            {
                case "Date Desc":
                    record = record.OrderByDescending(x => x.CreatedDate);
                    break;
                case "Type":
                    record = record.OrderBy(x => x.Type);
                    break;
                case "Type Desc":
                    record = record.OrderByDescending(x => x.Type);
                    break;
                case "Description":
                    record = record.OrderBy(x => x.Description);
                    break;
                case "Description Desc":
                    record = record.OrderByDescending(x => x.Description);
                    break;
                default:
                    record = record.OrderBy(x => x.CreatedDate);
                    break;
            }

            ViewBag.pic = dbObj.Admins.Where(x => x.AID == obj.ID).Select(x => x.ProfilePicture).FirstOrDefault();
            return View(record.ToPagedList(i ?? 1, 5));
        }
        public ActionResult AddType(int? id)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();
            var oldobj = dbObj.AddEditTypes.Where(x => x.ID == id).FirstOrDefault();
            ManageTypeModel typeobj = new ManageTypeModel();
            if (oldobj != null)
            {
                Context.AddEditType viewobj = dbObj.AddEditTypes.Where(x => x.ID == id).FirstOrDefault();
                typeobj.Type = viewobj.Type;
                typeobj.Description = viewobj.Description;
                ViewBag.pic = dbObj.Admins.Where(x => x.AID == obj.ID).Select(x => x.ProfilePicture).FirstOrDefault();
                return View(typeobj);
            }
            ViewBag.pic = dbObj.Admins.Where(x => x.AID == obj.ID).Select(x => x.ProfilePicture).FirstOrDefault();
            return View();
        }
        [HttpPost]
        public ActionResult AddType(Models.ManageTypeModel model)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();
            
            if (ModelState.IsValid || model.ID == null)
            {
                if (model.ID == null)
                {
                    Context.AddEditType addobj = new Context.AddEditType();
                    addobj.Type = model.Type;
                    addobj.Description = model.Description;
                    addobj.CreatedDate = DateTime.Now;
                    addobj.CreatedBy = obj.ID;
                    addobj.IsActive = true;
                    dbObj.AddEditTypes.Add(addobj);
                    dbObj.SaveChanges();
                }
                else
                {
                    var oldobj = dbObj.AddEditTypes.Where(x => x.ID == model.ID).FirstOrDefault();
                    oldobj.Type = model.Type;
                    oldobj.Description = model.Description;
                    oldobj.ModifiedDate = DateTime.Now;
                    oldobj.ModifiedBy = obj.ID;
                    dbObj.Entry(oldobj).State = System.Data.Entity.EntityState.Modified;
                    dbObj.SaveChanges();
                }
            }
            ModelState.Clear();
            return RedirectToAction("ManageType");

        }
        public ActionResult DeleteType(int id)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();
            
            AddEditType deletetype = dbObj.AddEditTypes.Where(x => x.ID == id).FirstOrDefault();

            if (deletetype.IsActive == true)
            {
                deletetype.IsActive = false;
                deletetype.ModifiedDate = DateTime.Now;
                deletetype.ModifiedBy = obj.ID;

                dbObj.Entry(deletetype).State = System.Data.Entity.EntityState.Modified;
                dbObj.SaveChanges();
            }
            else
            {
                deletetype.IsActive = true;
                deletetype.ModifiedDate = DateTime.Now;
                deletetype.ModifiedBy = obj.ID;

                dbObj.Entry(deletetype).State = System.Data.Entity.EntityState.Modified;
                dbObj.SaveChanges();
            }
            return RedirectToAction("ManageType");
        }
        [HttpGet]
        [Authorize(Roles ="Super Admin")]
        public ActionResult ManageSystemConfiguration()
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();
            ViewBag.pic = dbObj.Admins.Where(x => x.AID == obj.ID).Select(x => x.ProfilePicture).FirstOrDefault();
            ManageSysConfigModel sysobj = new ManageSysConfigModel();
            
            Context.ManageSystemConfiguration sysviewobj = dbObj.ManageSystemConfigurations.FirstOrDefault();
            
            sysobj.SupportEmail = sysviewobj.SupportEmail;
            sysobj.SupportContactNumber = sysviewobj.SupportContactNumber;
            sysobj.EmailAddress_es = sysviewobj.EmailAddress_es;
            sysobj.FacebookURL = sysviewobj.FacebookURL;
            sysobj.TwitterURL = sysviewobj.TwitterURL;
            sysobj.LinkedInURL = sysviewobj.LinkedInURL;
            return View(sysobj);
            
        }
        [HttpPost]
        public ActionResult ManageSystemConfiguration(Models.ManageSysConfigModel model)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();

            if (ModelState.IsValid)
            {
                var oldobj = dbObj.ManageSystemConfigurations.FirstOrDefault();
                oldobj.SupportEmail = model.SupportEmail;
                oldobj.SupportContactNumber = model.SupportContactNumber;
                oldobj.EmailAddress_es = model.EmailAddress_es;
                oldobj.FacebookURL = model.FacebookURL;
                oldobj.TwitterURL = model.TwitterURL;
                oldobj.LinkedInURL = model.LinkedInURL;
                oldobj.DefaultNoteDisplayImage = "Default/1.jpg";
                oldobj.DefaultProfilePicture = "Default/Joker.jpg";
                oldobj.ModifiedDate = DateTime.Now;
                oldobj.ModifiedBy = obj.ID;
                dbObj.Entry(oldobj).State = System.Data.Entity.EntityState.Modified;
                dbObj.SaveChanges();

               
            }
            ModelState.Clear();
            return RedirectToAction("ManageSystemConfiguration");

        }
        [NonAction]
        public void SendAcknowledgeAdminEmail(string adminEmailID, string adminFirstName, string adminpassword)
        {
            var fromEmail = new MailAddress(dbObj.ManageSystemConfigurations.FirstOrDefault().SupportEmail);         //Company (System) EmailID
            var toEmail = new MailAddress(adminEmailID);                     // Receiving Admin emailid
            var fromEmailPassword = dbObj.ManageSystemConfigurations.FirstOrDefault().SupportPassword;               //Company email Actual Password
            string subject = "Notes MarketPlace - Email Verification";

            string body = "Hello " + adminFirstName + "," +
                "<br/><br/>Thank you for signing up with us. Please click on below link to verify your email address and to login" +
                "<br/><br/>Your Username: " + adminEmailID + 
                "<br/><br/>Password: " + adminpassword +
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