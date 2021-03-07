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
        NotesMarketPlaceEntities1 dbObj = new NotesMarketPlaceEntities1();
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
            string message = "";
            using (NotesMarketPlaceEntities1 dbObj = new NotesMarketPlaceEntities1())
            {
                var v = dbObj.Users.Where(a => a.EmailID == login.EmailID).FirstOrDefault();
                if (v != null)
                {
                    if (string.Compare(login.Password, v.Password) == 0)
                    {
                        int timeout = login.RememberMe ? 525600 : 20; // 525600 min = 1 year
                        var ticket = new FormsAuthenticationTicket(login.EmailID, login.RememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);


                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Dashboard", "Home");
                        }
                    }
                    else
                    {
                        message = "Invalid credential provided";
                    }
                }
                else
                {
                    message = "Invalid credential provided";
                }
            }
            ViewBag.Message = message;
            return View();
        }
        
        [Route("UserLogin/Logout")]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        [HttpGet]
        [Route("UserLogin/ForgotPassword")]
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
                Context.User obj = dbObj.Users.Where(x => x.EmailID == model.EmailID).FirstOrDefault();
                string pwd = Membership.GeneratePassword(9, 2);
                obj.Password = pwd;
                dbObj.SaveChanges();
                SendPasswordEmail(obj.EmailID, pwd);
                TempData["Success"] = "New password has been sent to your email";
            }
            ModelState.Clear();
            return RedirectToAction("Login");
        }
        [Route("UserLogin/ChangePassword")]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpGet]
        [Route("UserLogin/SignUp")]
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
                Context.User obj = new Context.User();
                obj.RoleID = 3;
                obj.FirstName = model.FirstName;
                obj.LastName = model.LastName;
                obj.EmailID = model.EmailID;
                obj.Password = model.Password;
                obj.IsEmailVerified = false;
                obj.IsActive = true;
                obj.ActivationCode = Guid.NewGuid();
                dbObj.Users.Add(obj);
                dbObj.SaveChanges();

                SendVerificationLinkEmail(model.EmailID, model.FirstName,model.ActivationCode.ToString());
                TempData["Success"] = "Your Account Is Created Successfully"; 
            }
            ModelState.Clear();
            return RedirectToAction("SignUp");
        }
        [Route("UserLogin/EmailVerification")]
        public ActionResult EmailVerification(Models.User model,string code)
        {
            Context.User obj = dbObj.Users.Where(x=>x.ActivationCode.ToString()==code).FirstOrDefault();
            ViewBag.name = obj.FirstName;
            return View();
        }
        [Route("UserLogin/VerifyEmail")]
        public ActionResult VerifyEmail(string code)
        {
            Context.User obj = dbObj.Users.Where(x=>x.ActivationCode.ToString()==code).FirstOrDefault();
            obj.IsEmailVerified = true;
            dbObj.SaveChanges();
            return RedirectToAction("Login");
        }
        [NonAction]
        public bool IsEmailExist(string EmailID)
        {
            using (NotesMarketPlaceEntities1 dbObj = new NotesMarketPlaceEntities1())
            {
                var v = dbObj.Users.Where(a => a.EmailID == EmailID).FirstOrDefault();
                return v != null;
            }
        }
        [NonAction]
        public void SendPasswordEmail(string EmailID, string pwd)
        {
            var fromEmail = new MailAddress("thehamojha@gmail.com");
            var toEmail = new MailAddress(EmailID);
            var fromEmailPassword = "244466666";
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
        public void SendVerificationLinkEmail(string EmailID, string FirstName, string ActivationCode)
        {
            var verifyUrl = "/UserLogin/EmailVerification/";
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("thehamojha@gmail.com");
            var toEmail = new MailAddress(EmailID);
            var fromEmailPassword = "244466666";
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
    }
}