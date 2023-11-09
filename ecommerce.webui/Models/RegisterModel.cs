using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Hosting;

namespace ecommerce.webui.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "İsim Alanı Zorunludur.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyisim Alanı Zorunludur.")]
        public string LastName { get; set; }

        // public string UserName { get; set; }


        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifre Alanı Zorunludur.")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
 [Required(ErrorMessage = "Şifreyi Tekrarlama Alanı Zorunludur.")]
        [Compare("Password")]
        public string RePassword { get; set; }
         
        [Required(ErrorMessage = "Email Alanı Zorunludur.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
    }
}