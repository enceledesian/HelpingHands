using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HelpingHands.Models.ViewModels
{
    public class DonationRequirementViewModel
    {
        [Key]
        public int DonationRequirementId { get; set; }
        [Required(ErrorMessage = "Please enter firstname")]
        [StringLength(50)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please enter lastname")]
        [StringLength(50)]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please enter age")]
        public int Age { get; set; }
        [Required(ErrorMessage = "Please select any item")]
        [DisplayName("Material Item")]
        public int MaterialItemId { get; set; }
        [Required(ErrorMessage = "Please enter description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please enter valid address")]
        [StringLength(255)]
        public string Address { get; set; }
        [StringLength(50)]
        [DisplayName("Profile Photo")]
        public IFormFile Photo { get; set; }
        [Column(TypeName = "date")]
        [DisplayName("Delivery Date")]
        public DateTime? DeliveryDate { get; set; }
        [DisplayName("Delivery Status")]
        [StringLength(50)]
        public string DeliveryStatus { get; set; }
        [DisplayName("Recipient")]
        public string RecipientInfo { get; set; }
    }
}
