using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PodCraze.Models
{
    public class AdminViewModel
    {
        public AdminViewModel() { }
        public String Name { get; set; }
        public String Email { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
    }
}