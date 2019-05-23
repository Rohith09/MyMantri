using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyMantriMVC.Models
{
    public class Complaint
    {
        public int ComplaintId { get; set; }
        public int VoterId { get; set; }
        public string Constituency { get; set; }
        public DateTime ComplaintDateTime { get; set; }
        [Required]
        
        public string Category { get; set; }
        [Required]
        public string Description { get; set; }
        public string Status { get; set; }
    }
}
