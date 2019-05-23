using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyMantriDataAccessLayer.Models
{
    public partial class Workdone
    {
        public int WorkId { get; set; }
        public int MantriId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string WorkDesr { get; set; }

        public Mantri Mantri { get; set; }
    }
}
