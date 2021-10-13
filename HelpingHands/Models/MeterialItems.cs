using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpingHands.Models
{
    public partial class MeterialItems
    {
        public MeterialItems()
        {
            DonationRequirement = new HashSet<DonationRequirement>();
        }
        [Key]
        public int MaterialItemId { get; set; }
        [Required(ErrorMessage = "Please enter category name")]
        [DisplayName("Category Name")]
        [StringLength(50)]
        public string MererialItemName { get; set; }
        public bool IsActive { get; set; }
        [InverseProperty("MaterialItem")]
        public virtual ICollection<DonationRequirement> DonationRequirement { get; set; }
    }
}
