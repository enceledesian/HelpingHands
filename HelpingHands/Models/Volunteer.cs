using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpingHands.Models
{
    public partial class Volunteer
    {
        [Key]
        public int VolunteerId { get; set; }
        [Required(ErrorMessage = "Please enter firstname")]
        [StringLength(50)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please enter lastname")]
        [StringLength(50)]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please enter valid email")]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Email Id")]
        [StringLength(50)]
        public string EmailId { get; set; }
        [Required(ErrorMessage = "Please select gender")]
        [StringLength(50)]
        public string Gender { get; set; }
        public int? Age { get; set; }
        [StringLength(50)]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
                   ErrorMessage = "Entered phone format is not valid.")]
        [DisplayName("Contact Number")]
        public string MobileNumber { get; set; }
        public bool? IsActive { get; set; }
    }
}
