using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shop.Models
{
    public class LoginModel
    {
        [Display(Name ="User Name")]
        [Required(ErrorMessage ="Input your user name!")]
        public string UserName { get; set; }
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Input your password!")]
        public string Password { get; set; }
    }
}