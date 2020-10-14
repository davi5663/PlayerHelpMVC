using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;

namespace PlayerHelp.Models
{
    public class PlayerModel
    {
        public int PlayerLoginID { get; set; }

        [Display(Name = "Username")]
        [Required(ErrorMessage = "You need to write a Username.")]
        public string Username { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "You need to give us your Email address.")]
        public string EmailAddress { get; set; }

        [Display(Name = "Confirm Email")]
        [Compare("EmailAddress", ErrorMessage = "The Email and Confirm Email musth match")]
        public string ConfirmEmail { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "You need to write a Password.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Your password is not long enough")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Your passsword and confirm password do not match")]
        public string ConfirmPassword { get; set; }

        public string LoginErrorMsg { get; set; }
        public string Position { get; set; }
    }
}