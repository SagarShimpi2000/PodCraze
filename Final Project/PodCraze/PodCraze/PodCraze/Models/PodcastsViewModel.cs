using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PodCraze.Models
{
    public class PodcastsViewModel
    {
        public PodcastsViewModel()
        {
            PodcastList = new List<PodcastsViewModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public int IsPaid { get; set; }
        public string NarratorName { get; set; }
        public string Image { get; set; }
        public string Pdf { get; set; }
        public string Audio { get; set; }
        public string Description { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        public HttpPostedFileBase PdfFile { get; set; }
        public HttpPostedFileBase AudioFile { get; set; }

        public List<PodcastsViewModel> PodcastList { get; set; }
    }
}