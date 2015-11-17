﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ApexTest.Models
{
    public class Temperature
    {
        [Key]
        public int TemperatureId { get; set; }

        [Required]
        public float TemperatureFloat { get; set; }

        [Required]
        public long DateMillis { get; set; }

        // Foreign key 
        [Required]
        public int PatientId { get; set; }

        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }
    }
}