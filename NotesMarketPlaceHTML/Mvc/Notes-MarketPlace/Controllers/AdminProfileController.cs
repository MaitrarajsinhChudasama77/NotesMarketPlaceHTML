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
    public class AdminProfileController : Controller
    {
        NotesMarketPlaceEntities8 dbObj = new NotesMarketPlaceEntities8();
        [Authorize(Roles = "Super Admin,Admin")]
        public ActionResult MyProfile()
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();
            ViewBag.pic = dbObj.Admins.Where(x => x.AID == obj.ID).Select(x => x.ProfilePicture).FirstOrDefault();

            MyProfileModel proobj = new MyProfileModel();
            var oldobj = dbObj.Users.Where(x => x.ID == obj.ID).FirstOrDefault();

            if (oldobj != null)
            {
                proobj.UserID = obj.ID;
                proobj.FirstName = obj.FirstName;
                proobj.LastName = obj.LastName;
                proobj.EmailID = obj.EmailID;

                var upobj = dbObj.Admins.Where(x => x.AID == obj.ID).FirstOrDefault();
                
                proobj.PhoneNumber_CountryCode = upobj.PhoneNumber_CountryCode;
                proobj.PhoneNumber = upobj.PhoneNumber;
                proobj.SecondaryEmail = upobj.SecondaryEmail;

                ViewBag.PhoneNumber_CountryCode = new SelectList(dbObj.AddEditCountries, "CountryCode", "CountryCode");//Value,text
                return View(proobj);
            }
            else
            {
                proobj.UserID = obj.ID;
                proobj.FirstName = obj.FirstName;
                proobj.LastName = obj.LastName;
                proobj.EmailID = obj.EmailID;
                
                ViewBag.PhoneNumber_CountryCode = new SelectList(dbObj.AddEditCountries, "CountryCode", "CountryCode");//Value,text
                
                return View(proobj);
            }
        }
        [HttpPost]
        public ActionResult MyProfile(MyProfileModel model)
        {
            var emailid = User.Identity.Name.ToString();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();
            if (ModelState.IsValid)
            {
                var oldobj = dbObj.Admins.Where(x => x.AID == obj.ID).FirstOrDefault();

                string path = Path.Combine(Server.MapPath("~/Members"), obj.ID.ToString());

                //Checking for directory

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (oldobj == null) // User adding profile details first time
                {
                    //Saving into database

                    Context.Admin adminobj = new Context.Admin();
                    //addnoteobj.ID = model.ID;
                    adminobj.AID = obj.ID;
                    adminobj.PhoneNumber_CountryCode = model.PhoneNumber_CountryCode;
                    adminobj.PhoneNumber = model.PhoneNumber;
                    adminobj.SecondaryEmail = model.SecondaryEmail;
                    adminobj.CreatedDate = DateTime.Now;
                    adminobj.CreatedBy = obj.ID;
                    adminobj.IsActive = true;
                    dbObj.Admins.Add(adminobj);
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
                        adminobj.ProfilePicture = Path.Combine(("Members/" + obj.ID + "/"), displayimagename);
                        dbObj.SaveChanges();
                    }
                    else
                    {
                        adminobj.ProfilePicture = "/Default/Joker.jpg";
                        dbObj.SaveChanges();
                    }
                }
                else //editing old admin profile
                {
                    //Saving into database

                    Context.Admin editadminobj = dbObj.Admins.Where(x => x.AID == obj.ID).FirstOrDefault();
                    Context.User userobj1 = dbObj.Users.Where(x => x.ID == obj.ID).FirstOrDefault();

                    userobj1.FirstName = model.FirstName;
                    userobj1.LastName = model.LastName;
                    userobj1.EmailID = model.EmailID;
                   
                    editadminobj.PhoneNumber_CountryCode = model.PhoneNumber_CountryCode;
                    editadminobj.PhoneNumber = model.PhoneNumber;
                    editadminobj.SecondaryEmail = model.SecondaryEmail;
                    editadminobj.ModifiedDate = DateTime.Now;
                    editadminobj.ModifiedBy = obj.ID;
                    dbObj.Entry(editadminobj).State = System.Data.Entity.EntityState.Modified;
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
                        editadminobj.ProfilePicture = Path.Combine(("Members/" + obj.ID + "/"), displayimagename);
                        dbObj.SaveChanges();
                    }
                    else
                    {
                        editadminobj.ProfilePicture = "Default/Joker.jpg";
                        dbObj.SaveChanges();
                    }
                }
                return RedirectToAction("Dashboard", "AdminDashboard");
            }
            
            ViewBag.PhoneNumber_CountryCode = new SelectList(dbObj.AddEditCountries, "CountryCode", "CountryCode");//Value,text
            
            return View();
        }
        [Authorize(Roles = "Super Admin,Admin")]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login","UserLogin");
        }
    }
}