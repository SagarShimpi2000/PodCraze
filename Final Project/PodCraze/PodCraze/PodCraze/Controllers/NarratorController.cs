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
    public class NarratorController : Controller
    {
        PodCrazeEntities2 Db = new PodCrazeEntities2();

        // GET: Narrator
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(NarratorViewModel model)
        {
            if (model.Name != "" && model.Name != null)
            {
                //Add New listener data into database table
                Narrator NarratorObject = new Narrator();

                NarratorObject.Name = model.Name;
                NarratorObject.Username = model.Username;
                NarratorObject.Password = model.Password;
                NarratorObject.Email = model.Email;
                NarratorObject.Contact = model.Contact;
                NarratorObject.Gender = model.Gender;
               // NarratorObject.Status = "No";

                Db.Narrators.Add(NarratorObject);
                Db.SaveChanges();
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


        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            var getNarrator = Db.Narrators.Where(x => x.Username.Equals(model.Username) && x.Password.Equals(model.Password) && x.Status == 2).FirstOrDefault();
            if (getNarrator != null)
            {
                Session["Name"] = getNarrator.Name.ToString();
                Session["Username"] = getNarrator.Username.ToString();
                Session["Email"] = getNarrator.Email.ToString();
                
                if(getNarrator.Discription != null)
                Session["Description"] = getNarrator.Discription.ToString();
                Session["Gender"] = getNarrator.Gender.ToString();
 
                return RedirectToAction("Profile");
            }
            return View();
        }

        public ActionResult Profile()
        {
            if (Session["Username"] != null)
            {
                var narrator = Session["Username"];
                var freePod = Db.Podcasts.Where(x => x.Narrator_username.Equals(narrator.ToString()) && x.IsPaid == 0);
                var paidPod = Db.Podcasts.Where(x => x.Narrator_username.Equals(narrator.ToString()) && x.IsPaid == 1);
                
                int a = freePod.Count();
                int b = paidPod.Count();
                Session["FreePod"] = a;
                Session["PaidPod"] = b;
                Session["TotalPod"] = a+b;
                
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult AddPodcast()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddPodcast(PodcastsViewModel model)
        {
            if (model.ImageFile != null && model.ImageFile.ContentLength > 0)
            {
                if (model.PdfFile != null && model.PdfFile.ContentLength > 0)
                {
                    if (model.AudioFile != null && model.AudioFile.ContentLength > 0)
                    {
                        var imageFileName = Path.GetFileName(model.ImageFile.FileName);
                        var imagePath = Path.Combine(Server.MapPath("~/Data/Images/"), imageFileName);
                        model.ImageFile.SaveAs(imagePath);

                        var pdfFileName = Path.GetFileName(model.PdfFile.FileName);
                        var pdfPath = Path.Combine(Server.MapPath("~/Data/PDFs/"), pdfFileName);
                        model.PdfFile.SaveAs(pdfPath);
                        
                        var audioFileName = Path.GetFileName(model.AudioFile.FileName);
                        var audioPath = Path.Combine(Server.MapPath("~/Data/Audio/"), audioFileName);
                        model.AudioFile.SaveAs(audioPath);

                        Podcast podObject = new Podcast();

                        podObject.Name = model.Name;
                        podObject.Narrator_username = Session["Username"].ToString();
                        podObject.Image = imageFileName;
                        podObject.Pdf = pdfFileName;
                        podObject.Audio = audioFileName;
                        if(model.IsPaid == 1){
                            podObject.Amount = model.Amount;
                        }
                        else{
                            podObject.Amount = 0;
                        }
                        Db.Podcasts.Add(podObject);
                        Db.SaveChanges();

                        return View();
                    }
                }
            }
            return RedirectToAction("ReqFail", "Listener");
        }

        public ActionResult PodRequestList()
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

        public ActionResult AcceptReq(int id)
        {
                var req = Db.ListenerReqs.FirstOrDefault(x => x.Id == id);
                
                req.Status = 1;
                req.Narrator_username = Session["Username"].ToString();
                Db.SaveChanges();
            return RedirectToAction("PodRequestList");
        }

        public ActionResult TodoList()
        {
           if( Session["Username"] != null)
            {
                ListenerReqViewModel model = new ListenerReqViewModel();
                string a = Session["Username"].ToString();
                var todoList = Db.ListenerReqs.Where(x => x.Listener_username.Equals(a)).ToList();

                foreach ( var todo in todoList )
                {
                    ListenerReqViewModel obj = new ListenerReqViewModel();
                    obj.Id = todo.Id;
                    obj.Name = todo.Name.ToString();
                    obj.Image = "/Data/Images/" + todo.Image;
                    obj.Pdf = "/Data/PDFs/" + todo.Pdf;
                    obj.Amount = todo.Amount;
                    obj.ListenerName = todo.Listener_username.ToString();

                    model.ListenerReqList.Add(obj);
                }
                return View(model);
            }
            return RedirectToAction("Login");
        }

        public ActionResult UploadWork(int id)
        {
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