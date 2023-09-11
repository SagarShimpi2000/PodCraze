using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PodCraze.Models
{
    public class NarratorViewModel
    {
        public NarratorViewModel() 
        {
            NarratorList = new List<NarratorViewModel>();

        }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Contact is required")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string Contact { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }
        
        public String StatusValue { get; set; }
        public String Description { get; set; }

        // [Required, Microsoft.Web.Mvc.FileExtensions(Extensions = "csv",ErrorMessage = "Specify a CSV file. (Comma-separated values)")]
       public HttpPostedFileBase Voice { get; set; }

        public List<NarratorViewModel> NarratorList { get; set; }
    


}
}