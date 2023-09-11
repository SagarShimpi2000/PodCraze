using PodCraze.Data;
using PodCraze.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PodCraze.Controllers
{
    public class AdminController : Controller
    {
        PodCrazeEntities2 Db = new PodCrazeEntities2();

        // GET: Admin
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            var getAdmin = Db.Admins.Where(x => x.Username.Equals(model.Username) && x.Password.Equals(model.Password)).FirstOrDefault();
            if (getAdmin != null)
            {
                Session["Name"]=getAdmin.Name.ToString();
                Session["Username"]=getAdmin.Username.ToString();
                Session["Email"]=getAdmin.Email.ToString();

                return RedirectToAction("Profile");
            }
            return View();
        }

        public ActionResult Profile()
        {
            if (Session["Username"] != null)
            {
                var listener = Db.Listeners.ToList();
                var narrator = Db.Narrators.ToList();
                var freePod = Db.Podcasts.Where(x => x.IsPaid == 0);
                var paidPod = Db.Podcasts.Where(x => x.IsPaid == 1);

                Session["ListenerCount"] = listener.Count();
                Session["NarratorCount"] = narrator.Count();
                int a = freePod.Count();
                int b = paidPod.Count();
                Session["FreePod"] = a;
                Session["PaidPod"] = b;
                Session["TotalPod"] = a + b;

                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult ListenersList()
        {
            if (Session["Username"] != null)
            {
                ListenerViewModel model = new ListenerViewModel();

                var listenerList = Db.Listeners.ToList();
                foreach (var listener in listenerList)
                {
                    ListenerViewModel e = new ListenerViewModel();
                    e.Name = listener.Name;
                    e.Username = listener.Username;
                    e.Password = listener.Password;
                    e.Email = listener.Email;
                    e.Contact = listener.Contact;
                    e.Gender = listener.Gender;
                    model.ListenerList.Add(e);
                }
                return View(model);
            }
            return RedirectToAction("Login");
        }

        public ActionResult DeleteListener(string username)
        {
            var listener = Db.Listeners.FirstOrDefault(x => x.Username == username);
            Db.Listeners.Remove(listener);
            Db.SaveChanges();

            return RedirectToAction("ListenersList");
        }

        public ActionResult ListenerRequestList()
        {
            if (Session["Username"] != null)
            {
                ListenerReqViewModel model = new ListenerReqViewModel();

                var listenerReqList = Db.ListenerReqs.Where(x => x.Status == 0).ToList();
                foreach (var listenerReq in listenerReqList)
                {
                    ListenerReqViewModel obj = new ListenerReqViewModel();
                    obj.Id = listenerReq.Id;
                    obj.Name = listenerReq.Name.ToString();
                    obj.Image = "/Data/Images/" + listenerReq.Image;
                    obj.Pdf = "/Data/PDFs/" + listenerReq.Pdf;
                    obj.Amount = listenerReq.Amount;
                    obj.ListenerName = listenerReq.Listener_username.ToString();
                    obj.NarratorName = (listenerReq.Narrator_username != null) ? listenerReq.Narrator_username.ToString() : "None";
                    obj.StatusValue = listenerReq.Status == 1 ? "Accepted" : "Pending";

                    model.ListenerReqList.Add(obj);
                }
                return View(model);
            }
            return RedirectToAction("Login");
        }

        public ActionResult DeleteBookReq(int id)
        {
            if (id != 0)
            {
                var req = Db.ListenerReqs.FirstOrDefault(x => x.Id == id);

                // Db.ListenerReqs.Remove(req);

                req.Status = 2; // 2 means admin denied req
                Db.SaveChanges();
            }
            return RedirectToAction("ListenerRequestList");
        }



        public ActionResult NarratorsList()
        {
            if (Session["Username"] != null)
            {
                NarratorViewModel model = new NarratorViewModel();

                var getNarratorsList = Db.Narrators.Where(x => x.Status != 0 && x.Status != 3).ToList();
                foreach (var narrator in getNarratorsList)
                {
                    NarratorViewModel e = new NarratorViewModel();
                    e.Name = narrator.Name;
                    e.Username = narrator.Username;
                    e.Password = narrator.Password;
                    e.Email = narrator.Email;
                    e.Contact = narrator.Contact;
                    e.Gender = narrator.Gender;
                    e.StatusValue = narrator.Status == 1 ? "Active" : (narrator.Status == 2 ? "Inactive" : "Pending");

                    model.NarratorList.Add(e);
                }
                return View(model);
            }
            return RedirectToAction("Login");
        }

        public ActionResult DeleteNarrator(string username)
        {
            if (username != null)
            {
                var narrator = Db.Narrators.FirstOrDefault(x => x.Username.Equals(username));

                // Db.Narrators.Remove(narrator);

                narrator.Status = 3;  // 3 means deleted by admin 
                Db.SaveChanges();
            }
            return RedirectToAction("NarratorsList");
        }

        public ActionResult NarratorsRequestList()
        {
            if (Session["Username"] != null) 
            {
                NarratorViewModel model = new NarratorViewModel();

                var narratorList = Db.Narrators.Where(x => x.Status == 0).ToList();
                foreach (var narrator in narratorList)
                {
                    NarratorViewModel e = new NarratorViewModel();
                    e.Name = narrator.Name;
                    e.Username = narrator.Username;
                    e.Password = narrator.Password;
                    e.Email = narrator.Email;
                    e.Contact = narrator.Contact;
                    e.Gender = narrator.Gender;

                    model.NarratorList.Add(e);
                }
                return View(model);
            }
            return RedirectToAction("Login");
        }

        public ActionResult AcceptNarrator(string username)
        {
            if (username != null)
            {
                var narrator = Db.Narrators.FirstOrDefault(x => x.Username.Equals(username));
                narrator.Status = 1;
                Db.SaveChanges();
            }
            return RedirectToAction("NarratorsRequestList");
        }

        public ActionResult RejectNarrator(string username)
        {
            if(username != null)
            {
                var narrator = Db.Narrators.FirstOrDefault(x => x.Username.Equals(username));
                narrator.Status = 2;
                Db.SaveChanges();
            }
            return RedirectToAction("NarratorsRequestList");
        }

        public ActionResult Logout()
        {
           // Session.Clear();
            FormsAuthentication.SignOut();

            return RedirectToAction("Login");
        }
    }
}