using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Podcast.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult HomePage()
        {
            return View();
        }

        public ActionResult AdminLogin()
        {
            return View();
        }

        public ActionResult ListnerLogin()
        {
            return View();
        }

        public ActionResult CreatorLogin()
        {
            return View();
        }

    }
}