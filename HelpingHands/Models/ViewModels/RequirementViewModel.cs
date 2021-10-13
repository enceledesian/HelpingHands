using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HelpingHands.Models.ViewModels
{
    public class RequirementViewModel
    {
        public int DonationRequirementId { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        public int Age { get; set; }
        [DisplayName("Material Item")]
        public string MaterialItem { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        [DisplayName("Profile Photo")]
        public string Photo { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Delivery Date")]
        public DateTime? DeliveryDate { get; set; }
        [DisplayName("Delivery Status")]
        public string DeliveryStatus { get; set; }
        [DisplayName("Recipient")]
        public string RecipientInfo { get; set; }
        public string CategoryName { get; set; }
    }
}
