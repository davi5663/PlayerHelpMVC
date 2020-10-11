using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PlayerHelp.Models
{
    public class ClubModel
    {

        public int ClubID { get; set; }
        public string ClubName { get; set; }
        public string City { get; set; }

    }
}