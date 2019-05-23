using System;
using System.Collections.Generic;

namespace MyMantriDataAccessLayer.Models
{
    public partial class Feedback
    {
        public int ComplaintId { get; set; }
        public int? Rating { get; set; }
        public string FeedbackDesr { get; set; }

        public Complaint Complaint { get; set; }
    }
}
