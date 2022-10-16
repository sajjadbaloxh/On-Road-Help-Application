using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OnRoadHelp.Models
{
    public class adminlogin
    {
        public int  id{ get; set; }
        [Required(ErrorMessage = "Enter Email")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "Password \"{0}\" must have {2} character", MinimumLength = 3)]
        [RegularExpression(@"^([a-zA-Z0-9@*#]{3,15})$", ErrorMessage = "Password must contain: Minimum 3 characters atleast 1 UpperCase Alphabet, 1 LowerCase      Alphabet, 1 Number and 1 Special Character")]
        public string Password { get; set; }
    }
}