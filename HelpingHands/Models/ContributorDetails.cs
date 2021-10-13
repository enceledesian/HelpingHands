using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpingHands.Models
{
    public partial class ContributorDetails
    {
        [Key]
        public int ContributorId { get; set; }
        public string UserId { get; set; }
        [Column(TypeName = "date")]
        public DateTime DispatchDate { get; set; }
        [Required]
        [StringLength(50)]
        public string RecipientInfo { get; set; }
    }
}
