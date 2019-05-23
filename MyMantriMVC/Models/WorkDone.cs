using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyMantriMVC.Models
{
    public class WorkDone
    {
        public int WorkId { get; set; }
        public int MantriId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string WorkDesr { get; set; }

    }
}
