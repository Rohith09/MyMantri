using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyMantriMVC.Models
{
    public class Feedback
    {
        public int ComplaintId { get; set; }
        [Required]
        public int? Rating { get; set; }        
        public string FeedbackDesr { get; set; }
    }
}
