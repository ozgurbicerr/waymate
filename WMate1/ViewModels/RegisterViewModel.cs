using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WMate1.ViewModels
{
    public class RegisterViewModel
    {
        [DisplayName("Username"), Required(ErrorMessage = "{0} You must pick a username."),
         StringLength(25, ErrorMessage = "{0} must be max {1} character long.")]
        public string Username { get; set; }
        [DisplayName("E-Mail"), Required(ErrorMessage = "{0} This field cannot be blank."),
         StringLength(70, ErrorMessage = "{0} max {1} karakter uzunluğunda olmalıdır."),
         EmailAddress(ErrorMessage = "{0} address is not valid.")]
        public string Email { get; set; }
        [DisplayName("Password"), Required(ErrorMessage = "{0} This field cannot be blank."), DataType(DataType.Password),
         StringLength(26, ErrorMessage = "{0} max {1} karakter uzunluğunda olmalıdır.")]
        public string Password { get; set; }
        [DisplayName("Re-Password"), Required(ErrorMessage = "{0} This field cannot be blank."), DataType(DataType.Password),
         StringLength(26, ErrorMessage = "{0} max {1} karakter uzunluğunda olmalıdır."),
         Compare("Password", ErrorMessage = "{0} and {1} doesn't match.")]
        public string RePassword { get; set; }

    }
}