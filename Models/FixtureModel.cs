using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlayerHelp.Models
{
    public class FixtureModel
    {
        public int FixtureID { get; set; }
        public DateTime FixtureTime { get; set; }
        public string City { get; set; }
        public string Addresse { get; set; }
    }
}