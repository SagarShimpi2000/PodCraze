using PodCraze.Data;
using PodCraze.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PodCraze.Controllers
{
    public class HomeController : Controller
    {
        PodCrazeEntities2 Db = new PodCrazeEntities2();
        // GET: Home
        public ActionResult Index()
        {
            PodcastsViewModel model = new PodcastsViewModel();

            var podList = Db.Podcasts.ToList();
            foreach (var pod in podList)
            {
                PodcastsViewModel obj = new PodcastsViewModel();
                obj.Id = pod.Id;
                obj.Name = pod.Name;
                obj.Image = "/Data/Images/" + pod.Image;
                obj.NarratorName = pod.Narrator.Name;

                model.PodcastList.Add(obj);
            }
            return View(model);
        } 
        
        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(string searchPod)
        {
            PodcastsViewModel model = new PodcastsViewModel();
            if (!string.IsNullOrWhiteSpace(searchPod))
            {
                //var podList = Db.Podcasts.Where(x => x.Name.IndexOf(searchPod, StringComparison.OrdinalIgnoreCase)>=0 || 
                                                  //  x.Narrator.Name.IndexOf(searchPod, StringComparison.OrdinalIgnoreCase)>=0).ToList();
                var podList = Db.Podcasts.Where(x => x.Name.Contains(searchPod));

                foreach (var pod in podList)
                {
                    PodcastsViewModel obj = new PodcastsViewModel();
                    obj.Id = pod.Id;
                    obj.Name = pod.Name;
                    obj.Image = pod.Image;
                    obj.NarratorName = pod.Narrator.Name;
                    
                    model.PodcastList.Add(obj);
                }
                ViewBag.SearchTerm = searchPod;
            }
            return View(model);
        }

        public ActionResult Player(int id)
        {
            PodcastsViewModel model = new PodcastsViewModel();

            if (id > 0)
            {
                var pod = Db.Podcasts.FirstOrDefault(x => x.Id == id);
                model.Image = "/Data/Images/" + pod.Image;
                model.Audio = "/Data/Audio/" + pod.Audio;
                model.Pdf = "/Data/PDFs/" + pod.Pdf;
            }
            
            return View(model);
        }

    }
}


