using Notes_MarketPlace.Context;
using Notes_MarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net;
using System.Web.Security;

namespace Notes_MarketPlace.Controllers
{
    public class UserLoginController : Controller
    {
        NotesMarketPlaceEntities8 dbObj = new NotesMarketPlaceEntities8();
        [HttpGet]
        [Route("UserLogin/Login")]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [Route("userLogin/Login")]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLoginModel login, string ReturnUrl = "")
        {
            using (NotesMarketPlaceEntities8 dbObj = new NotesMarketPlaceEntities8())
            {
                var v = dbObj.Users.Where(a => a.EmailID == login.EmailID).FirstOrDefault();
                if (v != null)
                {
                    if(v.IsActive == true)
                    {
                        if(v.IsEmailVerified == true) 
                        {
                            if (string.Compare(login.Password, v.Password) == 0)
                            {
                                int timeout = login.RememberMe ? 525600 : 20;
                                var ticket = new FormsAuthenticationTicket(login.EmailID, login.RememberMe, timeout);
                                string encrypted = FormsAuthentication.Encrypt(ticket);
                                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                                cookie.Expires = DateTime.Now.AddMinutes(timeout);
                                cookie.HttpOnly = true;
                                Response.Cookies.Add(cookie);

                                var upobj = dbObj.UserProfiles.Where(a => a.UserID == v.ID).FirstOrDefault();
                                if (upobj == null)
                                {
                                    if (v.RoleID == 2 || v.RoleID == 1)
                                    {
                                        return RedirectToAction("Dashboard", "AdminDashboard");
                                    }
                                    return RedirectToAction("UserProfile", "Profile");
                                }
                                else if (!String.IsNullOrEmpty(ReturnUrl))
                                {
                                    return Redirect(ReturnUrl);
                                }
                                else
                                {
                                    if (v.RoleID == 2 || v.RoleID == 1)
                                    {
                                        return RedirectToAction("Dashboard", "AdminDashboard");
                                    }
                                    return RedirectToAction("SearchNotes", "Home");
                                }

                                /*if (Url.IsLocalUrl(ReturnUrl))
                                {
                                    return Redirect(ReturnUrl);
                                }
                                else
                                {
                                    return RedirectToAction("UserProfile", "Profile");
                                }*/
                            }
                            else
                            {
                                ModelState.AddModelError("Password", "Invalid Password");
                                return View(login);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("EmailID","Email Address is not verified");
                            return View(login);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("EmailID", "This user is not active");
                        return View(login);
                    }                    
                }
                else
                {
                    ModelState.AddModelError("EmailID","Invalid Email Address");
                    return View(login);
                }
            }            
        }
        
        [Route("UserLogin/Logout")]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        [HttpGet]
        [Route("UserLogin/ForgotPassword")]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [Route("UserLogin/ForgotPassword")]
        public ActionResult ForgotPassword(Models.ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var isExist = IsEmailExist(model.EmailID);
                if (isExist == false)
                {
                    ModelState.AddModelError("EmailID", "Email Does not exist");
                    return View(model);
                }
                else
                {
                    Context.User obj = dbObj.Users.Where(x => x.EmailID == model.EmailID).FirstOrDefault();
                    string pwd = Membership.GeneratePassword(10, 2);
                    obj.Password = pwd;
                    dbObj.SaveChanges();
                    SendPasswordEmail(obj.EmailID, pwd);
                    ViewBag.msg = "New password has been sent to your email";
                }
            }
            ModelState.Clear();
            return RedirectToAction("Login");
        }
        [Authorize]
        [HttpGet]
        [Route("UserLogin/ChangePassword")]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [Route("UserLogin/ChangePassword")]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult ChangePassword(Models.ChangePasswordModel model)
        {
            var emailid = User.Identity.Name.ToString();
            var v = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();
            Context.User obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (v != null)
                {
                    if (string.Compare(model.OldPassword, v.Password) == 0)
                    {
                        obj.Password = model.NewPassword;
                        dbObj.SaveChanges();
                    }
                    else
                    {
                        ModelState.AddModelError("Password","Old Password does not match with Current Password");
                    }
                }
                else
                {
                    ModelState.AddModelError("EmailID","Invalid  Email Address");
                }
            }
            ModelState.Clear();
            return RedirectToAction("Login");
        }
        [HttpGet]
        [Route("UserLogin/SignUp")]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        [Route("UserLogin/CreateUser")]
        public ActionResult CreateUser(Models.User model)
        {
            if (ModelState.IsValid)
            {
                var isExist = IsEmailExist(model.EmailID);
                if (isExist)
                {
                    ModelState.AddModelError("EmailID", "Email Does not exist");
                    return View(model);
                }
                Context.User obj = new Context.User();
                obj.RoleID = 3;
                obj.FirstName = model.FirstName;
                obj.LastName = model.LastName;
                obj.EmailID = model.EmailID;
                obj.Password = model.Password;
                obj.IsEmailVerified = false;
                obj.CreatedDate = DateTime.Now;
                obj.IsActive = false;
                //obj.ActivationCode = Guid.NewGuid();
                dbObj.Users.Add(obj);
                dbObj.SaveChanges();

                TempData["Success"] = "Your Account Is Created Successfully";
                SendVerificationLinkEmail(obj.EmailID,obj.FirstName); 
            }
            return RedirectToAction("SignUp");
        }
        
        [NonAction]
        public bool IsEmailExist(string EmailID)
        {
            using (NotesMarketPlaceEntities8 dbObj = new NotesMarketPlaceEntities8())
            {
                var v = dbObj.Users.Where(a => a.EmailID == EmailID).FirstOrDefault();
                return v != null;
            }
        }
        [NonAction]
        public void SendPasswordEmail(string EmailID, string pwd)
        {

            var fromEmail = new MailAddress(dbObj.ManageSystemConfigurations.FirstOrDefault().SupportEmail);
            var toEmail = new MailAddress(EmailID);
            var fromEmailPassword = dbObj.ManageSystemConfigurations.FirstOrDefault().SupportPassword;
            string subject = "New Temporary Password has been created for you";

            string body = "Hello," +
                "<br/><br/>We have generated a new password for you." +
                "<br/>Password: " + pwd +
                "<br/><br/>Regards,<br/>Notes Marketplace";

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
        public void SendVerificationLinkEmail(string EmailID, string FirstName)
        {
            var obj = dbObj.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();
            var verifyUrl = "/UserLogin/EmailVerification?emailid=" + obj.EmailID;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress(dbObj.ManageSystemConfigurations.FirstOrDefault().SupportEmail);
            var toEmail = new MailAddress(EmailID);
            var fromEmailPassword = dbObj.ManageSystemConfigurations.FirstOrDefault().SupportPassword;
            string subject = "Notes MarketPlace - Email Verification";

            string body = "Hello " + FirstName + "," +
                "<br/><br/>Thank you for signing up with us. Please click on below link to verify your email address and to login" +
                "<br/><br/><a href='" + link + "'>" + link + "</a> " +
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
        [Route("UserLogin/EmailVerification")]
        public ActionResult EmailVerification(string emailid)
        {
            var obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();
            TempData["FirstName"] = obj.FirstName;
            return View(obj);
        }
        [Route("UserLogin/VerifyEmail")]
        public ActionResult VerifyEmail(string emailid)
        {
            var obj = dbObj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();
            obj.IsEmailVerified = true;
            obj.IsActive = true;
            dbObj.SaveChanges();
            return RedirectToAction("Login");
        }
    }
}