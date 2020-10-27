using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;

namespace PlayerHelpMVC.Models
{
    public class AdminModel
    {
        public int AdminID { get; set; }

        [Display(Name = "Username")]
        [Required(ErrorMessage = "You need to write a Username.")]
        public string AdminUsername { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "You need to write a Password.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Your password is not long enough")]
        public string AdminPassword { get; set; }
    }
}