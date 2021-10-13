using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HelpingHands.Models.ViewModels
{
    public class Register
    {
        [Required(ErrorMessage = "Please enter email")]
        [DisplayName("Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter mobile number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
                   ErrorMessage = "Entered phone format is not valid.")]
        [DisplayName("Mobile Number")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "Please enter full name")]
        [DisplayName("Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter confirm password")]
        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]
        [Compare("Password",ErrorMessage ="Password and Confirmation Password do not match")]
        public string ConfirmPassword { get; set; }
    }
}
