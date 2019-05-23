using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyMantriDataAccessLayer.Models
{
    public partial class Complaint
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

        public Constituency ConstituencyNavigation { get; set; }
        public Voters Voter { get; set; }
        public Feedback Feedback { get; set; }
    }
}
