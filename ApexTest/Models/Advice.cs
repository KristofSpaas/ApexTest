using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ApexTest.Models
{
    public class Advice
    {
        [Key]
        public int AdviceId { get; set; }

        [Required]
        [StringLength(100)]
        public String AdviceTitle { get; set; }

        [Required]
        [StringLength(500)]
        public String AdviceContent { get; set; }

        // Foreign key 
        [Required]
        public int PatientId { get; set; }

        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }
    }
}