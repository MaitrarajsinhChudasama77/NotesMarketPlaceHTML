using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Notes_MarketPlace.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult MyDownloads()
        {
            return View();
        }
        public ActionResult MySoldNotes()
        {
            return View();
        }
        public ActionResult MyRejectedNotes()
        {
            return View();
        }
        public ActionResult UserProfile()
        {
            return View();
        }
    }
}