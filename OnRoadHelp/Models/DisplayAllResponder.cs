using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnRoadHelp.Models
{
    public class DisplayAllResponder
    {
        public int id { get; set; }
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
     
      
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Service Name")]
        public string ServiceName { get; set; }
        [Display(Name = "Service LOaction")]
        public string ServiceLocation { get; set; }
        [Display(Name = "Service Type")]
        public string ServiceType { get; set; }
        [Display(Name = "Latitude ")]
        public float Latval { get; set; }
        [Display(Name = "Longitude ")]
        public float Lngval { get; set; }


        [Display(Name = "DateTime")]
        public string DateTime { get; set; }
        public string Count { get; set; }

    }
  
}