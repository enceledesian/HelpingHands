using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HelpingHands.Models.ViewModels
{
    public class Login
    {
        [Required(ErrorMessage = "Please enter user name")]
        [DisplayName("Email Id")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
