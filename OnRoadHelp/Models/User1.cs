using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnRoadHelp.Models
{
    [MetadataType(typeof(NewsAttribs))]
    public partial class User
    {
        // leave it empty.
        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Please enter confirm password")]
        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
        [DataType(DataType.Password)]
        public string Confirmpwd { get; set; }
    }

    public class NewsAttribs
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Enter First Name")]
        [Display(Name = "First Name")]
        public string First_Name { get; set; }
        [Required(ErrorMessage = "Enter Last Name")]
        [Display(Name = "Last Name")]
        public string Last_Name { get; set; }
        [Required(ErrorMessage = "Enter Email")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Password \"{0}\" must have {2} character", MinimumLength = 3)]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "Enter Phone Num")]
        [Display(Name = "Enter Phone Number")]
        public string PhoneNumbers { get; set; }

    }
}
