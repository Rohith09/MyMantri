using System;
using System.Collections.Generic;

namespace MyMantriDataAccessLayer.Models
{
    public partial class Constituency
    {
        public Constituency()
        {
            Complaint = new HashSet<Complaint>();
            Voters = new HashSet<Voters>();
        }

        public string ConstituencyName { get; set; }
        public string MantriUid { get; set; }
        public string Status { get; set; }

        public Mantri MantriConstituencyNavigation { get; set; }
        public Mantri MantriMantriU { get; set; }
        public ICollection<Complaint> Complaint { get; set; }
        public ICollection<Voters> Voters { get; set; }
    }
}
