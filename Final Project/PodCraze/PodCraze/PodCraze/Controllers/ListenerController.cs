using PodCraze.Data;
using PodCraze.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PodCraze.Controllers
{
    public class ListenerController : Controller
    {
        PodCrazeEntities2 Db = new PodCrazeEntities2();

        // GET: Listener
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(ListenerViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Add New listener data into database table
                Listener ListenerObject = new Listener();

                ListenerObject.Name = model.Name;
                ListenerObject.Username = model.Username;
                ListenerObject.Password = model.Password;
                ListenerObject.Email = model.Email;
                ListenerObject.Contact = model.Contact;
                ListenerObject.Gender = model.Gender;
                ListenerObject.Requests = 0;

                Db.Listeners.Add(ListenerObject);
                Db.SaveChanges();
                return RedirectToAction("Login");
            }
            // we have to redirect the user to some page
            //
            //
            return RedirectToAction("Registration");
            //
            //
            //need to change return 
        }

        public ActionResult Login()
        {
            return View();
        }


        //[ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            var getListener = Db.Listeners.Where(x => x.Username.Equals(model.Username) && x.Password.Equals(model.Password)).FirstOrDefault();
            if (getListener != null)
            {
                Session["Name"] = getListener.Name.ToString();
                Session["Username"] = getListener.Username.ToString();
                Session["Email"] = getListener.Email.ToString();

                return RedirectToAction("Profile");
            }
            return View();
        }

        public ActionResult Profile()
        {
            if (Session["Username"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult DoRequest()
        {
            if (Session["Username"] != null)
            {
                return View();
            }
            return RedirectToAction("Login");
        }

        [HttpPost]
        public ActionResult DoRequest(ListenerReqViewModel model)
        {
            if (model.ImageFile != null && model.ImageFile.ContentLength > 0)
            {
                var imageFileName = Path.GetFileName(model.ImageFile.FileName);
                var imagePath = Path.Combine(Server.MapPath("~/Data/Images/"), imageFileName);
                model.ImageFile.SaveAs(imagePath);
                
                if (model.PdfFile != null && model.PdfFile.ContentLength > 0)
                {
                    var pdfFileName = Path.GetFileName(model.PdfFile.FileName);
                    var pdfPath = Path.Combine(Server.MapPath("~/Data/PDFs/"), pdfFileName);
                    model.PdfFile.SaveAs(pdfPath);

                    ListenerReq reqObject = new ListenerReq();

                    reqObject.Name = model.Name;
                    reqObject.Listener_username = Session["Username"].ToString();
                    reqObject.Image = imageFileName;
                    reqObject.Pdf = pdfFileName;
                    reqObject.Amount = model.Amount;

                    Db.ListenerReqs.Add(reqObject);
                    Db.SaveChanges();

                    return View();
                }
            }
            return RedirectToAction("ReqFail");
        }

        public ActionResult RequestList()
        {
            if (Session["Username"] != null)
            {
                ListenerReqViewModel model = new ListenerReqViewModel();
                var uname = Session["Username"].ToString();
                var reqlist = Db.ListenerReqs.Where(x => x.Listener_username.Equals(uname) && x.Status != 2);
                
                if(reqlist != null) 
                {
                    foreach (var req in reqlist) 
                    {
                        ListenerReqViewModel obj = new ListenerReqViewModel();
                        obj.Id = req.Id;
                        obj.Name = req.Name.ToString();
                        obj.Image = "/Data/Images/" + req.Image;
                        obj.Pdf = "/Data/PDFs/" + req.Pdf;
                        obj.Amount = req.Amount;
                        obj.ListenerName = req.Listener_username.ToString();
                        obj.NarratorName = (req.Narrator_username != null) ? req.Narrator_username.ToString() : "No response yet";
                        obj.StatusValue = req.Status != 0 ? (req.Status == 1 ? "Accepted" : "Admin_Denied") : "Pending";

                        model.ListenerReqList.Add(obj);
                    }
                    return View(model);
                }
            }
            return RedirectToAction("Login");
        }

        public ActionResult DeleteReq(int reqId)
        {
            if (Session["Username"] != null)
            { 
                if (reqId != 0)
                {
                    var req = Db.ListenerReqs.FirstOrDefault(x => x.Id == reqId);
                    Db.ListenerReqs.Remove(req);
                    Db.SaveChanges();
                }
                return RedirectToAction("RequestList");
            }
            return RedirectToAction("Login");
        }

        public ActionResult Logout()
        {
            // Session.Clear();
            FormsAuthentication.SignOut();

            return RedirectToAction("Login");
        }


    }
}