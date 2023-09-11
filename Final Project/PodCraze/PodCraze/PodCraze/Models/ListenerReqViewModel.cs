using PodCraze.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PodCraze.Models
{
    public class ListenerReqViewModel
    {
        public ListenerReqViewModel() 
        {
            ListenerReqList = new List<ListenerReqViewModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public string  ListenerName { get; set; }
        public string  NarratorName { get; set; }
        public string StatusValue { get; set; }
        public string Image { get; set; }
        public string Pdf { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        public HttpPostedFileBase PdfFile { get; set; }

        public List<ListenerReqViewModel> ListenerReqList { get; set; }
    }
}